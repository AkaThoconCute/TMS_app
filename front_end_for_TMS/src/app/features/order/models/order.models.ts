export interface ApiResult<T> {
  instance?: string;
  success: boolean;
  status: number;
  data: T;
}

// --- Order DTOs ---

export interface OrderDto {
  orderId: string;
  orderNumber: string;
  customerId: string;
  customerName: string;
  pickupAddress: string;
  deliveryAddress: string;
  cargoDescription: string;
  cargoWeightKg?: number;
  requestedPickupDate?: string;
  requestedDeliveryDate?: string;
  quotedPrice?: number;
  status: number;
  statusLabel: string;
  cancellationReason?: string;
  notes?: string;
  tripCount: number;
  completedAt?: string;
  cancelledAt?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface PaginatedOrdersDto {
  data: OrderDto[];
  totalCount: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
}

export interface OrderStatusSummaryDto {
  created: number;
  assigned: number;
  inTransit: number;
  delivered: number;
  completed: number;
  cancelled: number;
}

export interface CreateOrderDto {
  customerId: string;
  pickupAddress: string;
  deliveryAddress: string;
  cargoDescription: string;
  cargoWeightKg?: number;
  requestedPickupDate?: string;
  requestedDeliveryDate?: string;
  quotedPrice?: number;
  notes?: string;
}

export interface UpdateOrderDto {
  pickupAddress?: string;
  deliveryAddress?: string;
  cargoDescription?: string;
  cargoWeightKg?: number;
  requestedPickupDate?: string;
  requestedDeliveryDate?: string;
  quotedPrice?: number;
  notes?: string;
}

export interface CancelOrderDto {
  orderId: string;
  cancellationReason: string;
}

// --- Trip DTOs ---

export interface TripDto {
  tripId: string;
  tripNumber: string;
  orderId: string;
  orderNumber: string;
  truckId: string;
  truckLicensePlate: string;
  driverId: string;
  driverFullName: string;
  plannedPickupDate?: string;
  plannedDeliveryDate?: string;
  actualPickupDate?: string;
  actualDeliveryDate?: string;
  status: number;
  statusLabel: string;
  fuelCost?: number;
  tollCost?: number;
  otherCost?: number;
  totalCost?: number;
  costNotes?: string;
  cancellationReason?: string;
  notes?: string;
  completedAt?: string;
  cancelledAt?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface PaginatedTripsDto {
  data: TripDto[];
  totalCount: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
}

export interface CreateTripDto {
  orderId: string;
  truckId: string;
  driverId: string;
  plannedPickupDate?: string;
  plannedDeliveryDate?: string;
  notes?: string;
}

export interface UpdateTripDto {
  truckId?: string;
  driverId?: string;
  plannedPickupDate?: string;
  plannedDeliveryDate?: string;
  notes?: string;
}

export interface CompleteTripDto {
  tripId: string;
  actualDeliveryDate: string;
  fuelCost?: number;
  tollCost?: number;
  otherCost?: number;
  costNotes?: string;
  notes?: string;
}

export interface CancelTripDto {
  tripId: string;
  cancellationReason: string;
}

// Shared type for trip form dialog output
export interface TripFormPayload {
  truckId: string;
  driverId: string;
  plannedPickupDate?: string;
  plannedDeliveryDate?: string;
  notes?: string;
}
