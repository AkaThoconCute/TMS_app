import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { EnvService } from '@platform/env/env.service';
import {
  ApiResult, OrderDto, PaginatedOrdersDto, OrderStatusSummaryDto,
  CreateOrderDto, UpdateOrderDto, CancelOrderDto
} from '../models/order.models';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private readonly http = inject(HttpClient);
  private readonly env = inject(EnvService);
  private readonly baseUrl = `${this.env.apiUrl}/api/Order`;

  create(dto: CreateOrderDto): Observable<OrderDto> {
    return this.http.post<ApiResult<OrderDto>>(`${this.baseUrl}/Create`, dto)
      .pipe(map(res => res.data));
  }

  getById(orderId: string): Observable<OrderDto> {
    return this.http.get<ApiResult<OrderDto>>(`${this.baseUrl}/GetById/${orderId}`)
      .pipe(map(res => res.data));
  }

  list(
    pageNumber = 1,
    pageSize = 10,
    status?: number,
    customerId?: string,
    searchTerm?: string
  ): Observable<PaginatedOrdersDto> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (status !== undefined && status !== null) params = params.set('status', status);
    if (customerId) params = params.set('customerId', customerId);
    if (searchTerm) params = params.set('searchTerm', searchTerm);

    return this.http.get<ApiResult<PaginatedOrdersDto>>(`${this.baseUrl}/List`, { params })
      .pipe(map(res => res.data));
  }

  statusSummary(): Observable<OrderStatusSummaryDto> {
    return this.http.get<ApiResult<OrderStatusSummaryDto>>(`${this.baseUrl}/StatusSummary`)
      .pipe(map(res => res.data));
  }

  update(orderId: string, dto: UpdateOrderDto): Observable<OrderDto> {
    return this.http.put<ApiResult<OrderDto>>(`${this.baseUrl}/Update/${orderId}`, dto)
      .pipe(map(res => res.data));
  }

  complete(orderId: string, dto?: { notes?: string }): Observable<OrderDto> {
    return this.http.patch<ApiResult<OrderDto>>(`${this.baseUrl}/Complete/${orderId}`, dto ?? {})
      .pipe(map(res => res.data));
  }

  cancel(dto: CancelOrderDto): Observable<OrderDto> {
    return this.http.patch<ApiResult<OrderDto>>(`${this.baseUrl}/Cancel`, dto)
      .pipe(map(res => res.data));
  }
}
