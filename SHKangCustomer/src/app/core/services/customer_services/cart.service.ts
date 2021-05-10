import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { ResponseModel } from "../../models/response.model";
import { BaseService } from "../../utils/base.service";

@Injectable({
    providedIn: 'root'
})
export class CartService {
    items : any;
    private readonly API_URL: string = environment.apiUrl;
    
    constructor(private http: HttpClient,private baseService: BaseService) { }

    //#region functions
    /**
     * getCampaignList
     * @param requestModel 
     * @returns 
     */
    addToCart(requestModel: any): Observable<ResponseModel> {
        let headers = this.baseService.getHeaders(true);
        return this.http.post<any>(this.API_URL + 'UserCart/add-to-cart', requestModel, { headers: headers }).pipe(
            map((response: any) => {
                return response;
            })
        );
    }

    getCartCount(requestModel: any): Observable<ResponseModel> {
        let headers = this.baseService.getHeaders(true);
        return this.http.post<any>(this.API_URL + 'UserCart/get-cart-count', requestModel, { headers: headers }).pipe(
            map((response: any) => {
                return response;
            })
        );
    }

    addToProduct(product:any) {
        this.items=product;
        // console.log(this.items)
      }
    
      getItems() {
        return this.items;
      }
    
      clearCart() {
        this.items = [];
        return this.items;
      }

    //#endregion
}