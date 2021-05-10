import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HelperService } from 'src/app/core/helpers/helper-service.service';
import { CartService } from 'src/app/core/services/customer_services/cart.service';
import { ProductService } from 'src/app/core/services/customer_services/product.service';
import { Base64Service } from 'src/app/core/utils/base64.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {

  //#region variable declaration
  item : any;
  imageList : any = [];
  colorList : any = [];
  qty : any = 1;
  actualPrice: any;
  productId : any = 0
  //#endregion

  //#region constructor
  constructor(private route: ActivatedRoute,
    private productDetailService: ProductService,
    private toastr: ToastrService,
    private changedetactionRef : ChangeDetectorRef,
    private cartService: CartService,
    private helperService: HelperService,

    private router : Router) { }
  //#endregion

  //#region functions
  ngOnInit(): void {
    var productId = this.route.snapshot.params.productId;
    this.productId = productId;
    this.getProductDetail(productId);
  }
  
  /**
   * get product detail
   * @param productId 
   */
  getProductDetail(productId:any){
    var params = {
      productId : productId
    }
    this.productDetailService.getProductDetail(params).subscribe((response: any) => {
      if(response.result && response.status == 200){
        this.item = response.data;
        this.actualPrice = this.item.price;
        this.colorList = this.item.colorList;
        this.setSelectedColor();

        if(this.item && this.item.colorList && this.item.colorList.length > 0){
          var firstColor = this.item.colorList[0];
          this.getColorImage(firstColor.productColorId);
        }
        this.changedetactionRef.markForCheck();
      }else{
        this.toastr.error(response.message);
        return;
      }
    });
  }

  /**
   * get color image
   * @param productColorId 
   */
  getColorImage(productColorId:any){
    if(this.item.imageList && this.item.imageList.length > 0){
      var filterImage = this.item.imageList.filter((a: any) => a.productColorId == productColorId);
      if(filterImage && filterImage.length > 0){
        this.imageList = filterImage;
      }
    }
  }

  /**
   * get Category List
   * @returns 
   */
  getCategoryList(){
    var list : any = [];
    if(this.item){
      this.item.categoryList.forEach((category:any) => {
        list.push(category.categoryType);
      });
    }
    
    return list.join('/')
  }

  /**
   * set selected color
   */
  setSelectedColor(){
    this.colorList.forEach((color:any,index:any) => {
      if(index == 0){
        color.selected = true;
      }else{
        color.selected = false;
      }
    });
  }

  /**
   * select the color
   */
  selectColor(productColorId:any){
    this.colorList.forEach((color:any,index:any) => {
      color.selected = false;
      if(color.productColorId == productColorId){
        color.selected = true;
      }
    })

    this.getColorImage(productColorId);
  }

  /**
   * minus the qty
   * @param item 
   */
  minus(item:any){
    if(this.qty > 1){
      this.qty = this.qty - 1;
      item.price = (+item.price) - (+this.actualPrice);
    }
  }

  /**
   * plus the qty
   * @param item 
   */
  plus(item:any){
    this.qty = this.qty + 1;
    item.price = (+this.qty) * (+this.actualPrice);
  }

  /**
   * add the product to cart
   */
  addToCart(){
    var userData : any = JSON.parse(Base64Service.decode(localStorage.getItem('userdata')));
    var selectedColor = this.colorList.filter((a:any)=>a.selected);
    var params = {
      "UserId": userData.userId,
      "ProductId": this.productId,
      "ProductColorId": selectedColor && selectedColor.length > 0 ? selectedColor[0].productColorId_1 : 0,
      "CartId": "0",
      "Quantity": this.qty,
      "Price": this.actualPrice,
      "CreatedBy": userData.userId,
      "UpdatedBy": userData.userId
    }


    this.cartService.addToCart(params).subscribe((response: any) => {
      if(response.result && response.status == 200){
        var cartCountParams = {"UserId": userData.userId,}
        this.cartService.getCartCount(cartCountParams).subscribe((response: any) => {
          if(response.result && response.status == 200){
            this.helperService.cartCount= response.data
          }else{
            this.toastr.error(response.message);
        return;
          }
        })
        this.router.navigate(['/customer/ordersummary'])
      }else{
        this.toastr.error(response.message);
        return;
      }
    });
  }

  //#endregion
}
