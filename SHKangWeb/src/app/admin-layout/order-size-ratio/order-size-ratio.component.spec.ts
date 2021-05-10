import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderSizeRatioComponent } from './order-size-ratio.component';

describe('OrderSizeRatioComponent', () => {
  let component: OrderSizeRatioComponent;
  let fixture: ComponentFixture<OrderSizeRatioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderSizeRatioComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderSizeRatioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
