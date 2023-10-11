import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BhavInfoComponent } from './bhav-info.component';

describe('BhavInfoComponent', () => {
  let component: BhavInfoComponent;
  let fixture: ComponentFixture<BhavInfoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BhavInfoComponent]
    });
    fixture = TestBed.createComponent(BhavInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
