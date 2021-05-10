import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewContentManagementComponent } from './view-content-management.component';

describe('ViewContentManagementComponent', () => {
  let component: ViewContentManagementComponent;
  let fixture: ComponentFixture<ViewContentManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewContentManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewContentManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
