import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { contentService } from '../../services/content.services';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { HelperService } from '../../services/helper.service';

@Component({
  selector: 'app-edit-content-management',
  templateUrl: './edit-content-management.component.html',
  styleUrls: ['./edit-content-management.component.scss']
})
export class EditContentManagementComponent implements OnInit {
  
  //#region Variable Declaration
  @Input() selectData;
  contentFormObj = {
    name: null,
    shortName: null,
    description:null 
  };
  //#endregion
  
  //#region Constcructor
  constructor(public modal: NgbActiveModal,private contentService : contentService,private helperService : HelperService) {
  }
  //#endregion
  
  //#region Methods

  /**
   * NgOnInit
  */
  ngOnInit(): void {
    this.contentFormObj = {
      name: this.selectData.name,
      shortName: this.selectData.shortName,
      description:this.selectData.description
    };
  }
  
  /**
   * submit
  */
  submit() {
    if (this.contentFormObj.name && this.contentFormObj.shortName && this.contentFormObj.description)
    var user = this.helperService.getUserDetail();
    var params = {
      ContentId : this.selectData.contentId,
      Name : this.contentFormObj.name,
      ShortName : this.contentFormObj.shortName,
      Description : this.contentFormObj.description,
      CreatedBy : user.userId,
      UpdatedBy : user.userId
    }

    this.contentService.createItem(params).subscribe((response: any) => {
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
  
  //#endregion  
}
