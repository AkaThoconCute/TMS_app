import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule, TableLazyLoadEvent } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { TruckService } from '@features/truck/services/truck.service';
import { TruckDto } from '@features/truck/models/truck.models';
import { TruckFormDialog } from '@features/truck/components/truck-form-dialog/truck-form-dialog';

const STATUS_MAP: Record<number, { label: string; severity: 'success' | 'info' | 'warn' | 'danger' | 'secondary' }> = {
  1: { label: 'Available', severity: 'success' },
  2: { label: 'In Use', severity: 'info' },
  3: { label: 'Maintenance', severity: 'warn' },
  4: { label: 'Broken Down', severity: 'danger' },
  5: { label: 'Retired', severity: 'secondary' },
};

const OWNERSHIP_MAP: Record<number, string> = {
  1: 'Owned',
  2: 'Leased',
};

@Component({
  selector: 'app-truck-list',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    TagModule,
    TooltipModule,
    ToastModule,
    ConfirmDialogModule,
    TruckFormDialog,
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './truck-list.page.html',
})
export class TruckListPage {
  private readonly truckService = inject(TruckService);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  // Table states
  trucks = signal<TruckDto[]>([]);
  totalRecords = signal(0);
  loading = signal(true);
  rows = signal(2);
  first = signal(0);
  readonly rowsPerPageOptions = [2, 3, 4];

  // Dialog states
  formDialogVisible = signal(false);
  editingTruck = signal<TruckDto | null>(null);
  submitting = signal(false);

  // Table handlers
  loadTrucks(event: TableLazyLoadEvent): void {
    this.loading.set(true);
    const size = event.rows ?? this.rows();
    const page = Math.floor((event.first ?? 0) / size) + 1;

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

  // Status helpers
  getStatusLabel(status: number): string {
    return STATUS_MAP[status]?.label ?? 'Unknown';
  }

  getStatusSeverity(status: number): 'success' | 'info' | 'warn' | 'danger' | 'secondary' {
    return STATUS_MAP[status]?.severity ?? 'secondary';
  }

  getOwnershipLabel(type: number): string {
    return OWNERSHIP_MAP[type] ?? 'Unknown';
  }

  // Form dialog handlers
  onAdd(): void {
    this.editingTruck.set(null);
    this.formDialogVisible.set(true);
  }

  onEdit(truck: TruckDto): void {
    this.editingTruck.set(truck);
    this.formDialogVisible.set(true);
  }

  onFormSaved(payload: Partial<TruckDto>): void {
    this.submitting.set(true);

    const editTruck = this.editingTruck();
    const isEdit = editTruck !== null;
    const op$ = isEdit
      ? this.truckService.update(editTruck.truckId, payload)
      : this.truckService.create(payload);

    this.formDialogVisible.set(false); // !
    this.editingTruck.set(null);
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

  onFormClosed(event: { hadUnsavedChanges: boolean }): void {
    if (event.hadUnsavedChanges) {
      this.messageService.add({
        severity: 'info',
        summary: 'Not Saved',
        detail: 'Changes were discarded.',
        life: 3000,
      });
    }
    this.formDialogVisible.set(false);
    this.editingTruck.set(null);
  }

  // Delete handlers
  onDelete(truck: TruckDto): void {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete truck <b>${truck.licensePlate}</b>?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', text: true },
      acceptButtonProps: { label: 'Delete', severity: 'danger' },
      accept: () => {
        this.loading.set(true);
        this.truckService.delete(truck.truckId).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Deleted',
              detail: `Truck ${truck.licensePlate} deleted.`,
              life: 3000,
            });
            this.refreshTable();
          },
          error: () => {
            this.loading.set(false);
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Delete failed. Please try again.',
              life: 5000,
            });
          },
        });
      },
    });
  }
}
