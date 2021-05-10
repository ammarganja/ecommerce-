import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CampaignService } from 'src/app/core/services/customer_services/campaign.service';

@Component({
  selector: 'app-campaign',
  templateUrl: './campaign.component.html',
  styleUrls: ['./campaign.component.scss']
})
export class CampaignComponent implements OnInit {

  constructor(private campaignService: CampaignService,private toastr: ToastrService) { }

  //#region variable declaration
  throttle = 300;
  scrollDistance = 1;
  scrollUpDistance = 2;
  params = {
    "pageNo": 1,
    "limit": 5,
    "searchString": "",
    "column": "",
    "direction": "",
    "Status": 0
  }

  items : any = [];
  total : any = 0
  //#endregion

  ngOnInit(): void {
    this.getCampainList();
  }

  /**
   * get campaign list
   */
  getCampainList(isEvent: any = false) {
    if(isEvent){
      this.params.pageNo = this.params.pageNo + 1;
    }
    if((this.items.length != this.total) || this.total == 0){
      this.campaignService.getCampaignList(this.params).subscribe((response: any) => {
        if(response.result && response.status == 200){
          response.data.items.forEach((element:any) => {
            this.items.push(element)
          });
          this.total = response.data.total;
        }else{
          this.toastr.error(response.message);
          return;
        }
      });
    }
  }
}
