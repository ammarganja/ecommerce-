import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingupPopupComponent } from './singup-popup.component';

describe('SingupPopupComponent', () => {
  let component: SingupPopupComponent;
  let fixture: ComponentFixture<SingupPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SingupPopupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SingupPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
