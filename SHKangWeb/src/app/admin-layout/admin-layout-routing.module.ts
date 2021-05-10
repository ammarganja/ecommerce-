import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminLayoutComponent } from './admin-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { OrderListComponent } from './order-list/order-list.component';
import { CustomerComponent } from './customer/customer.component';
import { ProductListComponent } from './product/product-list.component';
import { ViewProductComponent } from './product/view-product/view-product.component';
import { ColorsComponent } from './master/colors/colors.component';
import { TypeComponent } from './master/type/type.component';
import { SizeComponent } from './master/size/size.component';
import { AddOrderComponent } from './order-list/add-order/add-order.component';
import { CategoryComponent } from './master/category/category.component';
import { CampaignComponent } from './campaign/campaign.component';
import { ProductpopupComponent } from './campaign/productpopup/productpopup.component';
import { CouponComponent } from './master/coupon/coupon.component';
import { ContentManagementComponent } from './content-management/content-management.component';
import { AuthGuard } from './auth.guard';
import { ConfirmationpopupComponent } from '../core/components/confirmationpopup/confirmationpopup.component';
import { ResetpasswordComponent } from '../auth/resetpassword/resetpassword.component';

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Dashboard',
        },
      },
      {
        path: 'orders',
        component: OrderListComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Orders',
        },
      },
      {
        path: 'addorder',
        component: AddOrderComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Add Order',
        },
      },
      {
        path: 'products',
        component: ProductListComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Products',
        },
      },
      {
        path: 'product-details/:id',
        component: ViewProductComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Products',
        },
      },
      {
        path: 'customers',
        component: CustomerComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Customers',
        },
      },
      {
        path: 'master/color',
        component: ColorsComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Colors',
        },
      },
      {
        path: 'master/type',
        component: TypeComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Types',
        },
      },
      {
        path: 'master/size',
        component: SizeComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Sizes',
        },
      },
      {
        path: 'master/category',
        component: CategoryComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Categories',
        },
      },
      {
        path: 'campaign',
        component: CampaignComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Campaigns',
        },
      },
      {
        path: 'productpopup',
        component: ProductpopupComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'promocode',
        component: CouponComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Promo Code',
        },
      },
      {
        path: 'content',
        component: ContentManagementComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Content',
        },
      },
      {
        path: 'confirmation',
        component: ConfirmationpopupComponent,
        canActivate: [AuthGuard],
        data: {
          breadcrumb: 'Confirmation',
        },
      },
      {
        path: 'resetpassword?code=code',
        component: ResetpasswordComponent
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
        data: {
          breadcrumb: 'Dashboard',
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminLayoutRoutingModule {}

