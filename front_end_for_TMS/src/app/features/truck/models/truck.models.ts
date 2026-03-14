export interface ApiResult<T> {
  instance?: string;
  success: boolean;
  status: number;
  data: T;
}

export interface TruckDto {
  truckId: string;
  licensePlate: string;
  vinNumber?: string;
  engineNumber?: string;
  brand?: string;
  modelYear?: number;
  purchaseDate?: string;
  truckType?: string;
  maxPayloadKg?: number;
  lengthMm?: number;
  widthMm?: number;
  heightMm?: number;
  ownershipType: number;
  currentStatus: number;
  odometerReading: number;
  lastMaintenanceDate?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface PaginatedTrucksDto {
  data: TruckDto[];
  totalCount: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
}