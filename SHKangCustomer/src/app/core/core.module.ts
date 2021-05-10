import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreRoutingModule } from './core-routing.module';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { ProductComponent } from './components/product/product.component';
import { ProductSearchComponent } from './components/product-search/product-search.component';
import { CampaignBannerComponent } from './components/campaign-banner/campaign-banner.component';
import { AddressComponent } from './components/address/address.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { AboutUsComponent } from './components/about-us/about-us.component';
import { TermsComponent } from './components/terms/terms.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { SpinnerService } from './services/spinner/spinner.service';
import { SubscribersService } from './services/subscribers/subscribers.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor, SpinnerInterceptorService } from './interceptors';
import { ToastrModule } from 'ngx-toastr';
import { AuthGuardService } from './services/guards/auth-guard.service';
import { BaseService } from './utils/base.service';
import { Base64Service } from './utils/base64.service';
import { AuthService } from './services/auth/auth.service';
import { NotFound404Component } from './components/not-found404/not-found404.component';
import { UnAuhtorized401Component } from './components/un-auhtorized401/un-auhtorized401.component';
import { Forbidden403Component } from './components/forbidden403/forbidden403.component';
import { InternalServer500Component } from './components/internal-server500/internal-server500.component';
import { OwlModule } from 'ngx-owl-carousel';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { MultiSelectDropdownComponent } from './components/multi-select-dropdown/multi-select-dropdown.component';
import { DeleteConfirmationComponent } from './components/delete-confirmation/delete-confirmation.component';
import { ServerError500Component } from './components/server-error500/server-error500.component';
import { ImageViewComponent } from './components/image-view/image-view.component';

export const interceptorProviders =
  [
    { provide: HTTP_INTERCEPTORS, useClass: SpinnerInterceptorService, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ];

@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    ProductComponent,
    ProductSearchComponent,
    CampaignBannerComponent,
    AddressComponent,
    ChangePasswordComponent,
    AboutUsComponent,
    TermsComponent,
    PrivacyComponent,
    NotFound404Component,
    UnAuhtorized401Component,
    Forbidden403Component,
    InternalServer500Component,
    MultiSelectDropdownComponent,
    DeleteConfirmationComponent,
    ServerError500Component,
    ImageViewComponent,
  ],
  imports: [
    CommonModule,
    CoreRoutingModule,
    ToastrModule.forRoot(),
    OwlModule,
    TabsModule.forRoot(),
    ModalModule.forChild(),
    ReactiveFormsModule,
    FormsModule,
    NgbDropdownModule,
    NgbCollapseModule,
    NgMultiSelectDropDownModule.forRoot(),
  ],
  providers: [AuthService,
    SpinnerService,
    SubscribersService,
    interceptorProviders,
    AuthGuardService,
    BaseService,
    Base64Service,
    InfiniteScrollModule
  ],
  exports: [HeaderComponent, FooterComponent,ProductComponent,ProductSearchComponent,TabsModule,ModalModule,ReactiveFormsModule,FormsModule,NgbDropdownModule,NgbCollapseModule,InfiniteScrollModule,NgMultiSelectDropDownModule,OwlModule
  ]

})
export class CoreModule {
  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule
    };
  }
}
