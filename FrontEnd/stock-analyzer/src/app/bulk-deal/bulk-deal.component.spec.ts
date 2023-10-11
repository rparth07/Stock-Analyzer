import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BulkDealComponent } from './bulk-deal.component';

describe('BulkDealComponent', () => {
  let component: BulkDealComponent;
  let fixture: ComponentFixture<BulkDealComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BulkDealComponent]
    });
    fixture = TestBed.createComponent(BulkDealComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
