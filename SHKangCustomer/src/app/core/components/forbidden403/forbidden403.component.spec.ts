import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Forbidden403Component } from './forbidden403.component';

describe('Forbidden403Component', () => {
  let component: Forbidden403Component;
  let fixture: ComponentFixture<Forbidden403Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Forbidden403Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Forbidden403Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
