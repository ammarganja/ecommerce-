import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from '../core/services/guards/auth-guard.service';
import { BillingDetailComponent } from './billing-detail/billing-detail.component';
import { CampaignDetailComponent } from './campaign-detail/campaign-detail.component';
import { CampaignComponent } from './campaign/campaign.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { CustomerComponent } from './customer.component';
import { HomeComponent } from './home/home.component';
import { MyOrdersComponent } from './my-orders/my-orders.component';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { OrderSummaryComponent } from './order-summary/order-summary.component';
import { OrderdetailssummaryComponent } from './orderdetailssummary/orderdetailssummary.component';
import { PaymentInvoiceComponent } from './payment-invoice/payment-invoice.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ProfileSettingComponent } from './profile-setting/profile-setting.component';
import { StyleComponent } from './style/style.component';
import { ThankYouComponent } from './thank-you/thank-you.component';

const routes: Routes = [
  {
    path: '',
    component: CustomerComponent,
    children: [
      {
        path: 'home',
        component: HomeComponent
      },
      {
        path: '', redirectTo: 'home', pathMatch: 'full'
      }
    ]
  },
  {
    path: 'styles',
    component: StyleComponent,
    canActivate : [AuthGuardService]
  },
  {
    path: 'campaign',
    component: CampaignComponent,
    canActivate : [AuthGuardService]
  },
  {
    path: 'campaign/:campignId',
    component: CampaignDetailComponent,
    canActivate : [AuthGuardService]
  },
  {
    path: 'product/:productId',
    component: ProductDetailComponent,
    canActivate : [AuthGuardService]
  },
  // {
  //   path: 'ordersummary',
  //   component: OrderdetailssummaryComponent
  // },
  {
    path: 'ordersummary',
    component: OrderSummaryComponent,
    canActivate : [AuthGuardService]
  },
  // {
  //   path: 'billingdetails',
  //   component: BillingDetailComponent
  // },
  {
    path: 'thankyou/:orderId',
    component: ThankYouComponent,
    canActivate : [AuthGuardService]
  },
  {
    path: 'payment/:invoiceId',
    component: PaymentInvoiceComponent,
    canActivate : [AuthGuardService]
  },
  {
    path: 'profile',
    component: ProfileSettingComponent,
    canActivate : [AuthGuardService],
    children: [
      {
        path: 'myorders',
        component: MyOrdersComponent,
        canActivate : [AuthGuardService]
      },
      {
        path: 'myprofile',
        component: MyProfileComponent,
        canActivate : [AuthGuardService]
      },
      {
        path: '', redirectTo: 'myorders', pathMatch: 'full'
      },
      {
        path: 'change-password',
        component: ChangePasswordComponent,
        canActivate : [AuthGuardService]
      }
    ]
  },
  { path: '**', redirectTo: '/404' },
  { path: '500', redirectTo: '/500' },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerModuleRoutingModule { }
