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
import { CustomerService } from '@features/customer/services/customer.service';
import { CustomerDto } from '@features/customer/models/customer.models';
import { CustomerFormDialog } from '@features/customer/components/customer-form-dialog/customer-form-dialog';

const STATUS_MAP: Record<number, { label: string; severity: 'success' | 'warn' | 'danger' | 'secondary' }> = {
  1: { label: 'Active', severity: 'success' },
  2: { label: 'Inactive', severity: 'secondary' },
};

const STATUS_FILTER_OPTIONS = [
  { label: 'All', value: null },
  { label: 'Active', value: 1 },
  { label: 'Inactive', value: 2 },
];

const CUSTOMER_TYPE_MAP: Record<number, { label: string; severity: 'info' | 'contrast' | 'success' | 'warn' | 'danger' | 'secondary' }> = {
  1: { label: 'Individual', severity: 'info' },
  2: { label: 'Business', severity: 'contrast' },
};

const CUSTOMER_TYPE_FILTER_OPTIONS = [
  { label: 'All', value: null },
  { label: 'Individual', value: 1 },
  { label: 'Business', value: 2 },
];

@Component({
  selector: 'app-customer-list',
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
    CustomerFormDialog,
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './customer-list.page.html',
})
export class CustomerListPage {
  private readonly customerService = inject(CustomerService);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  // Table states
  customers = signal<CustomerDto[]>([]);
  totalRecords = signal(0);
  loading = signal(true);
  rows = signal(10);
  first = signal(0);
  readonly rowsPerPageOptions = [5, 10, 20];

  // Search & filter states
  searchTerm = signal('');
  statusFilter = signal<number | null>(null);
  customerTypeFilter = signal<number | null>(null);
  readonly statusFilterOptions = STATUS_FILTER_OPTIONS;
  readonly customerTypeFilterOptions = CUSTOMER_TYPE_FILTER_OPTIONS;

  // Dialog states
  formDialogVisible = signal(false);
  editingCustomer = signal<CustomerDto | null>(null);
  submitting = signal(false);

  // Table handlers
  loadCustomers(event: TableLazyLoadEvent): void {
    this.loading.set(true);
    const size = event.rows ?? this.rows();
    const page = Math.floor((event.first ?? 0) / size) + 1;

    this.customerService.list(
      page,
      size,
      this.statusFilter() ?? undefined,
      this.customerTypeFilter() ?? undefined,
      this.searchTerm() || undefined
    ).subscribe({
      next: (res) => {
        this.customers.set(res.data);
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
    this.loadCustomers({ first: this.first(), rows: this.rows() });
  }

  onSearch(): void {
    this.loadCustomers({ first: 0, rows: this.rows() });
  }

  onStatusFilterChange(value: number | null): void {
    this.statusFilter.set(value);
    this.loadCustomers({ first: 0, rows: this.rows() });
  }

  onCustomerTypeFilterChange(value: number | null): void {
    this.customerTypeFilter.set(value);
    this.loadCustomers({ first: 0, rows: this.rows() });
  }

  // Status helpers
  getStatusLabel(status: number): string {
    return STATUS_MAP[status]?.label ?? 'Unknown';
  }

  getStatusSeverity(status: number): 'success' | 'warn' | 'danger' | 'secondary' {
    return STATUS_MAP[status]?.severity ?? 'secondary';
  }

  // Customer type helpers
  getCustomerTypeLabel(customerType: number): string {
    return CUSTOMER_TYPE_MAP[customerType]?.label ?? 'Unknown';
  }

  getCustomerTypeSeverity(customerType: number): 'info' | 'contrast' | 'success' | 'warn' | 'danger' | 'secondary' {
    return CUSTOMER_TYPE_MAP[customerType]?.severity ?? 'secondary';
  }

  // Form dialog handlers
  onAdd(): void {
    this.editingCustomer.set(null);
    this.formDialogVisible.set(true);
  }

  onEdit(customer: CustomerDto): void {
    this.editingCustomer.set(customer);
    this.formDialogVisible.set(true);
  }

  onFormSaved(payload: Partial<CustomerDto>): void {
    if (this.submitting()) return;
    this.submitting.set(true);

    const editCustomer = this.editingCustomer();
    const isEdit = editCustomer !== null;
    const op$ = isEdit
      ? this.customerService.update(editCustomer.customerId, payload)
      : this.customerService.create(payload);

    op$.subscribe({
      next: () => {
        this.formDialogVisible.set(false);
        this.editingCustomer.set(null);
        this.submitting.set(false);
        this.refreshTable();
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: isEdit ? 'Customer updated successfully.' : 'Customer created successfully.',
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
    this.editingCustomer.set(null);
  }

  // Delete handlers
  onDelete(customer: CustomerDto): void {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete customer <b>${customer.name}</b>?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', text: true },
      acceptButtonProps: { label: 'Delete', severity: 'danger' },
      accept: () => {
        this.loading.set(true);
        this.customerService.delete(customer.customerId).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Deleted',
              detail: `Customer ${customer.name} deleted.`,
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
