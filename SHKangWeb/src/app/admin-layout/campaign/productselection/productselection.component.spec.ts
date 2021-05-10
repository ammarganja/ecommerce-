import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductselectionComponent } from './productselection.component';

describe('ProductselectionComponent', () => {
  let component: ProductselectionComponent;
  let fixture: ComponentFixture<ProductselectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductselectionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductselectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
