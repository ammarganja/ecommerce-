import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CampaignService } from '../../services/customer_services/campaign.service';
import { ProductService } from '../../services/customer_services/product.service';

@Component({
  selector: 'app-product-search',
  templateUrl: './product-search.component.html',
  styleUrls: ['./product-search.component.scss'],
})
export class ProductSearchComponent implements OnInit {
  list :any= [];
  items: any;
  public selectedCategories: any;
  selectedCampaigns:any
  dropdownSettings = {
    singleSelection: false,
    idField: 'id',
    textField: 'itemName',
    itemsShowLimit: 5,
    allowSearchFilter: true,
    enableCheckAll	: false
  };
  params = {
    "pageNo": 1,
    "limit": 0,
    "searchString": "",
    "column": "",
    "direction": "",
    "Status": 0
  }
  @Output() productSearchText: EventEmitter<any> = new EventEmitter();
  @Output() productFilterText: EventEmitter<any> = new EventEmitter();
  @Output() productCategortSearch: EventEmitter<any> = new EventEmitter();
  routeSegment: any;
  styleList: any;
  selectedCampaignList:any
  CampaignList: any = [];
  categoryFilterList: any;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productservice: ProductService,
    private campaign: CampaignService,
  ) {
    // this.list = [
    //   { name: 'India', checked: false },
    //   { name: 'US', checked: false },
    //   { name: 'China', checked: false },
    //   { name: 'France', checked: false },
    // ];
  }

  ngOnInit(): void {
    this.router.events.subscribe((url: any) => console.log(url));
    let routeData = this.router.url.split('/');
    this.routeSegment = routeData[2];
    if(this.routeSegment == 'campaign'){
      this.campaign.getCampaignList(this.params).subscribe((res:any)=>{
        if(res.result){
          this.CampaignList = []; 
          res.data.items.forEach((element:any) => {
            let data={
              id:element.campaignId,
              itemName:element.campaignName
            } 
            this.CampaignList.push(data);
           });
        }
      });
    }else{
      this.productservice.getCategoryList().subscribe((res:any)=>{
        if(res.result){
          this.styleList = res.data
        }
      });
    }
  }

  shareCheckedList(item: any) {
    this.categoryFilterList=[];
    this.selectedCategories.forEach((element:any) => {
      this.categoryFilterList.push(element.id)
    });
    this.productCategortSearch.emit( this.categoryFilterList.join(','))
  }

  shareIndividualCheckedList(item: any) {
    this.categoryFilterList = [];
    this.selectedCategories.forEach((element:any) => {
      this.categoryFilterList.push(element.id)
    });
    this.productCategortSearch.emit( this.categoryFilterList.join(','))
  }
  shareCheckedListcampaign(item: any) {
    this.categoryFilterList=[];
    this.selectedCampaignList.forEach((element:any) => {
      this.categoryFilterList.push(element.id)
    });
    this.productCategortSearch.emit( this.categoryFilterList.join(','))
  }

  shareIndividualCheckedListCampaign(item: any) {
    this.categoryFilterList = [];
    this.selectedCampaignList.forEach((element:any) => {
      this.categoryFilterList.push(element.id)
    });
    this.productCategortSearch.emit( this.categoryFilterList.join(','))
  }

  onSearchProducts($event: any) {
    this.productSearchText.emit($event.target.value);
  }

  onFilterProduct(value: any) {
    this.productFilterText.emit(value);
  }
  
}
