import { TestBed } from '@angular/core/testing';

import { BulkDealService } from './bulk-deal.service';

describe('BulkDealService', () => {
  let service: BulkDealService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BulkDealService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
