import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/core/services/auth/auth.service';
import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CoupanComponent } from '../coupan/coupan.component';
import { Base64Service } from 'src/app/core/utils/base64.service';
import { UserService } from 'src/app/core/services/customer_services/user.service';
import { DeleteConfirmationComponent } from 'src/app/core/components/delete-confirmation/delete-confirmation.component';
import { EditUserAddressComponent } from '../edit-user-address/edit-user-address.component';
import { CartService } from 'src/app/core/services/customer_services/cart.service';
import { HelperService } from 'src/app/core/helpers/helper-service.service';

@Component({
  selector: 'app-order-summary',
  templateUrl: './order-summary.component.html',
  styleUrls: ['./order-summary.component.scss']
})

export class OrderSummaryComponent implements OnInit {
  dropdownSettings = {};
  coupanbutton = false;
  currentTabId = 0;
  isNewAddress = false;
  signupformobj: any;
  stateList: any = [];
  selectedState: any = [];
  selectedCountry: any = [];
  countryList: any = [];
  selectedAddress: any;
  promoCode: any;
  isCart = true;

  public appliedCouponCodeData: any;
  public orderProductList: any = [];
  public address: any;
  public userAddressList: any;
  public userOrderSummary: any;
  public emailNotificationState: any;
  public isCouponApplied = false;
  orderIdForSuccessTab: any;
  constructor(private userService: UserService,
    private modalService: NgbModal,
    private modal: NgbModal,
    private router: Router,
    private authService: AuthService,
    private toastr: ToastrService,
    private cartService: CartService,
    private helperService: HelperService,
    private changeDetectionRef: ChangeDetectorRef) {
    this.address = {
      fullName: "",
      companyName: "",
      emailId: "",
      phoneNumber: "",
      Address1: "",
      address2: "",
      stateId: "",
      countryId: "",
      country: "",
      state: "",
      city: "",
      zipcode: ""
    };
  }


  ngOnInit(): void {
    this.dropdownSettings = {
      singleSelection: true,
      idField: 'id',
      textField: 'itemName',
      itemsShowLimit: 5,
      allowSearchFilter: true
    };
    this.getUserOrderSummary()
    this.getUserAddressList();
    this.getCountryList();
    this.isCartdata();
  }
  toggleOn: any;
  nextScreen($event: any) {
  }

  onSelect(data: any): void {
    if (data && data.heading == 'Order Summary') {
      this.currentTabId = 0;
    }
    else if (data && data.heading == 'Billing Details') {
      this.currentTabId = 1;
    }
    else if (data && data.heading == 'Payment') {
      this.currentTabId = 2;
    } else {
      this.currentTabId = 0;
    }
    this.showCoupanButton();
  }

  getActiveTab() {
    if (this.currentTabId == 0) {
      this.currentTabId = 1;
    }
    else if (this.currentTabId == 1) {
      this.currentTabId = 2;
    } else if (this.currentTabId == 2) {
      this.router.navigate(['customer/thankyou/1']);
    }

    this.showCoupanButton();
  }

  showCoupanButton() {
    if (this.currentTabId == 2) {
      this.coupanbutton = true;
    }
    else {
      this.coupanbutton = false;
    }
  }

  checkAddress($event: any) {
    if ($event.target.checked) {
      this.isNewAddress = true;
    } else {
      this.isNewAddress = false;
    }
  }

  getUserOrderSummary() {
    let userData = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
    let requestParam = {
      "userId": userData.userId
    }
    this.authService.getUserOrderSummary(requestParam).subscribe((response: any) => {
      if (response.result && response.status == 200) {
        this.userOrderSummary = response.data;
      } else {
        this.toastr.error(response.message);
        return;
      }
    });
  }
  getUserAddressList() {
    let userData = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
    let requestParam = {
      "userId": userData.userId,
      "pageNo": 1,
      "limit": 10,
      "searchString": "",
      "column": "",
      "direction": ""
    }
    this.authService.getUserAddressList(requestParam).subscribe((response: any) => {
      if (response.result && response.status == 200) {
        this.userAddressList = response.data.items;
        let addressData = this.userAddressList.find((address: any) => address.isPrimary === true);
        this.selectedAddress = addressData.userAddressId;
      } else {
        this.toastr.error(response.message);
        return;
      }
    });
  }

