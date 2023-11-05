import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterFormContainerComponent } from './filter-form-container.component';

describe('FilterFormContainerComponent', () => {
  let component: FilterFormContainerComponent;
  let fixture: ComponentFixture<FilterFormContainerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FilterFormContainerComponent]
    });
    fixture = TestBed.createComponent(FilterFormContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
