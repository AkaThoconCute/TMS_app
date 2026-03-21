import { Component, effect, inject, input, model, output, untracked } from '@angular/core';
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
export class TruckFormDialog {
  private readonly formBuilder = inject(FormBuilder);

  visible = model(false);
  truck = input<TruckDto | null>(null);
  submitting = input(false);

  saved = output<Partial<TruckDto>>();
  closed = output<{ hadUnsavedChanges: boolean }>();

  readonly ownershipOptions = OWNERSHIP_OPTIONS;
  readonly truckTypeOptions = TRUCK_TYPE_OPTIONS;

  form: FormGroup = this.formBuilder.group({
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

  get isEditMode(): boolean {
    return this.truck() !== null;
  }

  get dialogTitle(): string {
    return this.isEditMode ? 'Edit Truck' : 'Add New Truck';
  }

  constructor() {
    let previouslyVisible = false;
    effect(() => {
      const isVisible = this.visible();
      if (isVisible && !previouslyVisible) {
        const t = untracked(() => this.truck());
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
      }
      previouslyVisible = isVisible;
    });
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
