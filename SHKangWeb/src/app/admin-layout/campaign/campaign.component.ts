import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ISortView, IFilterView, IGroupingView, ISearchView, PaginatorState, SortState, GroupingState } from 'src/app/shared/crud-table';
import { ICreateAction, IDeleteAction, IDeleteSelectedAction, IEditAction, IFetchSelectedAction, IUpdateStatusForSelectedAction } from 'src/app/shared/crud-table/models/table.model';
import { campaignService } from '../services/campaign.services';
import { EditCampaignComponent } from './edit-campaign/edit-campaign.component';
import { campaignData } from "./campaign.data";
import { DeleteconfirmationComponent } from 'src/app/core/components/deleteconfirmation/deleteconfirmation.component';
import { HelperService } from '../services/helper.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { Router } from '@angular/router';
@Component({
  selector: 'app-campaign',
  templateUrl: './campaign.component.html',
  styleUrls: ['./campaign.component.scss']
})
export class CampaignComponent implements

  //#region Variable Declaration

  OnInit,
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
  private subscriptions: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/
  Columns: any[];
  campaignList = [];
  searchString = "";
  deletestatus: any;
  // rows = [];

  //#endregion

  //#region Constcructor

  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    public campaignService: campaignService,
    public helperService: HelperService,
    private router: Router
  ) { }

  //#endregion

  //#region Methods

  /**
  * NgOnInit
  * angular lifecircle hooks
  */

  ngOnInit(): void {
    var filter = {
      "searchString": ''
    }
    // this.filterForm();
    this.campaignService.paginator.page = 1;
    this.campaignService.paginator.pageSize = 10;
    this.campaignService.paginator.pageSizes = [];
    this.grouping = this.campaignService.grouping;
    this.paginator = this.campaignService.paginator;
    this.sorting = this.campaignService.sorting;
    const sb = this.campaignService.isLoading$.subscribe(res => this.isLoading = res);
    this.subscriptions.push(sb);

    this.Columns = this.helperService.getCampaignsColumns();

    this.campaignService.patchState({ filter });
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }


  /**
  *filtration form 
  */

  filterForm() {
    this.filterGroup = this.fb.group({
      status: [''],
      type: [''],
      searchTerm: [''],
    });
    this.subscriptions.push(
      this.filterGroup.controls.status.valueChanges.subscribe(() =>
        this.filter()
      )
    );
    this.subscriptions.push(
      this.filterGroup.controls.type.valueChanges.subscribe(() => this.filter())
    );
  }

  //#endfiltration

  /**
  *filter 
  */

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
    this.campaignService.patchState({ filter });
  }

  //#endfilter

  /**
  *search 
  */

  searchForm() {
    this.searchGroup = this.fb.group({
      searchTerm: [''],
    });
    const searchEvent = this.searchGroup.controls.searchTerm.valueChanges
      .pipe(debounceTime(150), distinctUntilChanged())
      .subscribe((val) => this.search(val));
    this.subscriptions.push(searchEvent);
  }

  search(searchTerm: string) {
    this.campaignService.paginator.page = 1;
    var filter = {
      "searchString": searchTerm
    }
    this.campaignService.patchState({ filter });
  }

  //#endsearch

  /**
  *sorting 
  @param column (string)
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
    this.campaignService.patchState({ sorting });
  }

  //#endsorting

  /**
 *pagination 
 @param paginator (PaginatorState)
 */

  paginate(paginator: PaginatorState) {
    this.campaignService.patchState({ paginator });
  }

  //#endsorting

  /**
 *form actions 
 */

  create() {
    this.edit(undefined);
  }

  //#end formactions

  /**
*EditCampaign 
@param campaign (any)
*/

  edit(campaign: any) {
    // const modalRef = this.modalService.open(EditCampaignComponent, { size: 'xl' });
    // modalRef.componentInstance.selectData = campaign;
    // modalRef.result.then(() =>
    //   () => { }
    // );
    if (campaign) {
      var selectedCampaign = {
        campaignId: campaign.campaignId,
        campaignName: campaign.campaignName,
        totalProduct: campaign.totalProduct,
        image: campaign.image,
        description: campaign.description,
        productIds : campaign.productIds,
        isShowinFrontend : campaign.isShowinFrontend
      }
    }

    const modalRef = this.modalService.open(EditCampaignComponent, { size: 'xl' });
    modalRef.componentInstance.campaign = selectedCampaign;
    modalRef.result.then((res) => {
      this.campaignService.fetch();
    });
  }

  //#end EditCampaign

  /**
*Delete popup 
@param id (number)
*/

  delete(id: number) {
    const modalRef = this.modalService.open(DeleteconfirmationComponent);
    let data = {
      id: id,
      key: 'compaign',
    }
    modalRef.componentInstance.data = data;
    modalRef.result.then((res) => {
      if (res === 'yes') {
        var user = this.helperService.getUserDetail();
        var params = {
          CampaignId: id,
          UpdatedBy: user.userId
        }
        this.campaignService.deletecampaign(params).subscribe((response: any) => {
          if (response.result) {
            this.campaignService.fetch();
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

  redirectProduct(campaign: any) {
    this.router.navigate(['/products'], { queryParams: { idCampaign: campaign.campaignId } })
  }

  //#end Deletepopup 


  /**
*deleteSelected 
*/

  deleteSelected() {
  }

  //#end deleteSelected 

  /**
*updateStatusForSelected 
*/

  updateStatusForSelected() {
  }

  //#end updateStatusForSelected 

  /**
*fetchSelected 
*/

  fetchSelected() {
  }

  //#end fetchSelected 
}
