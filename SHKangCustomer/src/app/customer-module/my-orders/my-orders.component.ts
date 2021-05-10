import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { from } from 'rxjs';
import { CartService } from 'src/app/core/services/customer_services/cart.service';
import { MyorderService } from 'src/app/core/services/customer_services/myorder.service';
import { Base64Service } from 'src/app/core/utils/base64.service';
import { ViewInvoiceComponent } from '../view-invoice/view-invoice.component';

@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.scss'],
})
export class MyOrdersComponent implements OnInit {
  readypayment = false;
  isCollapsed = false;
  myOrder: any;
  orderinvoiceTRackStatus: any;
  constructor(
    private myorderService: MyorderService,
    private cartService: CartService,
    private model: NgbModal,
    private router: Router
  ) {
    this.isCollapsed = false;
    
  }

  ngOnInit(): void {
    this.myOrders();
  }

  addToProduct(product: any) {
    this.cartService.addToProduct(product);
    window.alert('Your product has been added to the cart!');
  }

  dateToday: number = Date.now();

  userdata = Base64Service.decode(localStorage.getItem('userdata'));

  ordersparams = {
    userId: JSON.parse(this.userdata).userId,
  };

  handleButtonClick(invoiceId: any) {
    this.myOrder.forEach((item: any) => {
      item.invoiceList.forEach((inData: any) => {
        if (
          inData.orderInvoiceId == invoiceId &&
          inData.isCollapsed == true &&
          inData.status == 'Delivered'
        ) {
          inData.isCollapsed = false;
        } else if (
          inData.orderInvoiceId == invoiceId &&
          inData.isCollapsed == false &&
          inData.status == 'Delivered'
        ) {
          inData.isCollapsed = true;
        } else {
          inData.isCollapsed = false;
        }
      });
    });
  }

  myOrders() {
    this.myorderService.getMyOrder(this.ordersparams).subscribe((res: any) => {
      if (res.result) {
        this.myOrder = res.data;
        this.myOrder.forEach((orderData: any) => {
          orderData.invoiceList.forEach((invoice: any) => {
            if (invoice.status == 'Delivered') {
              invoice.isCollapsed = false;
            }
          });
        });
      }
    });
  }
  orderInvoice(invoiceid: number) {
    let modelref = this.model.open(ViewInvoiceComponent, {
      centered: true,
      windowClass: 'login-modal in',
    });
    modelref.componentInstance.InvoiceId = invoiceid;
  }
  trackInvoice(invoiceid: number) {
    let data= {
      orderInvoiceId:invoiceid
    }
    this.myorderService
      .getTrackingDetails(data)
      .subscribe((res: any) => {
        if(res.result){
          this.orderinvoiceTRackStatus = res.data
        }
      });
  }
}
