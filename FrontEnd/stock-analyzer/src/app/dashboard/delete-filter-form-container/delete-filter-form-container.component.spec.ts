import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteFilterFormContainerComponent } from './delete-filter-form-container.component';

describe('DeleteFilterFormContainerComponent', () => {
  let component: DeleteFilterFormContainerComponent;
  let fixture: ComponentFixture<DeleteFilterFormContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeleteFilterFormContainerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DeleteFilterFormContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
