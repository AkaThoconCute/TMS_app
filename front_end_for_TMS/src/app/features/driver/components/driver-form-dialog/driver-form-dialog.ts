import { Component, effect, inject, input, model, output, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { TextareaModule } from 'primeng/textarea';
import { DriverDto } from '@features/driver/models/driver.models';

const LICENSE_CLASS_OPTIONS = [
  { label: 'B2', value: 'B2' },
  { label: 'C', value: 'C' },
  { label: 'D', value: 'D' },
  { label: 'FC', value: 'FC' },
];

const STATUS_OPTIONS = [
  { label: 'Active', value: 1 },
  { label: 'On Leave', value: 2 },
  { label: 'Terminated', value: 3 },
];

@Component({
  selector: 'app-driver-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    SelectModule,
    DatePickerModule,
    TextareaModule,
  ],
  templateUrl: './driver-form-dialog.html',
})
export class DriverFormDialog {
  private readonly formBuilder = inject(FormBuilder);

  visible = model(false);
  driver = input<DriverDto | null>(null);
  submitting = input(false);

  saved = output<Partial<DriverDto>>();
  closed = output<{ hadUnsavedChanges: boolean }>();

  readonly licenseClassOptions = LICENSE_CLASS_OPTIONS;
  readonly statusOptions = STATUS_OPTIONS;

  form: FormGroup = this.formBuilder.group({
    fullName: ['', Validators.required],
    phoneNumber: ['', Validators.required],
    licenseNumber: ['', Validators.required],
    licenseClass: [null as string | null],
    licenseExpiry: [null as Date | null],
    dateOfBirth: [null as Date | null],
    status: [1],
    hireDate: [null as Date | null],
    notes: [''],
  });

  get isEditMode(): boolean {
    return this.driver() !== null;
  }

  get dialogTitle(): string {
    return this.isEditMode ? 'Edit Driver' : 'Add New Driver';
  }

  constructor() {
    let previouslyVisible = false;
    effect(() => {
      const isVisible = this.visible();
      if (isVisible && !previouslyVisible) {
        const d = untracked(() => this.driver());
        if (d) {
          this.form.patchValue({
            fullName: d.fullName,
            phoneNumber: d.phoneNumber,
            licenseNumber: d.licenseNumber,
            licenseClass: d.licenseClass ?? null,
            licenseExpiry: d.licenseExpiry ? new Date(d.licenseExpiry) : null,
            dateOfBirth: d.dateOfBirth ? new Date(d.dateOfBirth) : null,
            status: d.status,
            hireDate: d.hireDate ? new Date(d.hireDate) : null,
            notes: d.notes ?? '',
          });
          this.form.markAsPristine();
        } else {
          this.form.reset({ status: 1 });
        }
      }
      previouslyVisible = isVisible;
    });
  }

  onSubmit(): void {
    if (this.form.invalid || this.submitting()) return;

    const raw = this.form.getRawValue();
    const payload: Partial<DriverDto> = {
      fullName: raw.fullName,
      phoneNumber: raw.phoneNumber,
      licenseNumber: raw.licenseNumber,
      licenseClass: raw.licenseClass ?? undefined,
      licenseExpiry: raw.licenseExpiry ? (raw.licenseExpiry as Date).toISOString() : undefined,
      dateOfBirth: raw.dateOfBirth ? (raw.dateOfBirth as Date).toISOString() : undefined,
      status: raw.status,
      hireDate: raw.hireDate ? (raw.hireDate as Date).toISOString() : undefined,
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
