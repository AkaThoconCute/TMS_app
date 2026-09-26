import { Component, computed, effect, inject, input, model, output, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { TextareaModule } from 'primeng/textarea';
import { TripDto, TripFormPayload } from '../../models/order.models';
import { TruckDto } from '@features/truck/models/truck.models';
import { DriverDto } from '@features/driver/models/driver.models';

@Component({
  selector: 'app-trip-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DialogModule,
    ButtonModule,
    SelectModule,
    DatePickerModule,
    TextareaModule,
  ],
  templateUrl: './trip-form-dialog.html',
})
export class TripFormDialog {
  private readonly fb = inject(FormBuilder);

  visible = model(false);
  trip = input<TripDto | null>(null);
  orderNumber = input('');
  trucks = input<TruckDto[]>([]);
  drivers = input<DriverDto[]>([]);
  submitting = input(false);

  saved = output<TripFormPayload>();
  closed = output<{ hadUnsavedChanges: boolean }>();

  activeDrivers = computed(() => this.drivers().filter(d => d.status === 1));

  form: FormGroup = this.fb.group({
    truckId: [null as string | null, Validators.required],
    driverId: [null as string | null, Validators.required],
    plannedPickupDate: [null as Date | null],
    plannedDeliveryDate: [null as Date | null],
    notes: [''],
  });

  get isEditMode(): boolean {
    return this.trip() !== null;
  }

  get dialogTitle(): string {
    return this.isEditMode ? 'Edit Trip' : 'Assign Truck & Driver';
  }

  constructor() {
    let previouslyVisible = false;
    effect(() => {
      const isVisible = this.visible();
      if (isVisible && !previouslyVisible) {
        const t = untracked(() => this.trip());
        if (t) {
          this.form.patchValue({
            truckId: t.truckId,
            driverId: t.driverId,
            plannedPickupDate: t.plannedPickupDate ? new Date(t.plannedPickupDate) : null,
            plannedDeliveryDate: t.plannedDeliveryDate ? new Date(t.plannedDeliveryDate) : null,
            notes: t.notes ?? '',
          });
          this.form.markAsPristine();
        } else {
          this.form.reset();
        }
        this.form.enable();
      }
      previouslyVisible = isVisible;
    });
  }

  onSubmit(): void {
    if (this.form.invalid || this.submitting()) return;

    const raw = this.form.getRawValue();
    const payload: TripFormPayload = {
      truckId: raw.truckId,
      driverId: raw.driverId,
      plannedPickupDate: raw.plannedPickupDate
        ? (raw.plannedPickupDate as Date).toISOString()
        : undefined,
      plannedDeliveryDate: raw.plannedDeliveryDate
        ? (raw.plannedDeliveryDate as Date).toISOString()
        : undefined,
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
