import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';


@Component({
  selector: 'app-orderdetailssummary',
  templateUrl: './orderdetailssummary.component.html',
  styleUrls: ['./orderdetailssummary.component.scss']
})
export class OrderdetailssummaryComponent implements OnInit {
  
  @Input() Coupanbutton=true;
  constructor() { }


  ngOnInit(): void {
  }
  toggleOn :any;
  nextScreen($event:any){
  }

  onSelect(data: any): void {
    console.log(data);
    ////this.value = data.heading;
  }

  getActiveTab() {
  }
}
