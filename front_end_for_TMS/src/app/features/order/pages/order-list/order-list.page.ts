import { Component, computed, inject, OnInit, signal } from '@angular/core';
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
import { DialogModule } from 'primeng/dialog';
import { TextareaModule } from 'primeng/textarea';
import { MessageService, ConfirmationService } from 'primeng/api';
import { OrderService } from '@features/order/services/order.service';
import { TripService } from '@features/order/services/trip.service';
import { CustomerService } from '@features/customer/services/customer.service';
import { TruckService } from '@features/truck/services/truck.service';
import { DriverService } from '@features/driver/services/driver.service';
import {
  OrderDto, TripDto, OrderStatusSummaryDto,
  CreateOrderDto, CompleteTripDto, TripFormPayload
} from '@features/order/models/order.models';
import { CustomerDto } from '@features/customer/models/customer.models';
import { TruckDto } from '@features/truck/models/truck.models';
import { DriverDto } from '@features/driver/models/driver.models';
import { OrderFormDialog } from '@features/order/components/order-form-dialog/order-form-dialog';
import { TripFormDialog } from '@features/order/components/trip-form-dialog/trip-form-dialog';
import { TripCompleteDialog } from '@features/order/components/trip-complete-dialog/trip-complete-dialog';

type TagSeverity = 'success' | 'secondary' | 'info' | 'warn' | 'danger' | 'contrast';

const ORDER_STATUS_MAP: Record<number, { label: string; severity: TagSeverity }> = {
  1: { label: 'Created', severity: 'info' },
  2: { label: 'Assigned', severity: 'warn' },
  3: { label: 'InTransit', severity: 'contrast' },
  4: { label: 'Delivered', severity: 'success' },
  5: { label: 'Completed', severity: 'secondary' },
  6: { label: 'Cancelled', severity: 'danger' },
};

const TRIP_STATUS_MAP: Record<number, { label: string; severity: TagSeverity }> = {
  1: { label: 'Planned', severity: 'info' },
  2: { label: 'InTransit', severity: 'contrast' },
  3: { label: 'Completed', severity: 'success' },
  4: { label: 'Cancelled', severity: 'danger' },
};

const ORDER_STATUS_FILTER_OPTIONS = [
  { label: 'All', value: null },
  { label: 'Created', value: 1 },
  { label: 'Assigned', value: 2 },
  { label: 'InTransit', value: 3 },
  { label: 'Delivered', value: 4 },
  { label: 'Completed', value: 5 },
  { label: 'Cancelled', value: 6 },
];

@Component({
  selector: 'app-order-list',
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
    DialogModule,
    TextareaModule,
    OrderFormDialog,
    TripFormDialog,
    TripCompleteDialog,
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './order-list.page.html',
})
export class OrderListPage implements OnInit {
  private readonly orderService = inject(OrderService);
  private readonly tripService = inject(TripService);
  private readonly customerService = inject(CustomerService);
  private readonly truckService = inject(TruckService);
  private readonly driverService = inject(DriverService);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  // --- Table state ---
  orders = signal<OrderDto[]>([]);
  totalRecords = signal(0);
  loading = signal(true);
  rows = signal(10);
  first = signal(0);
  readonly rowsPerPageOptions = [5, 10, 20];

  // --- Status summary ---
  statusSummary = signal<OrderStatusSummaryDto | null>(null);

  // --- Search & filter ---
  searchTerm = signal('');
  statusFilter = signal<number | null>(null);
  customerFilter = signal<string | null>(null);
  readonly statusFilterOptions = ORDER_STATUS_FILTER_OPTIONS;
  customerFilterOptions = computed(() => [
    { label: 'All Customers', value: null as string | null },
    ...this.customers().map(c => ({ label: c.name, value: c.customerId })),
  ]);

  // --- Dropdown data ---
  customers = signal<CustomerDto[]>([]);
  trucks = signal<TruckDto[]>([]);
  drivers = signal<DriverDto[]>([]);

  // --- Expanded rows + trips ---
  expandedRows: Record<string, boolean> = {};
  orderTrips = signal<Record<string, TripDto[] | undefined>>({});
  orderTripsLoading = signal<Record<string, boolean | undefined>>({});

  // --- Order form dialog ---
  orderFormVisible = signal(false);
  editingOrder = signal<OrderDto | null>(null);
  orderSubmitting = signal(false);

