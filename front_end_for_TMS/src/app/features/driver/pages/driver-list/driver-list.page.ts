import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule, TableLazyLoadEvent } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { MessageService, ConfirmationService } from 'primeng/api';
import { DriverService } from '@features/driver/services/driver.service';
import { DriverDto } from '@features/driver/models/driver.models';
import { DriverFormDialog } from '@features/driver/components/driver-form-dialog/driver-form-dialog';

const STATUS_MAP: Record<number, { label: string; severity: 'success' | 'warn' | 'danger' | 'secondary' }> = {
  1: { label: 'Active', severity: 'success' },
  2: { label: 'On Leave', severity: 'warn' },
  3: { label: 'Terminated', severity: 'danger' },
};

const STATUS_FILTER_OPTIONS = [
  { label: 'All', value: null },
  { label: 'Active', value: 1 },
  { label: 'On Leave', value: 2 },
  { label: 'Terminated', value: 3 },
];

@Component({
  selector: 'app-driver-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    TagModule,
    TooltipModule,
    ToastModule,
    ConfirmDialogModule,
    InputTextModule,
    SelectModule,
    DriverFormDialog,
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './driver-list.page.html',
})
export class DriverListPage {
  private readonly driverService = inject(DriverService);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  // Table states
  drivers = signal<DriverDto[]>([]);
  totalRecords = signal(0);
  loading = signal(true);
  rows = signal(10);
  first = signal(0);
  readonly rowsPerPageOptions = [5, 10, 20];

  // Search & filter states
  searchTerm = signal('');
  statusFilter = signal<number | null>(null);
  readonly statusFilterOptions = STATUS_FILTER_OPTIONS;

  // Dialog states
  formDialogVisible = signal(false);
  editingDriver = signal<DriverDto | null>(null);
  submitting = signal(false);

  // Table handlers
  loadDrivers(event: TableLazyLoadEvent): void {
    this.loading.set(true);
    const size = event.rows ?? this.rows();
    const page = Math.floor((event.first ?? 0) / size) + 1;

    this.driverService.list(page, size, this.statusFilter() ?? undefined, this.searchTerm() || undefined).subscribe({
      next: (res) => {
        this.drivers.set(res.data);
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
    this.loadDrivers({ first: this.first(), rows: this.rows() });
  }

  onSearch(): void {
    this.loadDrivers({ first: 0, rows: this.rows() });
  }

  onStatusFilterChange(value: number | null): void {
    this.statusFilter.set(value);
    this.loadDrivers({ first: 0, rows: this.rows() });
  }

  // Status helpers
  getStatusLabel(status: number): string {
    return STATUS_MAP[status]?.label ?? 'Unknown';
  }

  getStatusSeverity(status: number): 'success' | 'warn' | 'danger' | 'secondary' {
    return STATUS_MAP[status]?.severity ?? 'secondary';
  }

  // Form dialog handlers
  onAdd(): void {
    this.editingDriver.set(null);
    this.formDialogVisible.set(true);
  }

  onEdit(driver: DriverDto): void {
    this.editingDriver.set(driver);
    this.formDialogVisible.set(true);
  }

  onFormSaved(payload: Partial<DriverDto>): void {
    if (this.submitting()) return;
    this.submitting.set(true);

    const editDriver = this.editingDriver();
    const isEdit = editDriver !== null;
    const op$ = isEdit
      ? this.driverService.update(editDriver.driverId, payload)
      : this.driverService.create(payload);

    op$.subscribe({
      next: () => {
        this.formDialogVisible.set(false);
        this.editingDriver.set(null);
        this.submitting.set(false);
        this.refreshTable();
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: isEdit ? 'Driver updated successfully.' : 'Driver created successfully.',
          life: 3000,
        });
      },
      error: () => {
        this.submitting.set(false);
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
    this.editingDriver.set(null);
  }

  // Delete handlers
  onDelete(driver: DriverDto): void {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete driver <b>${driver.fullName}</b>?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', text: true },
      acceptButtonProps: { label: 'Delete', severity: 'danger' },
      accept: () => {
        this.loading.set(true);
        this.driverService.delete(driver.driverId).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Deleted',
              detail: `Driver ${driver.fullName} deleted.`,
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
