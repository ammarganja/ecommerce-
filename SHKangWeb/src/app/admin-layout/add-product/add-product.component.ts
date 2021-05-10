import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MatTableDataSource } from '@angular/material/table';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import {
  GroupingState,
  PaginatorState,
  SortState,
  ICreateAction,
  IEditAction,
  IDeleteAction,
  IDeleteSelectedAction,
  IFetchSelectedAction,
  IUpdateStatusForSelectedAction,
  ISortView,
  IFilterView,
  IGroupingView,
  ISearchView,
} from '../../shared/crud-table';
import { ProductsService } from '../services/products.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent implements OnInit,
OnDestroy,
ICreateAction,
IEditAction,
IDeleteAction,
IDeleteSelectedAction,
IFetchSelectedAction,
IUpdateStatusForSelectedAction,
ISortView,
IFilterView,
IGroupingView,
ISearchView,
IFilterView {

  paginator: PaginatorState;
  sorting: SortState;
  grouping: GroupingState;
  isLoading: boolean;
  filterGroup: FormGroup;
  searchGroup: FormGroup;
  private subscriptions: Subscription[] = [];

  @Input() order : any;

  rompres = [
    {
      selectedProduct: false,
      id: 1,
      img: '/assets/media/products/image1.jpg',
      category: 'ROMPERS',
      size_ratio:'S/M/L - 2/2/2',
      createdDate: '2012/11/10',
      style: 'j11215',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 11,
      hasError:false
    },
    {
      selectedProduct: false,
      id: 2,
      img: '/assets/media/products/image2.jpg',
      category: 'ROMPERS',
      size_ratio:'S/M/L - 2/2/2',
      createdDate: '2015/11/11',
      style: 'j11211',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 12,
      hasError:false
    },
    {
      selectedProduct: false,
      id: 3,
      img: '/assets/media/products/image3.jpg',
      category: 'ROMPERS',
      size_ratio:'S/M/L - 2/2/1',
      createdDate: '2017/11/12',
      style: 'j11212',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 13,
      hasError:false
    },
    {
      selectedProduct: false,
      id: 4,
      img: '/assets/media/products/image4.jpg',
      category: 'ROMPERS',
      size_ratio:'S/M/L - 2/2/2',
      createdDate: '2010/11/13',
      style: 'j11223',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 14,
      hasError:false
    },
    {
      selectedProduct: false,
      id: 5,
      img: '/assets/media/products/image3.jpg',
      category: 'ROMPERS',
      size_ratio:'S/M/L - 2/2/2',
      createdDate: '2013/11/14',
      style: 'j11234',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 15,
      hasError:false
    },
    {
      selectedProduct: false,
      id: 6,
      img: '/assets/media/products/image6.jpg',
      category: 'ROMPERS',
      size_ratio:'S/M/L - 2/2/2',
      createdDate: '2014/11/15',
      style: 'j11234',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 16,
      hasError:false
    },
    {
      selectedProduct: false,
      id: 7,
      img: '/assets/media/products/image7.jpg',
      category: 'ROMPERS',
      size_ratio:'S/M/L - 2/2/2',
      createdDate: '2017/11/16',
      style: 'j11224',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 17,
      hasError:false
    },
    {
      selectedProduct: false,
      id: 8,
      img: '/assets/media/products/image8.jpg',
      category: 'ROMPERS',
      size_ratio:'S/M/L - 2/1/2',
      createdDate: '2019/11/17',
      style: 'j11234',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 18,
      hasError:false
    },
    {
      selectedProduct: false,
      id: 9,
      img: '/assets/media/products/image9.jpg',
      category: 'ROMPERS',
      size_ratio:'S/M/L - 2/2/2',
      createdDate: '2021/11/01',
      style: 'j11245',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 19,
      hasError:false
    },
    {
      selectedProduct: false,
      id: 10,
      img: '/assets/media/products/image10.jpg',
      category: 'ROMPERS',
      size_ratio:'S/M/L - 2/2/2',
      createdDate: '2020/11/19',
      style: 'j11232',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 20,
      hasError:false
    },
  ];
  dresses = [
    {
      selectedProduct: false,
      id: 11,
      img: '/assets/media/products/image1.jpg',
      category: 'DRESSES',
      size_ratio:'S/M/L - 2/2/2',
      createdDate: '2012/11/10',
      style: 'j11215',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 21,
      hasError : false
    },
    {
      selectedProduct: false,
      id: 12,
      img: '/assets/media/products/image2.jpg',
      category: 'DRESSES',
      size_ratio:'S/M/L - 2/2/2',
      createdDate: '2015/11/11',
      style: 'j11211',
      stock: '100',
      qty: '',
      color: 'Blue',
      price: 22,
      hasError : false
    },
  ];

  style = [
    { id: 1, itemName: 'Style1' },
    { id: 2, itemName: 'Style2' },
    { id: 3, itemName: 'Style3' },
    { id: 4, itemName: 'Style4' },
    { id: 5, itemName: 'Style5' },
  ];

  categories: string;
  productList: any;
  
  constructor(public productsService: ProductsService,public modal: NgbActiveModal,) { }

  ngOnInit(): void {
    this.categories = 'all';
    this.productList = this.rompres.concat(this.dresses);
    if(this.order && this.order.idproducts){
      var selectedProducts = this.order.idproducts.split(',');
      if(selectedProducts && selectedProducts.length > 0){
        selectedProducts.forEach(selectedProduct => {
          this.productList.forEach(product => {
            if(product.id === +selectedProduct){
              product.selectedProduct = true;
            }
          });
        });
      }
    }
    this.productsService.paginator.page = 1;
    this.productsService.paginator.pageSize = 10;
    this.productsService.paginator.pageSizes = [];
    this.grouping = this.productsService.grouping;
    this.paginator = this.productsService.paginator;
    this.sorting = this.productsService.sorting;
  }

  changeValueEvent(index, selectedVal) {
    this.productList.forEach((product) => {
      if (product.id === +index) {
        product.selectedProduct = selectedVal;
      }else{
        product.selectedProduct = false;
      }
    });
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }

  // filtration
  filterForm() {
    
  }

  filter() {
    const filter = {};
    const status = this.filterGroup.get('status').value;
    if (status) {
      filter['status'] = status;
    }

    const type = this.filterGroup.get('type').value;
    if (type) {
      filter['type'] = type;
    }
    this.productsService.patchState({ filter });
  }

  // search
  searchForm() {
    
  }

  search(searchTerm: string) {
    this.productsService.patchState({ searchTerm });
  }

  // sorting
  sort(column: string) {
    const sorting = this.sorting;
    const isActiveColumn = sorting.column === column;
    if (!isActiveColumn) {
      sorting.column = column;
      sorting.direction = 'asc';
    } else {
      sorting.direction = sorting.direction === 'asc' ? 'desc' : 'asc';
    }
    this.productsService.patchState({ sorting });
  }

  // pagination
  paginate(paginator: PaginatorState) {
    this.productsService.patchState({ paginator });
  }

  // form actions
  create() {
    this.edit(undefined);
  }

  edit(order: any) {
    
  }

  changeRatio(order: any) {
    
  }

  delete(id: number) {
    
  }

  deleteSelected() {
    // // const modalRef = this.modalService.open(DeleteCustomersModalComponent);
    // // modalRef.componentInstance.ids = this.grouping.getSelectedRows();
    // // modalRef.result.then(() => this.productsService.fetch(), () => { });
  }

  updateStatusForSelected() {
    // // const modalRef = this.modalService.open(UpdateCustomersStatusModalComponent);
    // // modalRef.componentInstance.ids = this.grouping.getSelectedRows();
    // // modalRef.result.then(() => this.productsService.fetch(), () => { });
  }

  fetchSelected() {
    // // const modalRef = this.modalService.open(FetchCustomersModalComponent);
    // // modalRef.componentInstance.ids = this.grouping.getSelectedRows();
    // // modalRef.result.then(() => this.productsService.fetch(), () => { });
  }

  submit(){
      var selctedProducts = this.productList.filter(a=>a.selectedProduct == true);
      if(selctedProducts && selctedProducts.length > 0){
        var productWithNoQty = selctedProducts.filter(a=>a.qty === 0 || !a.qty);
        if(productWithNoQty && productWithNoQty.length > 0){
          productWithNoQty.forEach(element => {
            element.hasError = true;
          });
          return false;
        }
        this.modal.close(selctedProducts);
      }else{
        return false
      }
  }

}
