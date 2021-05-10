import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-deleteconfirmation',
  templateUrl: './deleteconfirmation.component.html',
  styleUrls: ['./deleteconfirmation.component.scss']
})
export class DeleteconfirmationComponent implements OnInit {

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
