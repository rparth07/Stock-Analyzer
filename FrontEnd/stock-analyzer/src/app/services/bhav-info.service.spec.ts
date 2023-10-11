import { TestBed } from '@angular/core/testing';

import { BhavInfoService } from './bhav-info.service';

describe('BhavInfoService', () => {
  let service: BhavInfoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BhavInfoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
