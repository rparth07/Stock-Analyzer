import { HttpClient } from '@angular/common/http';
import * as moment from 'moment';
import { of } from 'rxjs';
import { BhavInfoService } from './bhav-info.service';
import { BulkDealService } from './bulk-deal.service';

describe('Trading date requests', () => {
  it('keeps the selected calendar date across UTC boundaries', () => {
    const http = jasmine.createSpyObj<HttpClient>('http', ['get']);
    http.get.and.returnValue(of([]));
    const start = moment.parseZone('2026-10-26T00:00:00-08:00') as any;
    const end = moment.parseZone('2026-10-27T00:00:00+05:30') as any;
    new BhavInfoService(http).fetchBhavInfosBetween(start, end);
    expect(http.get.calls.mostRecent().args[1]).toEqual({ params: { startDate: '2026-10-26', endDate: '2026-10-27' } });
    new BulkDealService(http).fetchBulkDealsBetween(start, end);
    expect(http.get.calls.mostRecent().args[1]).toEqual({ params: { startDate: '2026-10-26', endDate: '2026-10-27' } });
  });
});
