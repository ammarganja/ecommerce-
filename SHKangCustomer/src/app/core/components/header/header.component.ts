import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { LoginpopupComponent } from 'src/app/authmodule/loginpopup/loginpopup.component';
import { HelperService } from '../../helpers/helper-service.service';
import { CartService } from '../../services/customer_services/cart.service';
import { ProductService } from '../../services/customer_services/product.service';
import { SubscribersService } from '../../services/subscribers/subscribers.service';
import { Base64Service } from '../../utils/base64.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  isSessionTrue = false;
  isLogIn = false;
  searchCollpase = false;
  SearchString: any;
  productList: any;
  isSearchIcon = false;
  // @ViewChild('ngbDropDownModule') x: NgbDropdownToggle
  constructor(
    private model: NgbModal,
    public helperService: HelperService,
    private productService: ProductService,
    public suscriberService: SubscribersService,
    private cartService: CartService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (localStorage.getItem('token') !== null) {
      this.isSessionTrue = true;
    }
    window.location.href;
    let routeData = window.location.href.split('/');
    if (routeData[routeData.length - 1] == 'home') {
      this.isSearchIcon = true;
    }

    this.searchProducts('');
    if(localStorage.getItem('userdata')){
      this.getCartCount();
    }
    
  }
  searchTrue() {
    this.searchCollpase = true;
  }

  getCartCount() {
    var userData: any = JSON.parse(
      Base64Service.decode(localStorage.getItem('userdata'))
    );
    var cartCountParams = { UserId: userData.userId };
    this.cartService
      .getCartCount(cartCountParams)
      .subscribe((response: any) => {
        if (response.result && response.status == 200) {
          this.helperService.cartCount = response.data;
        } else {
          this.toastr.error(response.message);
          return;
        }
      });
  }

  searchProducts(searchTerm: string) {
    // setTimeout(() => {
    //   this.suscriberService.searchTheProduct(searchTerm);
    // }, 100);

    this.productService.getProductList(searchTerm).subscribe((res) => {
      if (res.result) {
        this.helperService.productList = res.data.productLists;
      }
    });
  }

  getActiveUrl() {
    let activeUrl = this.router.routerState.snapshot['url'];
    return activeUrl;
  }
  logout() {
    localStorage.clear();
    this.isSessionTrue = false;
    this.helperService.isLogIn = false;
    let lastUrl = this.getActiveUrl();
    localStorage.setItem('returnUrl', JSON.stringify(lastUrl));
  }

  LoginModelOpen() {
    this.model.open(LoginpopupComponent, {
      centered: true,
      windowClass: 'login-modal in',
    });
  }

  routerNavigation(url: any) {
    let routeData = url.split('/');
    if (routeData[routeData.length - 1] == 'home') {
      this.isSearchIcon = true;
    } else {
      this.isSearchIcon = false;
    }
    // this.router.navigate([url]);
  }
}
