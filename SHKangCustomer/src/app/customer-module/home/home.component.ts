import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { LoginpopupComponent } from 'src/app/authmodule/loginpopup/loginpopup.component';
import { ResetPasswordComponent } from 'src/app/authmodule/reset-password/reset-password.component';
import { ImageViewComponent } from 'src/app/core/components/image-view/image-view.component';
import { HelperService } from 'src/app/core/helpers/helper-service.service';
import { CampaignService } from 'src/app/core/services/customer_services/campaign.service';
import { CartService } from 'src/app/core/services/customer_services/cart.service';
import { ProductService } from 'src/app/core/services/customer_services/product.service';
import { SubscribersService } from 'src/app/core/services/subscribers/subscribers.service';
import { Base64Service } from 'src/app/core/utils/base64.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  code: any;
  params = {
    pageNo: 1,
    limit: 5,
    searchString: '',
    column: '',
    direction: '',
    Status: 0,
  };
  HomeCampaign: any;
  SlideOptions = { items: 1, dots: false, nav: false };
  CarouselOptions = { items: 2, dots: false, nav: false };
  productList: any;
  total = 0;
  throttle = 300;
  scrollDistance = 1;
  scrollUpDistance = 2;

  public requestParam = {
    searchString: '',
    column: '',
    pageNo: 1,
    CategoryTypeId: '',
    limit: 5,
    direction: '',
  };

  constructor(
    private route: ActivatedRoute,
    public model: NgbModal,
    private campaign: CampaignService,
    private product: ProductService,
    public toastr: ToastrService,
    public helperService:HelperService,
    public subscribersService : SubscribersService,
    private cartService : CartService,
    private router : Router
  ) {
   
  }

  ngOnInit() {
    this.code = this.route.snapshot.params.code;
    if (this.code) {
      this.openModelReset();
    }
    this.getCampaign();
    this.productList = this.helperService.productList;
  }
  openModelReset() {
    let modelref = this.model.open(ResetPasswordComponent, {
      centered: true,
      windowClass: 'login-modal in',
    });
    modelref.componentInstance.code = this.code;
  }
  getCampaign() {
    this.campaign
      .getTokenlessCampaignList(this.params)
      .subscribe((res: any) => {
        if (res.result) {
          this.HomeCampaign = res.data.items;
          this.total = res.data.total;
        }
      });
  }
  loginData(){
    this.model.open(LoginpopupComponent,{windowClass:'Login-modal in',centered:true})
  }

  LoginModelOpen(product:any) {
    if(!this.helperService.isLogIn){
      this.model.open(LoginpopupComponent, {
        centered: true,
        windowClass: 'login-modal in',
      });
    }else{
      this.addToCart(product);
    }
  }

  addToCart(product:any){
    var userData : any = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
  
    var params = {
      "UserId": userData.userId,
      "ProductId": product.productId,
      "ProductColorId": product.productColorId,
      "CartId": "0",
      "Quantity": 1,
      "Price": product.price,
      "CreatedBy": userData.userId,
      "UpdatedBy": userData.userId
    }


    this.cartService.addToCart(params).subscribe((response: any) => {
      if(response.result && response.status == 200){
        var cartCountParams = {"UserId": userData.userId,}
        this.cartService.getCartCount(cartCountParams).subscribe((response: any) => {
          if(response.result && response.status == 200){
            this.helperService.cartCount= response.data
            this.toastr.success('Product successfully added to cart');
          }else{
            this.toastr.error(response.message);
        return;
          }
        })
        // this.router.navigate(['/customer/ordersummary'])
      }else{
        this.toastr.error(response.message);
        return;
      }
    });
  }
  viewProductImage(productId: any) {
    var modalRef = this.model.open(ImageViewComponent, {
      windowClass: 'login-modal in img_view',
      size : 'sm',
      centered: true,
      animation: true,
      backdrop: true,
      backdropClass: 'modal-backdrop fade in'
    });
    modalRef.componentInstance.productId = productId;
    modalRef.result.then((res) => {
      // this.getUserAddresses();
    });
  }

  // loadProductItems(isLoadMore: any = false) {
  //   if (isLoadMore) {
  //     this.requestParam.pageNo = this.requestParam.pageNo + 1;
  //   }
  //   if (this.productList.length != this.total || this.total == 0) {
  //     this.product
  //       .getProductItems(this.requestParam)
  //       .subscribe((response: any) => {
  //         if (response.result && response.status == 200) {
  //           this.productList = response.data.items;
  //           this.total = response.data.totalProducts;
  //         } else {
  //           this.toastr.error(response.message);
  //           return;
  //         }
  //       });
  //   }
  // }
}
