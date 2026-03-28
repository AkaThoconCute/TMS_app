export interface ApiResult<T> {
  instance?: string;
  success: boolean;
  status: number;
  data: T;
}

export interface DriverDto {
  driverId: string;
  fullName: string;
  phoneNumber: string;
  licenseNumber: string;
  licenseClass?: string;
  licenseExpiry?: string;
  dateOfBirth?: string;
  status: number;
  hireDate?: string;
  notes?: string;
  createdAt: string;
  updatedAt?: string;
  isLicenseExpiringSoon: boolean;
}

export interface PaginatedDriversDto {
  data: DriverDto[];
  totalCount: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
}
