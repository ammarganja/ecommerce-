import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-order-size-ratio',
  templateUrl: './order-size-ratio.component.html',
  styleUrls: ['./order-size-ratio.component.scss']
})
export class OrderSizeRatioComponent implements OnInit {

  @Input() order: any;

  orderRatio = {
    S: '',
    M: '',
    L: ''
  }
  constructor(public modal: NgbActiveModal) { }

  ngOnInit(): void {
    if(this.order){
      var ratios = this.order.ratio.split('/');
      if(ratios){
        this.orderRatio.S = ratios[0];
        this.orderRatio.M = ratios[1];
        this.orderRatio.L = ratios[2];
      }
    }
  }

  submit() {
    this.modal.dismiss(OrderSizeRatioComponent);
  }
}