  addUserAddress() {
    this.currentTabId = 2;
    if (this.isNewAddress) {
      this.isNewAddress = false;
      this.submit();
    }
  }

  updateCartQuantity(product: any, type: any) {
    let userData = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
    let requestParam = {
      "userId": userData.userId,
      "cartId": product.cartId,
      "productId": product.productId,
      "productColorId": product.colorId,
      "quantity": Number(product.quantity),
      "updatedBy": "2"
    }
    //type=="plus" ? requestParam.quantity + 1 : requestParam.quantity==1 ? requestParam.quantity : requestParam.quantity-1;
    if (type == 'plus') {
      requestParam.quantity++;
    }
    if (type == 'minus' && requestParam.quantity != 1) {
      requestParam.quantity--;
    }

    this.authService.updateUserOrderCart(requestParam).subscribe((response: any) => {
      if (response.result && response.status == 200) {
        this.getUserOrderSummary();
      } else {
        this.toastr.error(response.message);
        return;
      }
    });
  }

  convertToNumber(value: any) {
    return Number(value);
  }


  onCountrySelect(event: any) {
    this.stateList = [];
    this.selectedState = [];
    this.getStates();
  }

  onCountryDeSelect(event: any) {
    this.stateList = [];
    this.selectedState = [];
  }

  getStates() {
    if (this.selectedCountry && this.selectedCountry.length > 0) {
      this.userService.getStates(this.selectedCountry[0].id).subscribe((response: any) => {
        if (response.result) {
          this.stateList = response.data;
          if (this.address && this.address.stateId) {
            let arr: any[] = [{ id: this.address.stateId.toString(), itemName: this.address.state }];
            this.selectedState = arr;
          }
        } else {
          this.toastr.error(response.message)
        }
      });
    } else {
      this.stateList = [];
      this.selectedState = [];
    }
  }
  getCountryList() {
    this.userService.getCountries().subscribe((response: any) => {
      if (response.result) {
        this.countryList = response.data;
        if (this.address && this.address.countryId) {
          let arr: any[] = [{ id: this.address.countryId.toString(), itemName: this.address.country }];
          this.selectedCountry = arr;
          this.changeDetectionRef.markForCheck();

          this.getStates();
        }

      } else {
        this.toastr.error(response.message);
        return
      }
    });
  }



  submit() {
    if (this.selectedCountry.length == 0) {
      this.toastr.error("Please select Country");
      return;
    }

    if (this.selectedState.length == 0) {
      this.toastr.error("Please select State");
      return;
    }

    var userData = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
    var requeseParam = {
      "UserAddressId": this.address.userAddressId ? this.address.userAddressId : 0,
      "Address1": this.address.address1,
      "Address2": this.address.address2,
      "CountryId": this.selectedCountry[0].id,
      "City": this.address.city,
      "StateId": this.selectedState[0].id,
      "ZipCode": this.address.zipcode,
      "IsPrimary": this.address.isPrimary,
      "FullName": this.address.fullName ? this.address.fullName : '',
      "PhoneNumber": this.address.phoneNumber ? this.address.phoneNumber : '',
      "CompanyName": this.address.companyName ? this.address.companyName : '',
      "Email": this.address.emailId ? this.address.emailId : '',
      "UserId": userData.userId
    }

    this.userService.saveUserAddress(requeseParam).subscribe((response: any) => {
      if (response.result && response.status == 200) {
        this.toastr.success(response.message);
        this.getUserAddressList();
        return
      } else {
        this.toastr.error(response.message);
        return
      }
    });
  }



