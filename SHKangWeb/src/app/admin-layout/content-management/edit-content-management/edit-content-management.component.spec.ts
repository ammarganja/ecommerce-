import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditContentManagementComponent } from './edit-content-management.component';

describe('EditContentManagementComponent', () => {
  let component: EditContentManagementComponent;
  let fixture: ComponentFixture<EditContentManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditContentManagementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditContentManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
