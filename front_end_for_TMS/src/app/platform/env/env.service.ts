import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

/**
 * Application configuration service
 * Provides centralized access to environment configuration
 */
@Injectable({
  providedIn: 'root'
})
export class EnvService {
  readonly apiUrl = environment.apiUrl;
  readonly production = environment.production;

  constructor() { }

  /**
   * Get API endpoint URL
   * @param endpoint API endpoint path (e.g., '/api/Account')
   * @returns Full API URL
   */
  getApiUrl(endpoint: string): string {
    return `${this.apiUrl}${endpoint}`;
  }
}
