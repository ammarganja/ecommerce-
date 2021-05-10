import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule } from './authmodule/auth.module';
import { CoreModule } from './core/core.module';
import { CustomerModule } from './customer-module/customer-module';
import { ProfileSettingComponent } from './customer-module/profile-setting/profile-setting.component';
import { OrderdetailssummaryComponent } from './customer-module/orderdetailssummary/orderdetailssummary.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UserPersonalInfoComponent } from './customer-module/user-personal-info/user-personal-info.component';
import { EditUserAddressComponent } from './customer-module/edit-user-address/edit-user-address.component';
import { ViewInvoiceComponent } from './customer-module/view-invoice/view-invoice.component';
import { PaymentInvoiceComponent } from './customer-module/payment-invoice/payment-invoice.component';



@NgModule({
  declarations: [
    AppComponent,
    ProfileSettingComponent,
    OrderdetailssummaryComponent,
    UserPersonalInfoComponent,
    EditUserAddressComponent,
    ViewInvoiceComponent,
    PaymentInvoiceComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CoreModule.forRoot(),
    CustomerModule,
    AuthModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
  exports:[],

})
export class AppModule { }
