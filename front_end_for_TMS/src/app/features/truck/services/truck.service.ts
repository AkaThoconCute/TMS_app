import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { EnvService } from '../../../platform/env/env.service';
import { ApiResult, TruckDto, PaginatedTrucksDto } from '../models/truck.models';

@Injectable({
  providedIn: 'root'
})
export class TruckService {
  private readonly http = inject(HttpClient);
  private readonly env = inject(EnvService);
  private readonly baseUrl = `${this.env.apiUrl}/api/Truck`;

  create(dto: Partial<TruckDto>): Observable<TruckDto> {
    return this.http.post<ApiResult<TruckDto>>(`${this.baseUrl}/Create`, dto)
      .pipe(map(res => res.data));
  }

  getById(truckId: string): Observable<TruckDto> {
    return this.http.get<ApiResult<TruckDto>>(`${this.baseUrl}/GetById/${truckId}`)
      .pipe(map(res => res.data));
  }

  getByLicensePlate(licensePlate: string): Observable<TruckDto> {
    return this.http.get<ApiResult<TruckDto>>(`${this.baseUrl}/GetByLicensePlate/${licensePlate}`)
      .pipe(map(res => res.data));
  }

  list(
    pageNumber = 1,
    pageSize = 10,
    status?: number,
    searchTerm?: string
  ): Observable<PaginatedTrucksDto> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (status !== undefined && status !== null) params = params.set('status', status);
    if (searchTerm) params = params.set('searchTerm', searchTerm);

    return this.http.get<ApiResult<PaginatedTrucksDto>>(`${this.baseUrl}/List`, { params })
      .pipe(map(res => res.data));
  }

  update(truckId: string, dto: Partial<TruckDto>): Observable<TruckDto> {
    return this.http.put<ApiResult<TruckDto>>(`${this.baseUrl}/Update/${truckId}`, dto)
      .pipe(map(res => res.data));
  }

  delete(truckId: string): Observable<boolean> {
    return this.http.delete<ApiResult<boolean>>(`${this.baseUrl}/Delete/${truckId}`)
      .pipe(map(res => res.data));
  }

  updateOdometer(truckId: string, odometerReading: number): Observable<boolean> {
    return this.http.patch<ApiResult<boolean>>(`${this.baseUrl}/UpdateOdometer/${truckId}`, { odometerReading })
      .pipe(map(res => res.data));
  }

  updateStatus(truckId: string, status: number): Observable<boolean> {
    return this.http.patch<ApiResult<boolean>>(`${this.baseUrl}/UpdateStatus/${truckId}`, { status })
      .pipe(map(res => res.data));
  }

  updateMaintenanceDate(truckId: string, maintenanceDate: string): Observable<boolean> {
    return this.http.patch<ApiResult<boolean>>(`${this.baseUrl}/UpdateMaintenanceDate/${truckId}`, { maintenanceDate })
      .pipe(map(res => res.data));
  }
}