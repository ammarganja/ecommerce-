import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-coupan',
  templateUrl: './coupan.component.html',
  styleUrls: ['./coupan.component.scss']
})
export class CoupanComponent implements OnInit {

  constructor(private ngbActiveModal: NgbActiveModal) { }
  public couponCode : any;
  ngOnInit(): void {
  }

  close() {
    this.ngbActiveModal.close();
  }

  submit(){
    this.ngbActiveModal.close(this.couponCode);
    
  }
}
