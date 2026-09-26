import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { EnvService } from '@platform/env/env.service';
import {
  ApiResult, TripDto, PaginatedTripsDto,
  CreateTripDto, UpdateTripDto, CompleteTripDto, CancelTripDto
} from '../models/order.models';

@Injectable({
  providedIn: 'root'
})
export class TripService {
  private readonly http = inject(HttpClient);
  private readonly env = inject(EnvService);
  private readonly baseUrl = `${this.env.apiUrl}/api/Trip`;

  create(dto: CreateTripDto): Observable<TripDto> {
    return this.http.post<ApiResult<TripDto>>(`${this.baseUrl}/Create`, dto)
      .pipe(map(res => res.data));
  }

  getById(tripId: string): Observable<TripDto> {
    return this.http.get<ApiResult<TripDto>>(`${this.baseUrl}/GetById/${tripId}`)
      .pipe(map(res => res.data));
  }

  list(
    pageNumber = 1,
    pageSize = 10,
    orderId?: string,
    status?: number
  ): Observable<PaginatedTripsDto> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (orderId) params = params.set('orderId', orderId);
    if (status !== undefined && status !== null) params = params.set('status', status);

    return this.http.get<ApiResult<PaginatedTripsDto>>(`${this.baseUrl}/List`, { params })
      .pipe(map(res => res.data));
  }

  update(tripId: string, dto: UpdateTripDto): Observable<TripDto> {
    return this.http.put<ApiResult<TripDto>>(`${this.baseUrl}/Update/${tripId}`, dto)
      .pipe(map(res => res.data));
  }

  start(tripId: string, dto?: { actualPickupDate?: string }): Observable<TripDto> {
    return this.http.patch<ApiResult<TripDto>>(`${this.baseUrl}/Start/${tripId}`, dto ?? {})
      .pipe(map(res => res.data));
  }

  complete(dto: CompleteTripDto): Observable<TripDto> {
    return this.http.patch<ApiResult<TripDto>>(`${this.baseUrl}/Complete`, dto)
      .pipe(map(res => res.data));
  }

  cancel(dto: CancelTripDto): Observable<TripDto> {
    return this.http.patch<ApiResult<TripDto>>(`${this.baseUrl}/Cancel`, dto)
      .pipe(map(res => res.data));
  }
}
