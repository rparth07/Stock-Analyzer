import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, catchError, throwError, timeout, finalize } from 'rxjs';
import { ApiStatusService } from './api-status.service';
import { DomainConstants } from './domain.constants';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  constructor(private status: ApiStatusService) {}
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (![DomainConstants.StockAnalyzer_URL, DomainConstants.Filter_URL, DomainConstants.Notebook_URL, DomainConstants.StockAnalyzer_URL.replace('StockAnalyzer/', 'market-data/')]
      .some(url => request.url.startsWith(url))) return next.handle(request);
    // Never retry writes automatically: a timed-out request may already have saved.
    this.status.pending++;
    return next.handle(request).pipe(timeout(request.method === 'GET' ? 90000 : 30000), catchError(error => {
      this.status.report(error, request.method);
      return throwError(() => error);
    }), finalize(() => this.status.pending--));
  }
}
