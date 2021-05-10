import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-delete-address',
  templateUrl: './delete-address.component.html',
  styleUrls: ['./delete-address.component.scss']
})
export class DeleteAddressComponent implements OnInit {

  constructor(private modelservice:NgbActiveModal) { }

  ngOnInit(): void {
  }
  deleteAddress(){
    this.modelservice.dismiss(DeleteAddressComponent);
  }

  cancel(){
    this.modelservice.dismiss(DeleteAddressComponent);
  }

}
