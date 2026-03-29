import { Component, effect, inject, input, model, output, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { CustomerDto } from '@features/customer/models/customer.models';

const CUSTOMER_TYPE_OPTIONS = [
  { label: 'Individual', value: 1 },
  { label: 'Business', value: 2 },
];

const STATUS_OPTIONS = [
  { label: 'Active', value: 1 },
  { label: 'Inactive', value: 2 },
];

@Component({
  selector: 'app-customer-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    SelectModule,
    TextareaModule,
  ],
  templateUrl: './customer-form-dialog.html',
})
export class CustomerFormDialog {
  private readonly formBuilder = inject(FormBuilder);

  visible = model(false);
  customer = input<CustomerDto | null>(null);
  submitting = input(false);

  saved = output<Partial<CustomerDto>>();
  closed = output<{ hadUnsavedChanges: boolean }>();

  readonly customerTypeOptions = CUSTOMER_TYPE_OPTIONS;
  readonly statusOptions = STATUS_OPTIONS;

  form: FormGroup = this.formBuilder.group({
    name: ['', Validators.required],
    contactPerson: [''],
    phoneNumber: ['', Validators.required],
    email: ['', [Validators.email]],
    address: [''],
    taxCode: [''],
    customerType: [1],
    status: [1],
    notes: [''],
  });

  get isEditMode(): boolean {
    return this.customer() !== null;
  }

  get dialogTitle(): string {
    return this.isEditMode ? 'Edit Customer' : 'Add New Customer';
  }

  constructor() {
    let previouslyVisible = false;
    effect(() => {
      const isVisible = this.visible();
      if (isVisible && !previouslyVisible) {
        const c = untracked(() => this.customer());
        if (c) {
          this.form.patchValue({
            name: c.name,
            contactPerson: c.contactPerson ?? '',
            phoneNumber: c.phoneNumber,
            email: c.email ?? '',
            address: c.address ?? '',
            taxCode: c.taxCode ?? '',
            customerType: c.customerType,
            status: c.status,
            notes: c.notes ?? '',
          });
          this.form.markAsPristine();
        } else {
          this.form.reset({ customerType: 1, status: 1 });
        }
      }
      previouslyVisible = isVisible;
    });
  }

  onSubmit(): void {
    if (this.form.invalid || this.submitting()) return;

    const raw = this.form.getRawValue();
    const payload: Partial<CustomerDto> = {
      name: raw.name,
      contactPerson: raw.contactPerson || undefined,
      phoneNumber: raw.phoneNumber,
      email: raw.email || undefined,
      address: raw.address || undefined,
      taxCode: raw.taxCode || undefined,
      customerType: raw.customerType,
      status: raw.status,
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