  /**
   * Remove User Cart Product
  */
  removeUserProduct(product: any) {
    const modalRef = this.modalService.open(DeleteConfirmationComponent);
    let data = {
      id: product.productId,
      key: product.name,
    }
    modalRef.componentInstance.data = data;
    modalRef.result.then((res) => {
      if (res === 'yes') {
        let userData = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
        let requestParam = {
          "userId": userData.userId,
          "cartId": product.cartId
        }
        this.authService.removeCartProduct(requestParam).subscribe((response: any) => {
          if (response.result) {
            this.toastr.success(response.message);
            this.getUserOrderSummary();
            var cartCountParams = { "UserId": userData.userId, }
            this.cartService.getCartCount(cartCountParams).subscribe((response: any) => {
              if (response.result && response.status == 200) {
                this.helperService.cartCount = response.data
                if (response.data == 0) {
                  this.isCart = false;
                  console.log(this.isCart, "sdfasdf")
                }
                else {
                  this.isCart = true;
                }
              } else {
                this.toastr.error(response.message);
                return;
              }
            })
          } else {
            this.toastr.error(response.message);
          }
        });
      }
    });
  }

  deleteUserAddress(userAddressId: any) {
    let requestParam = {
      "userAddressId": userAddressId
    }
    this.authService.deleteUserAddress(requestParam).subscribe((response: any) => {
      if (response.result && response.status == 200) {
        this.toastr.success(response.message);
        this.getUserAddressList();
      } else {
        this.toastr.error(response.message);
        return;
      }
    });
  }

  selectAddress(userAddressId: any) {
    this.selectedAddress = userAddressId;
  }

  editAddress(address: any = {}) {
    var modalRef = this.modal.open(EditUserAddressComponent, {
      windowClass: 'login-modal in',
      centered: true,
    });
    modalRef.componentInstance.address = address;
    modalRef.result.then((res) => {
      // this.getUserAddresses();
    });
  }

  onEmailNotificationOn($event: any) {
    this.emailNotificationState = $event.srcElement.checked;
  }
  checkOut() {
    if(this.selectedAddress){
      this.orderProductList = [];
      let userData = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
      
      this.userOrderSummary.productDetail.forEach((element: any) => {
        let product = {
          "ProductId": element.productId,
          "Price": Number(element.price),
          "Quantity": Number(element.quantity),
          "ProductColorId": element.colorId
        }
        this.orderProductList.push(product);
      });
  
  
      let requestParam = {
        "OrderId": "0",
        "UserId": userData.userId,
        "CreatedBy": "2",
        "UserAddressId": this.selectedAddress,
        "IsMailSend": this.emailNotificationState ? "1" : "0",
        "Promocode": this.promoCode ? this.promoCode : 0,
        "ProductDetail": this.orderProductList
      }
  
      this.authService.addUserOrder(requestParam).subscribe((response: any) => {
        if (response.result && response.status == 200) {
          this.toastr.success(response.message);
          this.getActiveTab();
        } else {
          this.toastr.error(response.message);
          return;
        }
      });
    } else {
      this.toastr.error("Shipping address can not be empty.");
      this.currentTabId=0;
      this.getActiveTab();
    }
  }


  applyCoupan() {
    var modalRef = this.modal.open(CoupanComponent, {
      windowClass: 'login-modal in coupan_modal',
      centered: true,
      animation: true,
      backdrop: true,
      backdropClass: 'modal-backdrop fade in'
    });
    modalRef.result.then((res) => {
      if (res) {

        let userData = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));

        let requestParam = {
          "UserId": userData.userId,
          "Code": res
        }

        this.authService.checkPromoCode(requestParam).subscribe((response: any) => {
          if (response.result && response.status == 200) {
            this.isCouponApplied = true;
            this.appliedCouponCodeData = response.data;
            this.promoCode = response.data.promocodeModel.promocodeId;
            //this.toastr.success(response.message);
          } else {
            this.toastr.error(response.message);
            return;
          }
        });
      }
    });
  }

  removeCouponCode() {
    this.promoCode = 0;
    this.isCouponApplied = false;
  }

  isCartdata() {
    let userData = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
    var cartCountParams = { "UserId": userData.userId, }
    this.cartService.getCartCount(cartCountParams).subscribe((response: any) => {
      if (response.result && response.status == 200) {
        this.helperService.cartCount = response.data
        if (response.data == 0) {
          this.isCart = false;
          console.log(this.isCart, "sdfasdf")
        }
        else {
          this.isCart = true;
        }
      } else {
        this.toastr.error(response.message);
      }
    })
  }
}
