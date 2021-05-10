import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { CampaignModel } from '../../models/campaign.model';
import { ProductDetail } from '../../models/product-detail.model';
import { ResponseModel } from '../../models/response.model';
import { BaseService } from '../../utils/base.service';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private readonly API_URL: string = environment.apiUrl;

  constructor(private http: HttpClient, private baseService: BaseService) { }

  //#region functions
  /**
   * getProductList
   * @param requestModel
   * @returns
   */
  getProductList(search:any) {
    var params = {
      searchString: search?search:'',
      ColorId: '',
      CategoryTypeId: '',
      pageNo: 1,
      limit: 5,
      column: '',
      direction: '',
    };

    let headers = this.baseService.getHeaders(true);
    return this.http
      .post<ProductDetail>(
        this.API_URL + 'Tokenless/get-style-product-list',
        params,
        { headers: headers }
      )
      .pipe(
        map((response: any) => {
          return response;
        })
      );
  }

  getCategoryList(){
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post(
        this.API_URL + 'Common/get-product-category',
        {},
        { headers: headers }
      )
      .pipe(
        map((response: any) => {
          return response;
        })
      );
  }

  /**
   * getCampaignList
   * @param requestModel
   * @returns
   */
  getProductDetail(requestModel: any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http
      .post<ProductDetail>(
        this.API_URL + 'Product/get-product-detail',
        requestModel,
        { headers: headers }
      )
      .pipe(
        map((response: any) => {
          return response;
        })
      );
  }


  getProductItems(requestModel: any): Observable<ResponseModel> {
    let headers = this.baseService.getHeaders(true);
    return this.http.post<ProductDetail>(this.API_URL + 'Product/get-style-product-list', requestModel, { headers: headers }).pipe(
      map((response: any) => {
        return response;
      })
    );
  }
  //#endregion
}
