import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from 'src/app/core/services/customer_services/product.service';

@Component({
  selector: 'app-style',
  templateUrl: './style.component.html',
  styleUrls: ['./style.component.scss'],
})
export class StyleComponent implements OnInit {
  public productItems: any = [];
  total = 0
  throttle = 300;
  scrollDistance = 1;
  scrollUpDistance = 2;
  
  public requestParam = {
    "searchString": "",
    "column": "",
    "pageNo": 1,
    "CategoryTypeId": "",
    "limit": 10,
    "direction": "",
    "productCategoryTypeId":""
  }
  constructor(private productDetailService: ProductService,private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadProductItems();
  }

  loadProductItems(isLoadMore: any = false) {
    if (isLoadMore) {
      this.requestParam.pageNo = this.requestParam.pageNo + 1;
    }
    if (this.productItems.length != this.total || this.total == 0) {
      this.productDetailService
        .getProductItems(this.requestParam)
        .subscribe((response: any) => {
          if (response.result && response.status == 200) {
            response.data.productLists.forEach((element: any) => {
              this.productItems.push(element);
            });
            this.total = response.data.totalProducts;
          } else {
            this.toastr.error(response.message);
            return;
          }
        });
    }
  }

  getProductSearchText(text:any){
    this.productItems = [];
    this.requestParam.searchString = text;
    this.requestParam.pageNo = 1;
    this.loadProductItems();
  }

  getProductFilterText(text:any){
    this.productItems = [];
    this.requestParam.column = text;
    this.requestParam.pageNo = 1;
    this.loadProductItems();
  }
  getCategorySearch(event:any){
    this.productItems = [];
    this.requestParam.CategoryTypeId = event;
    this.requestParam.pageNo = 1;
    this.loadProductItems();
  }
}
