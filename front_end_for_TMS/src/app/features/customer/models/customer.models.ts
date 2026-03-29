export interface ApiResult<T> {
  instance?: string;
  success: boolean;
  status: number;
  data: T;
}

export interface CustomerDto {
  customerId: string;
  name: string;
  contactPerson?: string;
  phoneNumber: string;
  email?: string;
  address?: string;
  taxCode?: string;
  customerType: number;
  customerTypeLabel: string;
  status: number;
  statusLabel: string;
  notes?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface PaginatedCustomersDto {
  data: CustomerDto[];
  totalCount: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
}
