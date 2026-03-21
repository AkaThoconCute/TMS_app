import { Component, inject, input, output, signal, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { TruckDto } from '@features/truck/models/truck.models';

const OWNERSHIP_OPTIONS = [
  { label: 'Owned', value: 1 },
  { label: 'Leased', value: 2 },
];

const TRUCK_TYPE_OPTIONS = [
  { label: 'Thùng kín', value: 'Thùng kín' },
  { label: 'Mui bạt', value: 'Mui bạt' },
  { label: 'Bồn', value: 'Bồn' },
  { label: 'Cẩu', value: 'Cẩu' },
  { label: 'Tự đổ', value: 'Tự đổ' },
];

@Component({
  selector: 'app-truck-form-dialog',
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
  ],
  templateUrl: './truck-form-dialog.html',
})
export class TruckFormDialog implements OnChanges {
  private readonly fb = inject(FormBuilder);

  /** The truck to edit, or null for create mode. */
  truck = input<TruckDto | null>(null);
  /** Controls dialog visibility from parent. */
  visible = input(false);
  /** Whether a submit is in progress (blocks the button). */
  submitting = input(false);

  /** Emits the form payload when user submits. */
  saved = output<Partial<TruckDto>>();
  /** Emits when user closes/cancels — includes whether unsaved changes existed. */
  closed = output<{ hadUnsavedChanges: boolean }>();

  readonly ownershipOptions = OWNERSHIP_OPTIONS;
  readonly truckTypeOptions = TRUCK_TYPE_OPTIONS;

  form: FormGroup = this.fb.group({
    licensePlate: ['', Validators.required],
    vinNumber: [''],
    engineNumber: [''],
    brand: [''],
    modelYear: [null as number | null],
    purchaseDate: [null as Date | null],
    truckType: [null as string | null],
    maxPayloadKg: [null as number | null],
    lengthMm: [null as number | null],
    widthMm: [null as number | null],
    heightMm: [null as number | null],
    ownershipType: [1],
  });

  dialogVisible = signal(false);

  get isEditMode(): boolean {
    return this.truck() !== null;
  }

  get dialogTitle(): string {
    return this.isEditMode ? 'Edit Truck' : 'Add New Truck';
  }

  ngOnChanges(): void {
    if (this.visible()) {
      this.dialogVisible.set(true);
      const t = this.truck();
      if (t) {
        this.form.patchValue({
          licensePlate: t.licensePlate,
          vinNumber: t.vinNumber ?? '',
          engineNumber: t.engineNumber ?? '',
          brand: t.brand ?? '',
          modelYear: t.modelYear ?? null,
          purchaseDate: t.purchaseDate ? new Date(t.purchaseDate) : null,
          truckType: t.truckType ?? null,
          maxPayloadKg: t.maxPayloadKg ?? null,
          lengthMm: t.lengthMm ?? null,
          widthMm: t.widthMm ?? null,
          heightMm: t.heightMm ?? null,
          ownershipType: t.ownershipType,
        });
        this.form.markAsPristine();
      } else {
        this.form.reset({ ownershipType: 1 });
      }
    } else {
      this.dialogVisible.set(false);
    }
  }

  onSubmit(): void {
    if (this.form.invalid || this.submitting()) return;

    const raw = this.form.getRawValue();
    const payload: Partial<TruckDto> = {
      licensePlate: raw.licensePlate,
      vinNumber: raw.vinNumber || undefined,
      engineNumber: raw.engineNumber || undefined,
      brand: raw.brand || undefined,
      modelYear: raw.modelYear ?? undefined,
      purchaseDate: raw.purchaseDate ? (raw.purchaseDate as Date).toISOString() : undefined,
      truckType: raw.truckType ?? undefined,
      maxPayloadKg: raw.maxPayloadKg ?? undefined,
      lengthMm: raw.lengthMm ?? undefined,
      widthMm: raw.widthMm ?? undefined,
      heightMm: raw.heightMm ?? undefined,
      ownershipType: raw.ownershipType,
    };
    this.form.markAsPristine();
    this.saved.emit(payload);
  }

  onDialogHide(): void {
    if (this.submitting()) return;
    this.closed.emit({ hadUnsavedChanges: this.form.dirty });
  }
}
