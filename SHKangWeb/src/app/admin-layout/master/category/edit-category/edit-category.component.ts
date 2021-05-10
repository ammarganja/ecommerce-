import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CategorysService } from 'src/app/admin-layout/services/category.services';
import { HelperService } from 'src/app/admin-layout/services/helper.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.scss']
})
export class EditCategoryComponent implements OnInit {

    //#region Variable Declaration
    
  @Input() category: any;
  public modelType: any;
  categoryFormObj = {
    CategoryType: null,
    ProductCategoryTypeId: 0,
    UpdatedBy:0
  };

  //#endregion

  //#region Constcructor

  constructor(public modal: NgbActiveModal,private categoryService: CategorysService,private helperService: HelperService) { }

   //#endregion

  //#region Methods

  /**
   * NgOnInit
   * angular lifecircle hooks
   */

  ngOnInit(): void {
    if (this.category) {
      this.categoryFormObj = this.category;
    }

  }
  
    /**
   *submit button
   */

  submit() {
    var user = this.helperService.getUserDetail();
    this.categoryFormObj.UpdatedBy = user.userId;
    this.categoryService.createItem(this.categoryFormObj).subscribe((response: any) => {
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

  //#end submitbutton
}
