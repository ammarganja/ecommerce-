import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { from, Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { GroupingState, PaginatorState, SortState, ICreateAction, IEditAction, ISortView, IFilterView, IGroupingView, ISearchView } from '../../shared/crud-table';
import { EditProductComponent } from './edit-product/edit-product.component';
import { ProductsService } from '../services/products.service';
import { rompresData, dressesData } from './product.data';
import { ConfirmationpopupComponent } from 'src/app/core/components/confirmationpopup/confirmationpopup.component';
import { HelperService } from '../services/helper.service';
import { Columns } from '../_models/columns';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss'],
})
export class ProductListComponent
  implements
  OnInit,
  ICreateAction,
  IEditAction,
  ISortView,
  IFilterView,
  IGroupingView,
  ISearchView,
  IFilterView {
  //#region Variable Declaration

  multiSelectSettings: any;
  singleSelectsettings: any;

  paginator: PaginatorState;
  sorting: SortState;
  grouping: GroupingState;
  isLoading: boolean;
  filterGroup: FormGroup;
  searchGroup: FormGroup;
  subscriptions: Subscription[] = [];
  Columns: Columns[];
  colorList: any[];
  rompres = rompresData;
  dresses = dressesData;
  categoryList: any[];
  colorModel: any[];
  categoryModel: any[];
  title: any;
  productStatusName = 'Product';
  productList = [];
  searchString = "";
  idColor = '0';
  idCategory = "0";
  idSize = "0";
  hasQueryFilter = false;
  //#endregion

  //#region Constcructor

  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    public productService: ProductsService,
    private router: Router,
    public helperService: HelperService,
    private activatedRoute: ActivatedRoute
  ) {
    var color = this.activatedRoute.snapshot.queryParamMap.get("idColor");
    var category = this.activatedRoute.snapshot.queryParamMap.get("idCategory");
    var size = this.activatedRoute.snapshot.queryParamMap.get("idSize");

    if(color){
      this.idColor = color;
      this.hasQueryFilter = true;
    }else if(category){
      this.idCategory = category;
      this.hasQueryFilter = true;
    }else if (size){
      this.idSize = size;
      this.hasQueryFilter = true;
    }else{
      this.hasQueryFilter = false;
    }
    
  }
  //#endregion

  //#region Methods

  /**
   * NgOnInit
   */
  ngOnInit(): void {

    this.singleSelectsettings = this.helperService.singleSelectSetting();
    this.multiSelectSettings = this.helperService.MultiSelectSetting();
    this.Columns = this.helperService.getProductColumns();
    this.fetchProductData();
    this.fetchCategoryData();
    this.fetchColorData();
  }

  /**
   * Fetch Product data
  */
  fetchProductData() {
    var filter : any;
    if(+this.idColor){
      var item = {
        id: this.idColor
      }
      this.onColorSelect(item);
    }else if(+this.idCategory){
      var item = {
        id: this.idCategory
      }
      this.onCategorySelect(item);
    }else if(+this.idSize){
      filter = {
        "SizeId" : this.idSize
      }
      this.productService.patchState({ filter });
    }else{
      filter = {
        "searchString": ""
      }
      
    }
    
    this.productList = this.rompres.concat(this.dresses);
    this.productService.paginator.page = 1;
    this.productService.paginator.pageSize = 10;
    this.productService.paginator.pageSizes = [];
    this.grouping = this.productService.grouping;
    this.paginator = this.productService.paginator;
    this.sorting = this.productService.sorting;
    const sb = this.productService.isLoading$.subscribe(
      (res) => (this.isLoading = res));
    this.subscriptions.push(sb);

    this.productService.patchState({ filter });
  }

  /**
   * Fetch Category
  */
  fetchCategoryData() {
    this.productService.categoryList().subscribe((response: any) => {
      if (response.result) {
        this.categoryList = response.data;
      } else {
        Swal.fire({
          title: 'Error',
          text: response.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { });
      }
    });
  }

  /**
   * Color Data
   */
  fetchColorData() {
    this.productService.colorList().subscribe((response: any) => {
      if (response.result) {
        this.colorList = response.data;
      } else {
        Swal.fire({
          title: 'Error',
          text: response.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { });
      }
    });
  }

  /**
   * onColorSelect
   */
  onColorSelect(item: any) {
    var filter = {
      "ColorId": item.id,
      "CategoryTypeId": this.categoryModel && this.categoryModel.length > 0 ? this.categoryModel[0].id : '',
      "searchString": this.searchString
    }
    this.productService.patchState({ filter });
  }

  /**
   * OnColorDeselectAll
   */
  onColorDeselectAll() {
    this.colorModel = [];
    var filter = {
      "ColorId": "",
      "CategoryTypeId": this.categoryModel && this.categoryModel.length > 0 ? this.categoryModel[0].id : '',
      "searchString": this.searchString
    }
    this.productService.patchState({ filter });
  }
  
 /** 
  * onCategorySelect
  */
  onCategorySelect(item: any) {
    var filter = {
      "CategoryTypeId": item.id,
      "ColorId" : this.colorModel &&this.colorModel.length > 0 ? this.colorModel[0].id : '',
      "searchString": this.searchString
    }
    this.productService.patchState({ filter });
  }

  /**
   * OnCategoryRemoveAll
   */
  onCategoryDeselectAll() {
    this.categoryModel = [];
    var filter = {
      "CategoryTypeId": "",
      "ColorId" : this.colorModel &&this.colorModel.length > 0 ? this.colorModel[0].id : '',
      "searchString": this.searchString,
    }
    this.productService.patchState({ filter });
  }

  /**
 * OnCategoryDeselect
 */
  //// onCategoryDeselect($event) {
  //// }



  /**
   *FilterForm
   */
  filterForm() {
  }
  /**
   *Filter
   */
  filter() { }

  /**
   *SearchForm
   */
  searchForm() {
  }
  /**
   *Search
   @param searchTerm
   */
  search(searchTerm: string) {
    this.productService.paginator.page = 1;
    var filter : any;
    if(this.idColor != '0' || this.idCategory != '0' || this.idSize != '0'){
      filter = {
        "searchString": searchTerm,
        "ColorId": this.idColor != '0' ? this.idColor : undefined,
        "CategoryTypeId": this.idCategory != '0' ? this.idCategory : undefined,
        "SizeId": this.idSize != '0' ? this.idSize : undefined
      }
    }else{
      filter = {
        "searchString": searchTerm,
        "ColorId": this.colorModel &&this.colorModel.length > 0 ? this.colorModel[0].id : '',
        "CategoryTypeId": this.categoryModel && this.categoryModel.length > 0 ? this.categoryModel[0].id : ''
      }
    }
    
    this.productService.patchState({ filter });
  }

  /**
   *Sorting Column
   @param column
   */
  sort(column: string) {
    const sorting = this.sorting;
    const isActiveColumn = sorting.column === column;
    if (!isActiveColumn) {
      sorting.column = column;
      sorting.direction = 'asc';
    } else {
      sorting.direction = sorting.direction === 'asc' ? 'desc' : 'asc';
    }
    this.productService.patchState({ sorting });
  }

  /**
   *pagination
   @param paginator
   */
  paginate(paginator: PaginatorState) {
    this.productService.patchState({ paginator });
  }

  /**
   *FormActions Create
   */
  create() {
    this.edit(undefined);
  }

  /**
   *FormActions Edit
   @param product
   */
  edit(product: any) {
    const modalRef = this.modalService.open(EditProductComponent, {
      size: 'xl'
    });
    modalRef.componentInstance.product = product;
    modalRef.result.then(
      () => this.productService.fetch(),
      () => { }
    );
  }

  /**
   *FormActions View
   @param product
   */
  view(product: any) {
    this.router.navigate(['/product-details', product.productId]);
  }

  /**
   *FormActions delete
   @param id
   */

  delete(productId: string) {
    let modelref = this.modalService.open(ConfirmationpopupComponent);
    modelref.result.then((res) => {
      if (res === 'yes') {
        let updatedBy = "2";
        this.productService.delteProduct(productId, updatedBy).subscribe((response: any) => {
          if (response.result) {
            this.fetchProductData();
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
    });
  }

  /**
   *Product status
   @param Id
   @param status
   @param oldstatus 
   */

  productstatus(id: number, status: number, oldstatus: number) {
    if (status != oldstatus) {
      let modelref = this.modalService.open(ConfirmationpopupComponent);
      modelref.result.then((res) => {
        if (res === 'yes') {
          this.productService.changeProductStatus(id, status).subscribe((response: any) => {
            if (response.result) {
              this.fetchProductData();
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
      });
    }
  }

  backToProduct() {
    this.router.navigate(['/products']);
    setTimeout(() => {
      window.location.reload();  
    }, 10);
    
  }

  //#endregion
}
