import { HttpClient } from '@angular/common/http';
import {
  ChangeDetectorRef,
  Component,
  Input,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { DeleteconfirmationComponent } from 'src/app/core/components/deleteconfirmation/deleteconfirmation.component';
import {
  IDeleteAction,
  IDeleteSelectedAction,
  IFetchSelectedAction,
  IUpdateStatusForSelectedAction,
  ISortView,
  IFilterView,
  IGroupingView,
  ISearchView,
  PaginatorState,
  SortState,
  GroupingState,
} from 'src/app/shared/crud-table';
import { CampaignProductService } from '../../services/campaign.product';
import { HelperService } from '../../services/helper.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-edit-campaign',
  templateUrl: './edit-campaign.component.html',
  styleUrls: ['./edit-campaign.component.scss'],
})
export class EditCampaignComponent

  //#region Variable Declaration

  implements
  OnInit,
  OnDestroy,
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
  @Input() selectData;
  @Input() campaign: any;
  StyleSelected = [];
  Columns: any[];
  searchString : "";
  campaignFormObj : any = {

  };
  productList : any;
  categories: string;
  files: File[] = [];
  showDropzone = false;
  isDeleteImage = false;
  selectedProducts = [];
  showInFrontEnd = false
  //#endregion

  //#region Constcructor 

  constructor(
    private modal: NgbActiveModal,
    private changeDetecetionRef: ChangeDetectorRef,
    public campaignProductService : CampaignProductService,
    public helperService: HelperService,
    private http: HttpClient,
    private modalService: NgbModal
  ) { }

  //#endregion

  //#region Methods

  /**
*onSelect event 
@param event (onSelect)
*/

  onSelect(event) {
    this.files.length = 0;
    this.files.push(...event.addedFiles);
  }

  //#endonSelect

  /**
*onRemove event 
@param event (onRemove)
*/

  onRemove(event) {
    console.log(event);
    this.files.splice(this.files.indexOf(event), 1);
  }

  //#endonRemove

  /**
*ngOnDestroy  
*/

  ngOnDestroy() {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }

  //#endngOnDestroy

  /**
*filterForm  
*/

  filterForm() { }

  //#endfilterForm

  /**
*submit button  
*/

  submit() {
    var user = this.helperService.getUserDetail();
    var params = {
      CampaignId : this.campaignFormObj.campaignId ? this.campaignFormObj.campaignId : 0,
      CampaignName : this.campaignFormObj.campaignName,
      Description : this.campaignFormObj.description,
      Image : this.files && this.files[0] ? this.files[0] : null,
      ProductId : this.campaign && this.campaign.productIds ? this.campaign.productIds.join(',') : this.selectedProducts.join(','),
      UpdatedBy : user.userId,
      IsImageDeleted : this.isDeleteImage && this.files.length == 0 ? true : false,
      IsShowinFrontend : this.showInFrontEnd ? this.showInFrontEnd : false
    }
    if(!params.ProductId){
      Swal.fire({
        title: 'Error',
        text: 'Please select at least one Product',
        icon: 'error',
        showCancelButton: false,
        confirmButtonText: 'Ok'
      }).then((result) => { 
      
      })
      return false;
    }
    this.campaignProductService.createItem(params).subscribe((response: any) => {
      if (response.result) {
        this.modal.close();
      }else{
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

  //#endsubmit button

  /**
*cancel button  
*/
  cancel() {
    this.modal.close();
  }

  //#endcancel button 

  /**
*deleteSelected 
*/

  deleteSelected() {
  }

  //#enddeleteSelected

  /**
*fetchSelected 
*/

  fetchSelected() {
  }

  //#endfetchSelected

  /**
*updateStatusForSelected 
*/
  updateStatusForSelected() {
  }

  //#endupdateStatusForSelected

  /**
* NgOnInit
* angular lifecircle hooks
*/

  ngOnInit(): void {
    if(this.campaign){
      this.campaignFormObj = this.campaign;
    }
    this.getCampignProducts(0);
  }

  //#endNgOnInit

  /**
*changeValue event 
@param index (product)
@param selectedVal (selectedVal)
*/

  changeValueEvent(index, selectedVal) {
    if (this.campaign && this.campaign.productIds) {
      if (selectedVal) {
        this.campaign.productIds.push(index)
      } else {
        var ind = this.campaign.productIds.findIndex(a => a == index);
        if (ind > -1) {
          this.campaign.productIds.splice(ind, 1);
        }
      }
    } else {
      if (selectedVal) {
        this.selectedProducts.push(index)
      } else {
        var sInd = this.selectedProducts.findIndex(a => a == index);
        if (sInd > -1) {
          this.selectedProducts.splice(sInd, 1);
        }
      }
    }
  }

  //#end changeValue

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
    this.campaignProductService.patchState({ filter });
  }

  //#end filter

  /**
*search 
*/

  searchForm() { }

  search(searchTerm: string) {
    this.campaignProductService.paginator.page = 1;
    var filter = {
      "searchString": searchTerm,
      "ProductCategoryTypeId": 0
    }
    this.campaignProductService.patchState({ filter });
  }

  //#end search

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
    this.campaignProductService.patchState({ sorting });
  }

  //#end sorting

  /**
*pagination 
@param paginator (PaginatorState)
*/

  paginate(paginator: PaginatorState) {
    this.campaignProductService.patchState({ paginator });
  }

  //#end pagination

  /**
*changeRatio 
@param order (any)
*/
  changeRatio(order: any) { }

  //#end changeRatio

  /**
*delete 
@param id (number)
*/

  delete(id: number) { }

  //#end delete

  /**
   * get Campign Products
   * @param id 
   */
  getCampignProducts(id) {
    var filter = {
      "searchString": '',
      "ProductCategoryTypeId": this.campaign ? this.campaign.campaignId : id
    }
    this.campaignProductService.patchState({ filter });
    // this.filterForm();
    this.campaignProductService.paginator.page = 1;
    this.campaignProductService.paginator.pageSize = 10;
    this.campaignProductService.paginator.pageSizes = [];
    this.grouping = this.campaignProductService.grouping;
    this.paginator = this.campaignProductService.paginator;
    this.sorting = this.campaignProductService.sorting;
    const sb = this.campaignProductService.isLoading$.subscribe(res => this.isLoading = res);
    this.subscriptions.push(sb);
    this.Columns = this.helperService.getCampaignsProductColumns();
    this.productList = this.campaignProductService.items$;
  }

  //#end getCampignProducts

  /**
*onSelectAll 
@param event (onSelectAll)
*/

  onSelectAll(event) { }

  //#end onSelectAll

  /**
*onItemSelect 
@param event (onItemSelect)
*/

  onItemSelect(event) { }
  
  editImage(){
    this.showDropzone = true;
  }

  deleteImage(id: number) {
    const modalRef = this.modalService.open(DeleteconfirmationComponent);
    let data = {
      id: id,
      key: 'Image',
    }
    modalRef.componentInstance.data = data;
    modalRef.result.then((res) => {
      if (res === 'yes') {
        this.campaign.image = '';
        this.showDropzone = true;
        this.isDeleteImage = true;
      }
    });
  }

  showInFront($event){
    if($event && $event.target.checked){
      this.showInFrontEnd = true;
    }else{
      this.showInFrontEnd = false;
    }
  }

  //#end onItemSelect
}
