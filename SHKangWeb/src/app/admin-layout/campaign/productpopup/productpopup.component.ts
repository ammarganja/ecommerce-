import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductListComponent } from '../../product/product-list.component';

@Component({
  selector: 'app-productpopup',
  templateUrl: './productpopup.component.html',
  styleUrls: ['./productpopup.component.scss']
})
export class ProductpopupComponent implements OnInit {

  //#region Variable Declaration

  userselected = [];
  customerselected = [];
  singleSelectsettings;
  data = [];
  style = [];
  StyleSelected = [];
  uservals = [];
  selectedUserEmail: string
  selectedUserName: string
  selectedUserAddress: string;
  myValue = false;

  rompres = [
    { selectedProduct: false, id: 1, img: './assets/media/products/1.png', category: 'ROMPERS', createdDate: '2012/11/10', style: 'j11215', stock: '100', qty: '', color: 'Blue', price: 11 },
    { selectedProduct: false, id: 2, img: './assets/media/products/2.png', category: 'ROMPERS', createdDate: '2015/11/11', style: 'j11211', stock: '100', qty: '', color: 'Blue', price: 12 },
    { selectedProduct: false, id: 3, img: './assets/media/products/3.png', category: 'ROMPERS', createdDate: '2017/11/12', style: 'j11212', stock: '100', qty: '', color: 'Blue', price: 13 },
    { selectedProduct: false, id: 4, img: './assets/media/products/4.png', category: 'ROMPERS', createdDate: '2010/11/13', style: 'j11223', stock: '100', qty: '', color: 'Blue', price: 14 },
    { selectedProduct: false, id: 5, img: './assets/media/products/5.png', category: 'ROMPERS', createdDate: '2013/11/14', style: 'j11234', stock: '100', qty: '', color: 'Blue', price: 15 },
    { selectedProduct: false, id: 6, img: './assets/media/products/6.png', category: 'ROMPERS', createdDate: '2014/11/15', style: 'j11234', stock: '100', qty: '', color: 'Blue', price: 16 },
    { selectedProduct: false, id: 7, img: './assets/media/products/7.png', category: 'ROMPERS', createdDate: '2017/11/16', style: 'j11224', stock: '100', qty: '', color: 'Blue', price: 17 },
    { selectedProduct: false, id: 8, img: './assets/media/products/8.png', category: 'ROMPERS', createdDate: '2019/11/17', style: 'j11234', stock: '100', qty: '', color: 'Blue', price: 18 },
    { selectedProduct: false, id: 9, img: './assets/media/products/9.png', category: 'ROMPERS', createdDate: '2021/11/01', style: 'j11245', stock: '100', qty: '', color: 'Blue', price: 19 },
    { selectedProduct: false, id: 10, img: './assets/media/products/10.png', category: 'ROMPERS', createdDate: '2020/11/19', style: 'j11232', stock: '100', qty: '', color: 'Blue', price: 20 },
  ];
  dresses = [
    { selectedProduct: false, id: 11, img: './assets/media/products/11.png', category: 'DRESSES', createdDate: '2012/11/10', style: 'j11215', stock: '100', qty: '', color: 'Blue', price: 21 },
    { selectedProduct: false, id: 12, img: './assets/media/products/12.png', category: 'DRESSES', createdDate: '2015/11/11', style: 'j11211', stock: '100', qty: '', color: 'Blue', price: 22 }
  ];
  categories: string;
  productList: any;

  //#endregion

  //#region Constcructor

  constructor(private modelservice: NgbActiveModal) { }

  //#endregion

  //#region Methods

  /**
  * NgOnInit
  * angular lifecircle hooks
  */

  ngOnInit(): void {
    this.data = [
      { 'id': 1, first_name: 'TestBed.png', last_name: 'TestBed.png', email: 'test@test.com', gender: 'male', status: 'active' },
      { 'id': 2, first_name: 'TestBed.png', last_name: 'TestBed.png', email: 'test@test.com', gender: 'male', status: 'active' },
      { 'id': 3, first_name: 'TestBed.png', last_name: 'TestBed.png', email: 'test@test.com', gender: 'male', status: 'active' },
      { 'id': 4, first_name: 'TestBed.png', last_name: 'TestBed.png', email: 'test@test.com', gender: 'male', status: 'active' },
      { 'id': 5, first_name: 'TestBed.png', last_name: 'TestBed.png', email: 'test@test.com', gender: 'male', status: 'active' },
      { 'id': 6, first_name: 'TestBed.png', last_name: 'TestBed.png', email: 'test@test.com', gender: 'male', status: 'active' },
      { 'id': 7, first_name: 'TestBed.png', last_name: 'TestBed.png', email: 'test@test.com', gender: 'male', status: 'active' },
      { 'id': 8, first_name: 'TestBed.png', last_name: 'TestBed.png', email: 'test@test.com', gender: 'male', status: 'active' },
      { 'id': 9, first_name: 'TestBed.png', last_name: 'TestBed.png', email: 'test@test.com', gender: 'male', status: 'active' },
      { 'id': 10, first_name: 'TestBed.png', last_name: 'TestBed.png', email: 'test@test.com', gender: 'male', status: 'active' },
      { 'id': 11, first_name: 'TestBed.png', last_name: 'TestBed.png', email: 'test@test.com', gender: 'male', status: 'active' }
    ];

    for (let key in this.data) {
      this.uservals.push({ id: this.data[key]['id'], itemName: this.data[key]['first_name'] });
    }
    this.style = [
      { id: 1, itemName: 'Style1' },
      { id: 2, itemName: 'Style2' },
      { id: 3, itemName: 'Style3' },
      { id: 4, itemName: 'Style4' },
      { id: 5, itemName: 'Style5' },
    ];

    this.singleSelectsettings = {
      singleSelection: true,
      enableSearchFilter: true,
    }

    this.productList = this.rompres.concat(this.dresses);
  }

  /**
 *onItemSelect 
 @param item (any)
 */

  onItemSelect(item: any) {
    console.log('onItemSelect', item);
  }

  //#end onItemSelect 

  /**
*onItemSelect 
@param item (any)
*/
  onSelectAll(items: any) {
    console.log('onSelectAll', items);
  }

  //#end onItemSelect 

  /**
*cancel button 
*/

  cancel() {
    this.modelservice.dismiss(ProductListComponent);
  }

  //#end cancelbutton 

  /**
*onUserSelect 
@param id (number)
*/

  onUserSelect(id) {
    let result = this.data.find(res => res.id === id.id);
    this.selectedUserAddress = result.address;
    this.selectedUserName = result.first_name;
    this.selectedUserEmail = result.email;
  }

  //#end onUserSelect 

  /**
*changeValueEvent 
*/

  changeValueEvent() {
    console.log("*********", this.productList);
  }

  //#end changeValueEvent 

  /**
*getAllProduct 
*/

  getAllProduct() {
    console.log("-------", this.categories);
    if (this.categories == 'rompers') {
      this.productList = this.rompres;
    } else if (this.categories == 'dresses') {
      this.productList = this.dresses;
    } else {
      this.productList = this.rompres.concat(this.dresses);
    }
  }

  //#end getAllProduct 


}
