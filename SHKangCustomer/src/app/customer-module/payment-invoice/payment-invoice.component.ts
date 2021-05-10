import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Constant } from 'src/app/core/constants/constants';
import { HelperService } from 'src/app/core/helpers/helper-service.service';
import { MyorderService } from 'src/app/core/services/customer_services/myorder.service';

@Component({
  selector: 'app-payment-invoice',
  templateUrl: './payment-invoice.component.html',
  styleUrls: ['./payment-invoice.component.scss']
})


export class PaymentInvoiceComponent implements OnInit {
  invoiceId: any;
  invoiceProducts: any;
  orderDetails: any;
  strikeCheckout: any = null;
  stripePromise: any;
  paymentData: any;
  constructor(private route: ActivatedRoute,
    private myorder: MyorderService,
    private helperservice: HelperService,
    private toastrService: ToastrService,
    private router: Router) {
    this.invoiceId = this.route.snapshot.params.invoiceId;
  }

  ngOnInit(): void {
    if (this.invoiceId) {
      let data = {
        orderInvoiceId: this.invoiceId,
      };
      this.myorder.getInvoiceDetails(data).subscribe((res: any) => {
        if (res.result) {
          this.invoiceProducts = res.data.productDetails
          this.orderDetails = res.data
        }
      })
    }
    this.stripePaymentGateway();
  }
  checkout(amount: any) {
    let userdata = localStorage.getItem('userdata');
    let userID: any;
    if (userdata) {
      userID = JSON.parse(this.helperservice.decodeData(userdata)).userId;
    }
    let self = this;
    const paymentHandler = (<any>window).StripeCheckout.configure({
      key: 'pk_test_51InKC6SEdhE1kSSow3tyllKc5DhBneIIO8slat3d97Cspx8X8GniZdNbsmLNSSXVE03KubZWErpJS7c610LismgF00vRWZPU3v',
      locale: 'auto',
      token: function (stripeToken: any) {
        self.paymentData = {
          StripeEmail: stripeToken.email,
          StripeToken: stripeToken.id,
          LoggedInUserId: userID,
          OrderInvoiceId: self.invoiceId,
          Amount: amount
        }
        self.myorder.MakePayment(self.paymentData).subscribe((res: any) => {
          if (res.result) {
            self.router.navigate(['/customer/styles']);
          } else {
            self.toastrService.error(res.message);
          }
        })
      }
    });

    paymentHandler.open({
      name: 'Positronx',
      description: '3 widgets',
      amount: amount * 100
    });
  }

  
  pay(data: any) {
    this.myorder.MakePayment(data).subscribe((res: any) => {
      console.log(res)
      debugger
    })

  }

  stripePaymentGateway() {
    var self = this;
    if (!window.document.getElementById('stripe-script')) {
      const script = window.document.createElement("script");
      script.id = "stripe-script";
      script.type = "text/javascript";
      script.src = 'https://checkout.stripe.com/checkout.js';
      script.onload = () => {
        self.strikeCheckout = (<any>window).StripeCheckout.configure({
          key: 'pk_test_51InKC6SEdhE1kSSow3tyllKc5DhBneIIO8slat3d97Cspx8X8GniZdNbsmLNSSXVE03KubZWErpJS7c610LismgF00vRWZPU3v',
          locale: 'auto',
          token: function (stripeToken: any) {
            self.toastrService.success("Payment has been successfull!");
          }
        });
      }

      window.document.body.appendChild(script);
    }
  }


}
