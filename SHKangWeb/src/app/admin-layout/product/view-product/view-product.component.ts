import { Component, Input, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { rompresData, dressesData } from "../product.data";
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { ProductsService } from '../../services/products.service';
@Component({
  selector: 'app-view-product',
  templateUrl: './view-product.component.html',
  styleUrls: ['./view-product.component.scss']
})

export class ViewProductComponent implements OnInit {
  
  constructor(private route: ActivatedRoute, public productService: ProductsService,) { }

  productList = [];
  rompres = [];
  dresses = [];
  public productId : any;
  productDetails: any;  
  ngOnInit(): void {
    // this.rompres =rompresData;
    // this.dresses = dressesData;
    // this.productList = this.rompres.concat(this.dresses);  
    this.productId = this.route.snapshot.paramMap.get("id");
    this.fetchProductData(this.productId)
  }

  fetchProductData(productId:string){
    this.productService.detailProduct(productId).subscribe((response: any) => {
      if (response.result) {
          this.productDetails = response.data;
          console.log('===========>', JSON.stringify(this.productDetails.imageList) );
      } else {
        Swal.fire({
          title: 'Error',
          text: response.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { })
      }
    });
   
  }

}
