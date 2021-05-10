import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { CampaignModel } from "../../models/campaign.model";
import { ResponseModel } from "../../models/response.model";
import { BaseService } from "../../utils/base.service";

@Injectable({
    providedIn: 'root'
})
export class CampaignService {
    private readonly API_URL: string = environment.apiUrl;
    
    constructor(private http: HttpClient,private baseService: BaseService) { }

    //#region functions
    /**
     * getCampaignList
     * @param requestModel 
     * @returns 
     */
    getCampaignList(requestModel: any): Observable<ResponseModel> {
        let headers = this.baseService.getHeaders(true);
        return this.http.post<CampaignModel[]>(this.API_URL + 'Common/get-campaign-list', requestModel, { headers: headers }).pipe(
            map((response: any) => {
                return response;
            })
        );
    }
    /**
     * getTokenlessCampaignList
     * @param requestModel 
     * @returns 
     */
     getTokenlessCampaignList(requestModel: any): Observable<ResponseModel> {
        let headers = this.baseService.getHeaders(true);
        return this.http.post<CampaignModel[]>(this.API_URL + 'Tokenless/get-campaign-list', requestModel, { headers: headers }).pipe(
            map((response: any) => {
                return response;
            })
        );
    }

    getCampaignProducts(requestModel:any){
        let headers = this.baseService.getHeaders(true);
        return this.http.post<CampaignModel[]>(this.API_URL + 'Product/get-campaign-product-list', requestModel, { headers: headers }).pipe(
            map((response: any) => {
                return response;
            })
        );
    }
    getCampaignDetails(requestModel:number){
        let headers = this.baseService.getHeaders(true);
        return this.http.post<CampaignModel[]>(this.API_URL + 'Product/get-campaign-detail?campaignId='+requestModel, {}, { headers: headers }).pipe(
            map((response: any) => {
                return response;
            })
        );
    }
  


    //#endregion
}