import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailmessageComponent } from './emailmessage.component';

describe('EmailmessageComponent', () => {
  let component: EmailmessageComponent;
  let fixture: ComponentFixture<EmailmessageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmailmessageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailmessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
