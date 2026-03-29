import { Component, computed, effect, inject, input, model, output, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { TextareaModule } from 'primeng/textarea';
import { TagModule } from 'primeng/tag';
import { OrderDto, CreateOrderDto } from '../../models/order.models';
import { CustomerDto } from '@features/customer/models/customer.models';

type TagSeverity = 'success' | 'secondary' | 'info' | 'warn' | 'danger' | 'contrast';

const ORDER_STATUS_SEVERITY: Record<number, TagSeverity> = {
  1: 'info',
  2: 'warn',
  3: 'contrast',
  4: 'success',
  5: 'secondary',
  6: 'danger',
};

@Component({
  selector: 'app-order-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    InputNumberModule,
    SelectModule,
    DatePickerModule,
    TextareaModule,
    TagModule,
  ],
  templateUrl: './order-form-dialog.html',
})
export class OrderFormDialog {
  private readonly fb = inject(FormBuilder);

  visible = model(false);
  order = input<OrderDto | null>(null);
  customers = input<CustomerDto[]>([]);
  submitting = input(false);

  saved = output<CreateOrderDto>();
  closed = output<{ hadUnsavedChanges: boolean }>();

  activeCustomers = computed(() => this.customers().filter(c => c.status === 1));

  form: FormGroup = this.fb.group({
    customerId: [null as string | null, Validators.required],
    pickupAddress: ['', Validators.required],
    deliveryAddress: ['', Validators.required],
    cargoDescription: ['', Validators.required],
    cargoWeightKg: [null as number | null],
    requestedPickupDate: [null as Date | null],
    requestedDeliveryDate: [null as Date | null],
    quotedPrice: [null as number | null],
    notes: [''],
  });

  get isEditMode(): boolean {
    return this.order() !== null;
  }

  get dialogTitle(): string {
    return this.isEditMode ? 'Edit Order' : 'Create New Order';
  }

  get isTerminal(): boolean {
    const o = this.order();
    return o !== null && (o.status === 5 || o.status === 6);
  }

  getStatusSeverity(status: number): TagSeverity {
    return ORDER_STATUS_SEVERITY[status] ?? 'secondary';
  }

  constructor() {
    let previouslyVisible = false;
    effect(() => {
      const isVisible = this.visible();
      if (isVisible && !previouslyVisible) {
        const o = untracked(() => this.order());
        if (o) {
          this.form.patchValue({
            customerId: o.customerId,
            pickupAddress: o.pickupAddress,
            deliveryAddress: o.deliveryAddress,
            cargoDescription: o.cargoDescription,
            cargoWeightKg: o.cargoWeightKg ?? null,
            requestedPickupDate: o.requestedPickupDate ? new Date(o.requestedPickupDate) : null,
            requestedDeliveryDate: o.requestedDeliveryDate ? new Date(o.requestedDeliveryDate) : null,
            quotedPrice: o.quotedPrice ?? null,
            notes: o.notes ?? '',
          });
          this.form.get('customerId')!.disable();
          if (this.isTerminal) {
            this.form.disable();
          }
          this.form.markAsPristine();
        } else {
          this.form.reset();
          this.form.get('customerId')!.enable();
          this.form.enable();
        }
      }
      previouslyVisible = isVisible;
    });
  }

  onSubmit(): void {
    if (this.form.invalid || this.submitting() || this.isTerminal) return;

    const raw = this.form.getRawValue();
    const payload: CreateOrderDto = {
      customerId: raw.customerId,
      pickupAddress: raw.pickupAddress,
      deliveryAddress: raw.deliveryAddress,
      cargoDescription: raw.cargoDescription,
      cargoWeightKg: raw.cargoWeightKg ?? undefined,
      requestedPickupDate: raw.requestedPickupDate
        ? (raw.requestedPickupDate as Date).toISOString()
        : undefined,
      requestedDeliveryDate: raw.requestedDeliveryDate
        ? (raw.requestedDeliveryDate as Date).toISOString()
        : undefined,
      quotedPrice: raw.quotedPrice ?? undefined,
      notes: raw.notes || undefined,
    };
    this.form.markAsPristine();
    this.saved.emit(payload);
  }

  onDialogHide(): void {
    if (this.submitting()) return;
    this.closed.emit({ hadUnsavedChanges: this.form.dirty });
  }
}
