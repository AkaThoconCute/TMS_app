import { Injectable } from '@angular/core';

/**
 * Cookie Service
 * Handles all cookie operations with secure defaults
 */
@Injectable({
  providedIn: 'root'
})
export class CookieService {
  private get docAvailable(): boolean {
    return typeof document !== 'undefined';
  }

  private get hasCookies(): boolean {
    return this.docAvailable && !!document.cookie;
  }

  /**
   * Get cookie by name
   * @param name Cookie name
   * @returns Cookie value or null if not found
   */
  getCookie(name: string): string | null {
    if (!this.docAvailable) {
      return null;
    }

    const nameEQ = name + '=';
    const cookies = document.cookie.split(';');

    for (let cookie of cookies) {
      cookie = cookie.trim();
      if (cookie.indexOf(nameEQ) === 0) {
        return decodeURIComponent(cookie.substring(nameEQ.length));
      }
    }

    return null;
  }

  /**
   * Set cookie with secure defaults
   * @param name Cookie name
   * @param value Cookie value
   * @param days Expiration days (default: 7)
   */
  setCookie(name: string, value: string, days: number = 7): void {
    if (!this.docAvailable) {
      return;
    }

    const date = new Date();
    date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
    const expires = 'expires=' + date.toUTCString();
    document.cookie = `${name}=${encodeURIComponent(value)};${expires};path=/;secure;samesite=strict`;
  }

  /**
   * Delete cookie
   * @param name Cookie name
   */
  deleteCookie(name: string): void {
    if (!this.docAvailable) {
      return;
    }
    document.cookie = `${name}=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;`;
  }

  /**
   * Check if cookie exists
   * @param name Cookie name
   * @returns true if cookie exists
   */
  hasCookie(name: string): boolean {
    return this.getCookie(name) !== null;
  }

  /**
   * Delete all cookies
   */
  clearAllCookies(): void {
    if (!this.docAvailable) {
      return;
    }

    const cookies = document.cookie.split(';');

    for (let cookie of cookies) {
      const cookieName = cookie.split('=')[0].trim();
      if (cookieName) {
        this.deleteCookie(cookieName);
      }
    }
  }
}
