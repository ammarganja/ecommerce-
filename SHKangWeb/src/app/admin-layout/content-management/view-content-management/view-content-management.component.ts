import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-view-content-management',
  templateUrl: './view-content-management.component.html',
  styleUrls: ['./view-content-management.component.scss']
})
export class ViewContentManagementComponent implements OnInit {

  @Input() description;
  contentFormObj= {
    name:null,
    short_name : null,
    description:null
  };


  constructor(public modal: NgbActiveModal) { 
  }

  ngOnInit(): void {
    this.contentFormObj.description = this.description; 
  }
  submit() {

  }
}
