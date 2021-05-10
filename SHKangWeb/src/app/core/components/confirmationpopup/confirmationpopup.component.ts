import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-confirmationpopup',
  templateUrl: './confirmationpopup.component.html',
  styleUrls: ['./confirmationpopup.component.scss']
})
export class ConfirmationpopupComponent implements OnInit {

  @Input() id: number;
  @Input() title: string;
  isLoading = false;
  subscriptions: Subscription[] = [];
  headertitle: string;

  constructor(public modal: NgbActiveModal) { }

  ngOnInit(): void {
    this.headertitle = this.title
  }
  cancel() {
    this.modal.close('no')
  }
  confirmstatus() {
    this.modal.close('yes')
  }
}
