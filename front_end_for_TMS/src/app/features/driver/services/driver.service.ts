import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { EnvService } from '../../../platform/env/env.service';
import { ApiResult, DriverDto, PaginatedDriversDto } from '../models/driver.models';

@Injectable({
  providedIn: 'root'
})
export class DriverService {
  private readonly http = inject(HttpClient);
  private readonly env = inject(EnvService);
  private readonly baseUrl = `${this.env.apiUrl}/api/Driver`;

  create(dto: Partial<DriverDto>): Observable<DriverDto> {
    return this.http.post<ApiResult<DriverDto>>(`${this.baseUrl}/Create`, dto)
      .pipe(map(res => res.data));
  }

  getById(driverId: string): Observable<DriverDto> {
    return this.http.get<ApiResult<DriverDto>>(`${this.baseUrl}/GetById/${driverId}`)
      .pipe(map(res => res.data));
  }

  list(
    pageNumber = 1,
    pageSize = 10,
    status?: number,
    searchTerm?: string
  ): Observable<PaginatedDriversDto> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (status !== undefined && status !== null) params = params.set('status', status);
    if (searchTerm) params = params.set('searchTerm', searchTerm);

    return this.http.get<ApiResult<PaginatedDriversDto>>(`${this.baseUrl}/List`, { params })
      .pipe(map(res => res.data));
  }

  update(driverId: string, dto: Partial<DriverDto>): Observable<DriverDto> {
    return this.http.put<ApiResult<DriverDto>>(`${this.baseUrl}/Update/${driverId}`, dto)
      .pipe(map(res => res.data));
  }

  delete(driverId: string): Observable<boolean> {
    return this.http.delete<ApiResult<boolean>>(`${this.baseUrl}/Delete/${driverId}`)
      .pipe(map(res => res.data));
  }
}
