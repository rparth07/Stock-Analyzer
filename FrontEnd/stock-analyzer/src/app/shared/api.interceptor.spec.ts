import { TestBed, fakeAsync, tick } from '@angular/core/testing';
import { HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiInterceptor } from './api.interceptor';
import { ApiStatusService } from './api-status.service';
import { DomainConstants } from './domain.constants';

describe('API recovery', () => {
  let http: HttpClient;
  let requests: HttpTestingController;
  let status: ApiStatusService;
  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [HttpClientTestingModule], providers: [
      { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true }
    ] });
    http = TestBed.inject(HttpClient); requests = TestBed.inject(HttpTestingController);
    status = TestBed.inject(ApiStatusService);
  });
  afterEach(() => requests.verify());
  it('passes actual successful data through unchanged', () => {
    const data = [{ symbol: 'TEST', closePrice: 123.45 }];
    http.get(DomainConstants.StockAnalyzer_URL + 'test').subscribe(value => expect(value).toEqual(data));
    requests.expectOne(DomainConstants.StockAnalyzer_URL + 'test').flush(data);
    expect(status.message).toBe('');
  });
  it('ends a hung request after 90 seconds', fakeAsync(() => {
    let failed = false;
    http.get(DomainConstants.StockAnalyzer_URL + 'test').subscribe({ error: () => failed = true });
    const pending = requests.expectOne(DomainConstants.StockAnalyzer_URL + 'test');
    tick(90000);
    expect(failed).toBeTrue(); expect(pending.cancelled).toBeTrue();
    expect(status.message).toContain('too long');
  }));
  it('preserves write failures without an automatic retry', () => {
    let failed = false;
    http.post(DomainConstants.Notebook_URL + 'save', {}).subscribe({ error: () => failed = true });
    requests.expectOne(DomainConstants.Notebook_URL + 'save').flush({}, { status: 503, statusText: 'Unavailable' });
    expect(failed).toBeTrue(); expect(status.message).toContain('Keep your draft');
    requests.expectNone(DomainConstants.Notebook_URL + 'save');
  });
  it('explains provider rate limits', () => {
    http.get(DomainConstants.StockAnalyzer_URL + 'test').subscribe({ error: () => {} });
    requests.expectOne(DomainConstants.StockAnalyzer_URL + 'test').flush({}, { status: 429, statusText: 'Limited' });
    expect(status.message).toContain('request limit');
  });
});
