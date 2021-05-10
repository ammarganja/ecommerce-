import { Injectable, OnDestroy, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, Observable } from 'rxjs';
import { exhaustMap, map } from 'rxjs/operators';
import { TableService, TableResponseModel, ITableState, BaseModel, PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
import { environment } from 'src/environments/environment';
import { Product } from '../_models/product.model';
import { baseFilter } from '../helpers/http-extensions';
import { BaseService } from 'src/app/core/services/base.service';

const DEFAULT_STATE: ITableState = {
  filter: {},
  paginator: new PaginatorState(),
  sorting: new SortState(),
  searchTerm: '',
  grouping: new GroupingState(),
  entityId: undefined
};


@Injectable({
  providedIn: 'root'
})
export class ProductsService extends TableService<Product> implements OnDestroy {
  API_URL = `${environment.apiUrl}/admin/Product/`;
  API_URL_FILTER = `${environment.apiUrl}/admin/`;
  constructor(@Inject(HttpClient) http, private baseService: BaseService) {
    super(http);
  }

  // READ
  find(tableState: ITableState): Observable<TableResponseModel<Product>> {
    var params = {
      searchString: tableState.filter?.searchString,
      ColorId: tableState.filter?.ColorId,
      CategoryTypeId: tableState.filter?.CategoryTypeId,
      pageNo: tableState.paginator.page,
      limit: tableState.paginator.pageSize,
      column:tableState.sorting.column,
      direction: tableState.sorting.direction,
      SizeId: tableState.filter?.SizeId,
    }

    let headers = this.baseService.getHeaders(true);

    return this.http.post<any[]>(this.API_URL+'product-list',params,{headers : headers}).pipe(
      map((response: any) => {
        const result: TableResponseModel<Product> = {
          items: response.data.items,
          total: response.data.total
        };
        return result;
      })
    );
  }

  deleteItems(ids: number[] = []): Observable<any> {
    const tasks$ = [];
    ids.forEach(id => {
      tasks$.push(this.delete(id));
    });
    return forkJoin(tasks$);
  }

  updateStatusForItems(ids: number[], status: number): Observable<any> {
    return this.http.get<Product[]>(this.API_URL).pipe(
      map((products: Product[]) => {
        return products.filter(c => ids.indexOf(c.id) > -1).map(c => {
          c.status = status;
          return c;
        });
      }),
      exhaustMap((products: Product[]) => {
        const tasks$ = [];
        products.forEach(product => {
          tasks$.push(this.update(product));
        });
        return forkJoin(tasks$);
      })
    );
  }

  changeProductStatus(productId:number,status:number) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'change-product-status', { productId: productId, Status:status}, { headers: headers })
      .pipe(map(res => res));
  }

  delteProduct(productId:string,updatedBy:string) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'delete-product', { productId: productId, updatedBy:updatedBy}, { headers: headers })
      .pipe(map(res => res));
  }

  detailProduct(productId:string) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'product-detail', { productId: productId}, { headers: headers })
      .pipe(map(res => res));
  }

  categoryList() {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL_FILTER + 'Master/get-product-category', {},{ headers: headers })
      .pipe(map(res => res));
  }

  getSizeGroupList() {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL_FILTER + 'Master/get-size-list ', {},{ headers: headers })
      .pipe(map(res => res));
  }

  getSizeUnitList(groupId) {
    var params = {
      sizeId: groupId,
    }
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL_FILTER + 'Master/get-size-detail-list', params,{ headers: headers })
      .pipe(map(res => res));
  }

  addProduct(formData:any) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'add-product', formData,{ headers: headers })
      .pipe(map(res => res));
  }

  addProductColor(productColorImage:any) {
    let headers = this.baseService.getHeadersForImage(true);
    const formData = new FormData();

    formData.append('productId', productColorImage.productId);
    formData.append('ColorId', productColorImage.ColorId);
    formData.append('DeleteImageId',productColorImage.DeleteImageId);
    if(productColorImage && productColorImage.image){
      productColorImage.image.forEach(img => {
        if(img.name){
          formData.append('image', img);
          formData.append('file[]', img);
        }
      });
    }    
    return this.http
      .post(this.API_URL + 'add-product-color', formData,{ headers: headers })
      .pipe(map(res => res));
  }

  removeProductColor(removeColor) {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'delete-product-color', removeColor,{ headers: headers })
      .pipe(map(res => res));
  }


  getProductDetails(productId:number) {
    let headers = this.baseService.getHeaders(true);
    var params = {
      productId: productId,
    }
    return this.http
      .post(this.API_URL + 'product-detail ', params,{ headers: headers })
      .pipe(map(res => res));
  }

  colorList() {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL_FILTER + 'Master/get-color-list', {},{ headers: headers })
      .pipe(map(res => res));
  }
  removeColor(formdata){
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(this.API_URL + 'delete-product-color', formdata,{ headers: headers })
      .pipe(map(res=> res));
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sb => sb.unsubscribe());
  }
}
