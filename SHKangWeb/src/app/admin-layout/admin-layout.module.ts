import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminLayoutRoutingModule } from './admin-layout-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AdminLayoutComponent } from './admin-layout.component';
import { ScriptsInitComponent } from './init/scipts-init/scripts-init.component';
import { AsideComponent } from './components/aside/aside.component';
import { MixedWidget1Component } from '../partials/content/widgets/mixed/mixed-widget1/mixed-widget1.component';
import { InlineSVGModule } from 'ng-inline-svg';
import { NgApexchartsModule } from 'ng-apexcharts';
import {
  NgbDatepickerModule,
  NgbDropdownModule,
} from '@ng-bootstrap/ng-bootstrap';
import { DropdownMenu1Component } from '../partials/content/dropdown-menus/dropdown-menu1/dropdown-menu1.component';
import { DropdownMenu2Component } from '../partials/content/dropdown-menus/dropdown-menu2/dropdown-menu2.component';
import { DropdownMenu3Component } from '../partials/content/dropdown-menus/dropdown-menu3/dropdown-menu3.component';
import { DropdownMenu4Component } from '../partials/content/dropdown-menus/dropdown-menu4/dropdown-menu4.component';
import { OrderListComponent } from './order-list/order-list.component';
import { CRUDTableModule } from '../shared/crud-table';
import { FormGroup, FormsModule } from '@angular/forms';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { CustomerComponent } from './customer/customer.component';
import { MatTableModule } from '@angular/material/table';
import { ProductListComponent } from './product/product-list.component';
import { EditProductComponent } from './product/edit-product/edit-product.component';
import { HeaderMobileComponent } from './components/header-mobile/header-mobile.component';
import { EditCustomerComponent } from './customer/edit-customer/edit-customer.component';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { DemoMaterialModule } from '../../material-module';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';
import { ColorsComponent } from './master/colors/colors.component';
import { EditColorComponent } from './master/colors/edit-color/edit-color.component';
import { TypeComponent } from './master/type/type.component';
import { EditTypeComponent } from './master/type/edit-type/edit-type.component';
import { DeleteTypeComponent } from './master/type/delete-type/delete-type.component';
import { SizeComponent } from './master/size/size.component';
import { EditSizeComponent } from './master/size/edit-size/edit-size.component';
import { AddOrderComponent } from './order-list/add-order/add-order.component';
import { CategoryComponent } from './master/category/category.component';
import { EditCategoryComponent } from './master/category/edit-category/edit-category.component';
import { CampaignComponent } from './campaign/campaign.component';
import { ProductpopupComponent } from './campaign/productpopup/productpopup.component';
import { ProductselectionComponent } from './campaign/productselection/productselection.component';
import { CouponComponent } from './master/coupon/coupon.component';
import { EditCouponComponent } from './master/coupon/edit-coupon/edit-coupon.component';
import { ContentManagementComponent } from './content-management/content-management.component';
import { EditContentManagementComponent } from './content-management/edit-content-management/edit-content-management.component';
// import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CKEditorModule } from 'ckeditor4-angular';
import { PaymentStatusComponent } from './order-list/payment-status/payment-status.component';
import { ViewContentManagementComponent } from './content-management/view-content-management/view-content-management.component';
import { EditCampaignComponent } from './campaign/edit-campaign/edit-campaign.component';
import { DeleteOrderComponent } from './order-list/delete-order/delete-order.component';
import { OrderSizeRatioComponent } from './order-size-ratio/order-size-ratio.component';
import { AddProductComponent } from './add-product/add-product.component';
import { CreateInvoiceComponent } from './create-invoice/create-invoice.component';
import { AddressManagementComponent } from './customer/address-management/address-management.component';
import { EditAddressComponent } from './customer/address-management/edit-address/edit-address.component';
import { DeleteAddressComponent } from './customer/address-management/delete-address/delete-address.component';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { OrderBasicDetailComponent } from './order-basic-detail/order-basic-detail.component';
import { ViewProductComponent } from './product/view-product/view-product.component';
@NgModule({
  declarations: [
    DashboardComponent,
    AdminLayoutComponent,
    ScriptsInitComponent,
    AsideComponent,
    MixedWidget1Component,
    DropdownMenu1Component,
    DropdownMenu2Component,
    DropdownMenu3Component,
    DropdownMenu4Component,
    AsideComponent,
    FooterComponent,
    HeaderComponent,
    HeaderMobileComponent,
    OrderListComponent,
    ProductListComponent,
    CustomerComponent,
    EditProductComponent,
    EditCustomerComponent,
    ColorsComponent,
    EditColorComponent,
    TypeComponent,
    EditTypeComponent,
    DeleteTypeComponent,
    SizeComponent,
    EditSizeComponent,
    AddOrderComponent,
    CategoryComponent,
    EditCategoryComponent,
    CampaignComponent,
    ProductpopupComponent,
    ProductselectionComponent,
    CouponComponent,
    EditCouponComponent,
    ContentManagementComponent,
    EditContentManagementComponent,
    PaymentStatusComponent,
    ViewContentManagementComponent,
    EditCampaignComponent,
    DeleteOrderComponent,
    OrderSizeRatioComponent,
    AddProductComponent,
    CreateInvoiceComponent,
    AddressManagementComponent,
    EditAddressComponent,
    OrderBasicDetailComponent,
    DeleteAddressComponent,
    ViewProductComponent,
  ],
  imports: [
    CommonModule,
    AdminLayoutRoutingModule,
    InlineSVGModule,
    NgApexchartsModule,
    NgbDropdownModule,
    CRUDTableModule,
    FormsModule,
    MatTableModule,
    NgxDatatableModule,
    DemoMaterialModule,
    AngularMultiSelectModule,
    NgbDatepickerModule,
    CKEditorModule,
    NgxDropzoneModule,
  ],
  providers: [],
  schemas: [NO_ERRORS_SCHEMA],
})
export class AdminLayoutModule {}
