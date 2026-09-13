import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiResponse } from "@platform/auth/auth.models";
import { EnvService } from "@platform/env/env.service";
import { catchError, map, Observable, throwError } from "rxjs";

/**
 * Cancellation Service
 * Handles both HTTP API calls and cancellation state management
 */
@Injectable({
  providedIn: 'root'
})
export class CancellationService {
  private readonly apiService: string;

  constructor(
    private http: HttpClient,
    private configService: EnvService
  ) {
    this.apiService = this.configService.getApiUrl('/api/work-requests');
  }

  /**
   * Make a cancellation request
   */
  makeRequest(): Observable<void> {
    return this.http
      .post<ApiResponse<void>>(`${this.apiService}/start`, {})
      .pipe(
        map(response => response.data),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * Handle HTTP errors
   */
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      errorMessage = error.error?.data?.errors?.[0] || error.message || 'Unknown error';
    }

    console.error('API Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}