  // --- Trip form dialog ---
  tripFormVisible = signal(false);
  editingTrip = signal<TripDto | null>(null);
  tripFormOrderId = signal('');
  tripFormOrderNumber = signal('');
  tripSubmitting = signal(false);

  // --- Trip complete dialog ---
  tripCompleteVisible = signal(false);
  completingTrip = signal<TripDto | null>(null);
  tripCompleteSubmitting = signal(false);

  // --- Cancel dialog ---
  cancelDialogVisible = signal(false);
  cancelType = signal<'order' | 'trip'>('order');
  cancelTargetLabel = signal('');
  cancelTargetId = signal('');
  cancelTargetOrderId = signal('');
  cancellationReason = signal('');
  cancelSubmitting = signal(false);

  ngOnInit(): void {
    this.loadDropdownData();
    this.loadStatusSummary();
  }

  // ========== DATA LOADING ==========

  loadOrders(event: TableLazyLoadEvent): void {
    this.loading.set(true);
    const size = event.rows ?? this.rows();
    const page = Math.floor((event.first ?? 0) / size) + 1;

    this.orderService.list(
      page,
      size,
      this.statusFilter() ?? undefined,
      this.customerFilter() ?? undefined,
      this.searchTerm() || undefined,
    ).subscribe({
      next: (res) => {
        this.orders.set(res.data);
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
    this.loadOrders({ first: this.first(), rows: this.rows() });
  }

  private refreshAll(): void {
    this.refreshTable();
    this.loadStatusSummary();
  }

  loadStatusSummary(): void {
    this.orderService.statusSummary().subscribe({
      next: (summary) => this.statusSummary.set(summary),
    });
  }

  private loadDropdownData(): void {
    this.customerService.list(1, 1000).subscribe(res => this.customers.set(res.data));
    this.truckService.list(1, 1000).subscribe(res => this.trucks.set(res.data));
    this.driverService.list(1, 1000).subscribe(res => this.drivers.set(res.data));
  }

  loadTripsForOrder(orderId: string): void {
    this.orderTripsLoading.update(v => ({ ...v, [orderId]: true }));
    this.tripService.list(1, 100, orderId).subscribe({
      next: (res) => {
        this.orderTrips.update(v => ({ ...v, [orderId]: res.data }));
        this.orderTripsLoading.update(v => ({ ...v, [orderId]: false }));
      },
      error: () => {
        this.orderTripsLoading.update(v => ({ ...v, [orderId]: false }));
      },
    });
  }

  // ========== SEARCH & FILTER ==========

  onSearch(): void {
    this.loadOrders({ first: 0, rows: this.rows() });
  }

  onStatusFilterChange(value: number | null): void {
    this.statusFilter.set(value);
    this.loadOrders({ first: 0, rows: this.rows() });
  }

  onCustomerFilterChange(value: string | null): void {
    this.customerFilter.set(value);
    this.loadOrders({ first: 0, rows: this.rows() });
  }

  // ========== ROW EXPAND ==========

  onRowExpand(event: { data: OrderDto }): void {
    this.loadTripsForOrder(event.data.orderId);
  }

  // ========== STATUS HELPERS ==========

  getOrderStatusLabel(status: number): string {
    return ORDER_STATUS_MAP[status]?.label ?? 'Unknown';
  }

  getOrderStatusSeverity(status: number): TagSeverity {
    return ORDER_STATUS_MAP[status]?.severity ?? 'secondary';
  }

  getTripStatusLabel(status: number): string {
    return TRIP_STATUS_MAP[status]?.label ?? 'Unknown';
  }

  getTripStatusSeverity(status: number): TagSeverity {
    return TRIP_STATUS_MAP[status]?.severity ?? 'secondary';
  }

  isOrderTerminal(order: OrderDto): boolean {
    return order.status === 5 || order.status === 6;
  }

  hasActiveTrip(order: OrderDto): boolean {
    const trips = this.orderTrips()[order.orderId];
    return trips?.some(t => t.status === 1 || t.status === 2) ?? false;
  }

  // ========== ORDER CRUD ==========

  onAddOrder(): void {
    this.editingOrder.set(null);
    this.orderFormVisible.set(true);
  }

  onEditOrder(order: OrderDto): void {
    this.editingOrder.set(order);
    this.orderFormVisible.set(true);
  }

  onOrderFormSaved(payload: CreateOrderDto): void {
    if (this.orderSubmitting()) return;
    this.orderSubmitting.set(true);

    const editOrder = this.editingOrder();
    const isEdit = editOrder !== null;
    const op$ = isEdit
      ? this.orderService.update(editOrder.orderId, {
        pickupAddress: payload.pickupAddress,
        deliveryAddress: payload.deliveryAddress,
        cargoDescription: payload.cargoDescription,
        cargoWeightKg: payload.cargoWeightKg,
        requestedPickupDate: payload.requestedPickupDate,
        requestedDeliveryDate: payload.requestedDeliveryDate,
        quotedPrice: payload.quotedPrice,
        notes: payload.notes,
      })
      : this.orderService.create(payload);

    op$.subscribe({
      next: () => {
        this.orderFormVisible.set(false);
        this.editingOrder.set(null);
        this.orderSubmitting.set(false);
        this.refreshAll();
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: isEdit ? 'Order updated successfully.' : 'Order created successfully.',
          life: 3000,
        });
      },
      error: () => {
        this.orderSubmitting.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Operation failed. Please try again.',
          life: 5000,
        });
      },
    });
  }

  onOrderFormClosed(event: { hadUnsavedChanges: boolean }): void {
    if (event.hadUnsavedChanges) {
      this.messageService.add({
        severity: 'info',
        summary: 'Not Saved',
        detail: 'Changes were discarded.',
        life: 3000,
      });
    }
    this.editingOrder.set(null);
  }

  // ========== ORDER LIFECYCLE ==========

  onCompleteOrder(order: OrderDto): void {
    this.confirmationService.confirm({
      message: `Are you sure you want to complete order <b>${order.orderNumber}</b>? This will finalize the order for invoicing.`,
      header: 'Complete Order',
      icon: 'pi pi-check-circle',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', text: true },
      acceptButtonProps: { label: 'Complete', severity: 'success' },
      accept: () => {
        this.loading.set(true);
        this.orderService.complete(order.orderId).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Completed',
              detail: `Order ${order.orderNumber} completed.`,
              life: 3000,
            });
            this.refreshAll();
          },
          error: () => {
            this.loading.set(false);
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Complete failed. Please try again.',
              life: 5000,
            });
          },
        });
      },
    });
  }

  onCancelOrder(order: OrderDto): void {
    this.cancelType.set('order');
    this.cancelTargetLabel.set(order.orderNumber);
    this.cancelTargetId.set(order.orderId);
    this.cancelTargetOrderId.set(order.orderId);
    this.cancellationReason.set('');
    this.cancelDialogVisible.set(true);
  }

  // ========== TRIP CRUD ==========

  onAddTrip(order: OrderDto): void {
    this.editingTrip.set(null);
    this.tripFormOrderId.set(order.orderId);
    this.tripFormOrderNumber.set(order.orderNumber);
    this.tripFormVisible.set(true);
  }

  onEditTrip(trip: TripDto): void {
    this.editingTrip.set(trip);
    this.tripFormOrderId.set(trip.orderId);
    this.tripFormOrderNumber.set(trip.orderNumber);
    this.tripFormVisible.set(true);
  }

  onTripFormSaved(payload: TripFormPayload): void {
    if (this.tripSubmitting()) return;
    this.tripSubmitting.set(true);

    const editTrip = this.editingTrip();
    const isEdit = editTrip !== null;
    const orderId = this.tripFormOrderId();

    const op$ = isEdit
      ? this.tripService.update(editTrip.tripId, {
        truckId: payload.truckId,
        driverId: payload.driverId,
        plannedPickupDate: payload.plannedPickupDate,
        plannedDeliveryDate: payload.plannedDeliveryDate,
        notes: payload.notes,
      })
      : this.tripService.create({
        orderId,
        truckId: payload.truckId,
        driverId: payload.driverId,
        plannedPickupDate: payload.plannedPickupDate,
        plannedDeliveryDate: payload.plannedDeliveryDate,
        notes: payload.notes,
      });

    op$.subscribe({
      next: () => {
        this.tripFormVisible.set(false);
        this.editingTrip.set(null);
        this.tripSubmitting.set(false);
        this.refreshAll();
        this.loadTripsForOrder(orderId);
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: isEdit ? 'Trip updated successfully.' : 'Trip created successfully.',
          life: 3000,
        });
      },
      error: () => {
        this.tripSubmitting.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Operation failed. Please try again.',
          life: 5000,
        });
      },
    });
  }

  onTripFormClosed(event: { hadUnsavedChanges: boolean }): void {
    if (event.hadUnsavedChanges) {
      this.messageService.add({
        severity: 'info',
        summary: 'Not Saved',
        detail: 'Changes were discarded.',
        life: 3000,
      });
    }
    this.editingTrip.set(null);
  }

  // ========== TRIP LIFECYCLE ==========

  onStartTrip(trip: TripDto): void {
    this.confirmationService.confirm({
      message: `Start trip <b>${trip.tripNumber}</b>? This will mark the cargo as picked up and in transit.`,
      header: 'Start Trip',
      icon: 'pi pi-play',
      rejectButtonProps: { label: 'Cancel', severity: 'secondary', text: true },
      acceptButtonProps: { label: 'Start Trip', severity: 'info' },
      accept: () => {
        this.tripService.start(trip.tripId).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Started',
              detail: `Trip ${trip.tripNumber} started.`,
              life: 3000,
            });
            this.refreshAll();
            this.loadTripsForOrder(trip.orderId);
          },
          error: () => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Start trip failed. Please try again.',
              life: 5000,
            });
          },
        });
      },
    });
  }

  onCompleteTrip(trip: TripDto): void {
    this.completingTrip.set(trip);
    this.tripCompleteVisible.set(true);
  }

  onTripCompleteSaved(dto: CompleteTripDto): void {
    if (this.tripCompleteSubmitting()) return;
    this.tripCompleteSubmitting.set(true);

    const orderId = this.completingTrip()?.orderId ?? '';

    this.tripService.complete(dto).subscribe({
      next: () => {
        this.tripCompleteVisible.set(false);
        this.completingTrip.set(null);
        this.tripCompleteSubmitting.set(false);
        this.refreshAll();
        if (orderId) this.loadTripsForOrder(orderId);
        this.messageService.add({
          severity: 'success',
          summary: 'Completed',
          detail: 'Trip completed successfully.',
          life: 3000,
        });
      },
      error: () => {
        this.tripCompleteSubmitting.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Complete trip failed. Please try again.',
          life: 5000,
        });
      },
    });
  }

  onTripCompleteClosed(event: { hadUnsavedChanges: boolean }): void {
    if (event.hadUnsavedChanges) {
      this.messageService.add({
        severity: 'info',
        summary: 'Not Saved',
        detail: 'Changes were discarded.',
        life: 3000,
      });
    }
    this.completingTrip.set(null);
  }

  onCancelTrip(trip: TripDto): void {
    this.cancelType.set('trip');
    this.cancelTargetLabel.set(trip.tripNumber);
    this.cancelTargetId.set(trip.tripId);
    this.cancelTargetOrderId.set(trip.orderId);
    this.cancellationReason.set('');
    this.cancelDialogVisible.set(true);
  }

  // ========== CANCEL DIALOG ==========

  onConfirmCancel(): void {
    const reason = this.cancellationReason().trim();
    if (!reason || this.cancelSubmitting()) return;

    this.cancelSubmitting.set(true);
    const type = this.cancelType();
    const id = this.cancelTargetId();
    const orderId = this.cancelTargetOrderId();

    if (type === 'order') {
      this.orderService.cancel({ orderId: id, cancellationReason: reason }).subscribe({
        next: () => {
          this.cancelDialogVisible.set(false);
          this.cancelSubmitting.set(false);
          this.refreshAll();
          this.loadTripsForOrder(id);
          this.messageService.add({
            severity: 'success',
            summary: 'Cancelled',
            detail: 'Order cancelled.',
            life: 3000,
          });
        },
        error: () => {
          this.cancelSubmitting.set(false);
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Cancel failed. Please try again.',
            life: 5000,
          });
        },
      });
    } else {
      this.tripService.cancel({ tripId: id, cancellationReason: reason }).subscribe({
        next: () => {
          this.cancelDialogVisible.set(false);
          this.cancelSubmitting.set(false);
          this.refreshAll();
          if (orderId) this.loadTripsForOrder(orderId);
          this.messageService.add({
            severity: 'success',
            summary: 'Cancelled',
            detail: 'Trip cancelled.',
            life: 3000,
          });
        },
        error: () => {
          this.cancelSubmitting.set(false);
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Cancel failed. Please try again.',
            life: 5000,
          });
        },
      });
    }
  }
}
