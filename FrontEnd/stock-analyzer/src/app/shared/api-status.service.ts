import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { TimeoutError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ApiStatusService {
  message = '';
  pending = 0;
  report(error: unknown, method: string): void {
    const write = method !== 'GET' && method !== 'HEAD';
    const reason = error instanceof TimeoutError ? 'The data service took too long to respond.'
      : error instanceof HttpErrorResponse && error.status === 404 ? 'The requested resource was not found.'
      : error instanceof HttpErrorResponse && error.status === 429 ? 'The data service request limit was reached. Wait before trying again.'
      : error instanceof HttpErrorResponse && (error.status === 401 || error.status === 403) ? 'The data service denied access.'
      : 'The data service is unavailable. Check your connection and try again later.';
    const validation = error instanceof HttpErrorResponse && [400, 404, 422].includes(error.status) && typeof error.error?.message === 'string'
      ? error.error.message : reason;
    this.message = validation + (write ? ' Your change could not be confirmed. Keep your draft and check before submitting again.' : ' Results may be unavailable or from an earlier request.');
  }
}
