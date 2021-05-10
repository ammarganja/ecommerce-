import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-delete-order',
  templateUrl: './delete-order.component.html',
  styleUrls: ['./delete-order.component.scss'],
})
export class DeleteOrderComponent implements OnInit {
  @Input() id: number;
  isLoading = false;
  subscriptions: Subscription[] = [];

  constructor(public modal: NgbActiveModal) {}

  ngOnInit(): void {}

  cancel() {
    this.modal.close('no');
  }
  deleteOrder() {
    this.modal.close('yes');
  }
}
