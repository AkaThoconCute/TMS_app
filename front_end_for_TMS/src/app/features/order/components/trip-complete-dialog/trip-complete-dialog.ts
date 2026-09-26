import { Component, effect, inject, input, model, output, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { DatePickerModule } from 'primeng/datepicker';
import { TextareaModule } from 'primeng/textarea';
import { TripDto, CompleteTripDto } from '../../models/order.models';

@Component({
  selector: 'app-trip-complete-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    InputNumberModule,
    DatePickerModule,
    TextareaModule,
  ],
  templateUrl: './trip-complete-dialog.html',
})
export class TripCompleteDialog {
  private readonly fb = inject(FormBuilder);

  visible = model(false);
  trip = input<TripDto | null>(null);
  submitting = input(false);

  saved = output<CompleteTripDto>();
  closed = output<{ hadUnsavedChanges: boolean }>();

  form: FormGroup = this.fb.group({
    actualDeliveryDate: [null as Date | null, Validators.required],
    fuelCost: [null as number | null],
    tollCost: [null as number | null],
    otherCost: [null as number | null],
    costNotes: [''],
    notes: [''],
  });

  get dialogTitle(): string {
    const t = this.trip();
    return t ? `Complete Trip ${t.tripNumber}` : 'Complete Trip';
  }

  constructor() {
    let previouslyVisible = false;
    effect(() => {
      const isVisible = this.visible();
      if (isVisible && !previouslyVisible) {
        this.form.reset({ actualDeliveryDate: new Date() });
      }
      previouslyVisible = isVisible;
    });
  }

  onSubmit(): void {
    if (this.form.invalid || this.submitting()) return;
    const t = this.trip();
    if (!t) return;

    const raw = this.form.getRawValue();
    const payload: CompleteTripDto = {
      tripId: t.tripId,
      actualDeliveryDate: (raw.actualDeliveryDate as Date).toISOString(),
      fuelCost: raw.fuelCost ?? undefined,
      tollCost: raw.tollCost ?? undefined,
      otherCost: raw.otherCost ?? undefined,
      costNotes: raw.costNotes || undefined,
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
