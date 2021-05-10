import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-order-basic-detail',
  templateUrl: './order-basic-detail.component.html',
  styleUrls: ['./order-basic-detail.component.scss']
})
export class OrderBasicDetailComponent implements OnInit {

  @Input() orderDetail : any;
  constructor() { }

  ngOnInit(): void {
  }
  

}
