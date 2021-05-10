import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CampaignService } from 'src/app/core/services/customer_services/campaign.service';
import { ProductService } from 'src/app/core/services/customer_services/product.service';

@Component({
  selector: 'app-campaign-detail',
  templateUrl: './campaign-detail.component.html',
  styleUrls: ['./campaign-detail.component.scss'],
})
export class CampaignDetailComponent implements OnInit {
  campaignId: any;

  public requestParam = {
    searchString: '',
    column: '',
    pageNo: 1,
    productCategoryTypeId: '',
    limit: 10,
    direction: '',
    CategoryTypeId:''
  };
  public productItems: any = [];
  total = 0;
  throttle = 300;
  scrollDistance = 1;
  scrollUpDistance = 2;
  campaignModel: any;

  constructor(
    private router: ActivatedRoute,
    private campaignservice: CampaignService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    if (this.campaignId == undefined) {
      this.campaignId = this.router.snapshot.params.campignId;
    }
    this.requestParam.productCategoryTypeId = this.campaignId;
    if(this.campaignId){
      this.campaignservice.getCampaignDetails(this.campaignId).subscribe((response:any)=>{
        if(response.result){
          this.campaignModel  = response.data;
        }
      })
    }
    this.loadProductItems();
  }

  loadProductItems(isLoadMore: any = false) {
    if (isLoadMore) {
      this.requestParam.pageNo = this.requestParam.pageNo + 1;
    }
    if (this.productItems.length != this.total || this.total == 0) {
      this.campaignservice
        .getCampaignProducts(this.requestParam)
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
  getProductSearchText(text: any) {
    this.productItems = [];
    this.requestParam.searchString = text;
    this.requestParam.pageNo = 1;
    this.loadProductItems();
  }
  getProductFilterText(text: any) {
    this.productItems = [];
    this.requestParam.column = text;
    this.requestParam.pageNo = 1;
    this.loadProductItems();
  }
  getCategorySearch(event:any){
    this.productItems = [];
    this.requestParam.productCategoryTypeId = event;
    this.requestParam.pageNo = 1;
    this.loadProductItems();
  }
}
