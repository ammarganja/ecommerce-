import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-edit-type',
  templateUrl: './edit-type.component.html',
  styleUrls: ['./edit-type.component.scss']
})
export class EditTypeComponent implements OnInit {
  
  @Input() customer: any;;


  countryData = [];
  stateData = [];
  singleSelectsettings;

  public modelType: any;
  customerFormObj = {
    id: null,
    name: null,
  };
  constructor(public modal: NgbActiveModal) { }

  ngOnInit(): void {

    this.singleSelectsettings = {
      singleSelection: true,
      enableSearchFilter: true,
    }

    if (this.customer) {
      this.customerFormObj = this.customer;
    }

  }
  
  submit() {
    this.modal.dismiss(EditTypeComponent);
  }

}
