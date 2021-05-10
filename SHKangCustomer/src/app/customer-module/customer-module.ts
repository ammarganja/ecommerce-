import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerModuleRoutingModule } from './customer-module-routing.module';
import { HomeComponent } from './home/home.component';
import { BannerComponent } from './banner/banner.component';
import { CampaignComponent } from './campaign/campaign.component';
import { CustomerComponent } from './customer.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { OrderSummaryComponent } from './order-summary/order-summary.component';
import { BillingDetailComponent } from './billing-detail/billing-detail.component';
import { PaymentComponent } from './payment/payment.component';
import { CoupanComponent } from './coupan/coupan.component';
import { ThankYouComponent } from './thank-you/thank-you.component';
import { MyOrdersComponent } from './my-orders/my-orders.component';
import { CoreModule } from '../core/core.module';
import { CampaignDetailComponent } from './campaign-detail/campaign-detail.component';
import { StyleComponent } from './style/style.component';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { ChangePasswordComponent } from './change-password/change-password.component';

@NgModule({
  declarations: [
    HomeComponent,
    BannerComponent,
    CampaignComponent,
    CustomerComponent,
    ProductDetailComponent,
    OrderSummaryComponent,
    BillingDetailComponent,
    PaymentComponent,
    CoupanComponent,
    ThankYouComponent,
    MyOrdersComponent,
    CampaignDetailComponent,
    StyleComponent,
    MyProfileComponent,
    ChangePasswordComponent,
  ],
  imports: [
    CommonModule,
    CustomerModuleRoutingModule,
    CoreModule,
  ]
})
export class CustomerModule { }
