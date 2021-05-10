import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-delete-confirmation',
  templateUrl: './delete-confirmation.component.html',
  styleUrls: ['./delete-confirmation.component.scss']
})
export class DeleteConfirmationComponent implements OnInit {

  @Input() data: any;
  isLoading = false;
  constructor(public modal: NgbActiveModal) { }

  ngOnInit(): void {
  }

  cancel() {
    this.modal.close('no')
  }
  delete() {
    this.modal.close('yes')
  }

}
