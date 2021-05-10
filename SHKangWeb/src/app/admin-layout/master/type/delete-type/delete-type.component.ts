import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-delete-type',
  templateUrl: './delete-type.component.html',
  styleUrls: ['./delete-type.component.scss']
})
export class DeleteTypeComponent implements OnInit {

  constructor( private model:NgbActiveModal) { }

  ngOnInit(): void {
  }
  deleteType(){

  }
  cancel(){
    this.model.dismiss(DeleteTypeComponent);
  }

}
