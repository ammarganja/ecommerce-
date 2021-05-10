import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderBasicDetailComponent } from './order-basic-detail.component';

describe('OrderBasicDetailComponent', () => {
  let component: OrderBasicDetailComponent;
  let fixture: ComponentFixture<OrderBasicDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderBasicDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderBasicDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
