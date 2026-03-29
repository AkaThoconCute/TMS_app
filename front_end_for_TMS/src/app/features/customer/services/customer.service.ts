import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { EnvService } from '@platform/env/env.service';
import { ApiResult, CustomerDto, PaginatedCustomersDto } from '@features/customer/models/customer.models';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private readonly http = inject(HttpClient);
  private readonly env = inject(EnvService);
  private readonly baseUrl = `${this.env.apiUrl}/api/Customer`;

  create(dto: Partial<CustomerDto>): Observable<CustomerDto> {
    return this.http.post<ApiResult<CustomerDto>>(`${this.baseUrl}/Create`, dto)
      .pipe(map(res => res.data));
  }

  getById(customerId: string): Observable<CustomerDto> {
    return this.http.get<ApiResult<CustomerDto>>(`${this.baseUrl}/GetById/${customerId}`)
      .pipe(map(res => res.data));
  }

  list(
    pageNumber = 1,
    pageSize = 10,
    status?: number,
    customerType?: number,
    searchTerm?: string
  ): Observable<PaginatedCustomersDto> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (status !== undefined && status !== null) params = params.set('status', status);
    if (customerType !== undefined && customerType !== null) params = params.set('customerType', customerType);
    if (searchTerm) params = params.set('searchTerm', searchTerm);

    return this.http.get<ApiResult<PaginatedCustomersDto>>(`${this.baseUrl}/List`, { params })
      .pipe(map(res => res.data));
  }

  update(customerId: string, dto: Partial<CustomerDto>): Observable<CustomerDto> {
    return this.http.put<ApiResult<CustomerDto>>(`${this.baseUrl}/Update/${customerId}`, dto)
      .pipe(map(res => res.data));
  }

  delete(customerId: string): Observable<boolean> {
    return this.http.delete<ApiResult<boolean>>(`${this.baseUrl}/Delete/${customerId}`)
      .pipe(map(res => res.data));
  }
}
