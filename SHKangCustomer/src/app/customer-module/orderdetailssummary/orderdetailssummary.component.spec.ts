import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderdetailssummaryComponent } from './orderdetailssummary.component';

describe('OrderdetailssummaryComponent', () => {
  let component: OrderdetailssummaryComponent;
  let fixture: ComponentFixture<OrderdetailssummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderdetailssummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderdetailssummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
