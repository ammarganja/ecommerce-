import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HelperService } from '../../helpers/helper-service.service';
import { CartService } from '../../services/customer_services/cart.service';
import { ProductService } from '../../services/customer_services/product.service';
import { ImageViewComponent } from '../image-view/image-view.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Base64Service } from '../../utils/base64.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit {

  @Input() isHome = true;
  @Input() product: any;
  item: any;
  imageList: any = [];
  colorList: any = [];
  qty: any = 1;
  actualPrice: any;
  productId: any = 0

  constructor(private productservice: ProductService, private cartService: CartService, private modal: NgbModal,
    private toastr: ToastrService, private router: Router,
    private helperService: HelperService,) { }

  addToCart() {
    var userData: any = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
    var params = {
      "UserId": userData.userId,
      "ProductId": this.product.productId,
      "ProductColorId": this.product.productColorId,
      "CartId": "0",
      "Quantity": 1,
      "Price": this.product.price,
      "CreatedBy": userData.userId,
      "UpdatedBy": userData.userId
    }


    this.cartService.addToCart(params).subscribe((response: any) => {
      if (response.result && response.status == 200) {
        var cartCountParams = { "UserId": userData.userId, }
        this.cartService.getCartCount(cartCountParams).subscribe((response: any) => {
          if (response.result && response.status == 200) {
            this.helperService.cartCount = response.data;
            this.toastr.success("Product has been added to your cart");
          } else {
            this.toastr.error(response.message);
            return;
          }
        })
        //this.router.navigate(['/customer/ordersummary'])
      } else {
        this.toastr.error(response.message);
        return;
      }
    });
  }
  
  ngOnInit(): void {
  }

  viewProductImage(productId: any) {
    var modalRef = this.modal.open(ImageViewComponent, {
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
}
