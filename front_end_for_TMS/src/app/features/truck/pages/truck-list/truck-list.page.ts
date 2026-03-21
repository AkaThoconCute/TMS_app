import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TableModule, TableLazyLoadEvent } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { TruckService } from '@features/truck/services/truck.service';
import { TruckDto } from '@features/truck/models/truck.models';

const STATUS_MAP: Record<number, { label: string; severity: 'success' | 'info' | 'warn' | 'danger' | 'secondary' }> = {
  1: { label: 'Available', severity: 'success' },
  2: { label: 'In Use', severity: 'info' },
  3: { label: 'Maintenance', severity: 'warn' },
  4: { label: 'Broken Down', severity: 'danger' },
  5: { label: 'Retired', severity: 'secondary' },
};

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

const OWNERSHIP_MAP: Record<number, string> = {
  1: 'Owned',
  2: 'Leased',
};

@Component({
  selector: 'app-truck-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    TagModule,
    TooltipModule,
    DialogModule,
    InputTextModule,
    InputNumberModule,
    SelectModule,
    DatePickerModule,
    ToastModule,
  ],
  providers: [MessageService],
  templateUrl: './truck-list.page.html',
  styleUrl: './truck-list.page.css',
})
export class TruckListPage {
  private readonly truckService = inject(TruckService);
  private readonly messageService = inject(MessageService);
  private readonly fb = inject(FormBuilder);

  // Table state
  trucks = signal<TruckDto[]>([]);
  totalRecords = signal(0);
  loading = signal(true);
  rows = signal(3);
  first = signal(0);
  readonly rowsPerPageOptions = [2, 3, 4];

  // Dialog state
  dialogVisible = signal(false);
  editingTruck = signal<TruckDto | null>(null);
  submitting = signal(false);

  readonly ownershipOptions = OWNERSHIP_OPTIONS;
  readonly truckTypeOptions = TRUCK_TYPE_OPTIONS;

  // Form
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

  get isEditMode(): boolean {
    return this.editingTruck() !== null;
  }

  get dialogTitle(): string {
    return this.isEditMode ? 'Edit Truck' : 'Add New Truck';
  }

  // ─── Table ────────────────────────────────────────────
  loadTrucks(event: TableLazyLoadEvent): void {
    this.loading.set(true);
    const page = Math.floor((event.first ?? 0) / (event.rows ?? this.rows())) + 1;
    const size = event.rows ?? this.rows();

    this.truckService.list(page, size).subscribe({
      next: (res) => {
        this.trucks.set(res.data);
        this.totalRecords.set(res.totalCount);
        this.first.set(event.first ?? 0);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }

  private refreshTable(): void {
    this.loadTrucks({ first: this.first(), rows: this.rows() });
  }

  // ─── Status helpers ───────────────────────────────────
  getStatusLabel(status: number): string {
    return STATUS_MAP[status]?.label ?? 'Unknown';
  }

  getStatusSeverity(status: number): 'success' | 'info' | 'warn' | 'danger' | 'secondary' {
    return STATUS_MAP[status]?.severity ?? 'secondary';
  }

  getOwnershipLabel(type: number): string {
    return OWNERSHIP_MAP[type] ?? 'Unknown';
  }

  // ─── Dialog open / close ──────────────────────────────
  onAdd(): void {
    this.editingTruck.set(null);
    this.form.reset({ ownershipType: 1 });
    this.dialogVisible.set(true);
  }

  onEdit(truck: TruckDto): void {
    this.editingTruck.set(truck);
    this.form.patchValue({
      licensePlate: truck.licensePlate,
      vinNumber: truck.vinNumber ?? '',
      engineNumber: truck.engineNumber ?? '',
      brand: truck.brand ?? '',
      modelYear: truck.modelYear ?? null,
      purchaseDate: truck.purchaseDate ? new Date(truck.purchaseDate) : null,
      truckType: truck.truckType ?? null,
      maxPayloadKg: truck.maxPayloadKg ?? null,
      lengthMm: truck.lengthMm ?? null,
      widthMm: truck.widthMm ?? null,
      heightMm: truck.heightMm ?? null,
      ownershipType: truck.ownershipType,
    });
    this.form.markAsPristine();
    this.dialogVisible.set(true);
  }

  onDialogHide(): void {
    // If currently submitting, let submit flow handle everything
    if (this.submitting()) return;

    if (this.form.dirty) {
      this.messageService.add({
        severity: 'info',
        summary: 'Not Saved',
        detail: 'Changes were discarded.',
        life: 3000,
      });
    }
    this.resetDialog();
  }

  private resetDialog(): void {
    this.editingTruck.set(null);
    this.form.reset({ ownershipType: 1 });
  }

  // ─── Submit ───────────────────────────────────────────
  onSubmit(): void {
    if (this.form.invalid || this.submitting()) return;
    this.submitting.set(true);

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

    const isEdit = this.isEditMode;
    const op$ = isEdit
      ? this.truckService.update(this.editingTruck()!.truckId, payload)
      : this.truckService.create(payload);

    // Close dialog immediately — table shows loading until refresh completes
    this.dialogVisible.set(false);
    this.resetDialog();
    this.loading.set(true);

    op$.subscribe({
      next: () => {
        this.submitting.set(false);
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: isEdit ? 'Truck updated successfully.' : 'Truck created successfully.',
          life: 3000,
        });
        this.refreshTable();
      },
      error: () => {
        this.submitting.set(false);
        this.loading.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Operation failed. Please try again.',
          life: 5000,
        });
      },
    });
  }

  onDelete(_truck: TruckDto): void {
    // no-logic placeholder
  }
}
