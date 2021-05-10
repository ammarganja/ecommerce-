import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnAuhtorized401Component } from './un-auhtorized401.component';

describe('UnAuhtorized401Component', () => {
  let component: UnAuhtorized401Component;
  let fixture: ComponentFixture<UnAuhtorized401Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UnAuhtorized401Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UnAuhtorized401Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
