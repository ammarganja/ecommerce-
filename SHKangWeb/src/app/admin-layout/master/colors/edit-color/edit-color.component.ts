import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { IAlert } from 'src/app/admin-layout/_models/alert.model';
import { ColorService } from '../../../services/color.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-edit-color',
  templateUrl: './edit-color.component.html',
  styleUrls: ['./edit-color.component.scss']
})

export class EditColorComponent implements OnInit {

  //#region Variable Declaration
  @Input() color: any;
  public modelType: any;
  colorFormObj = {
    ColorId: 0,
    Name: null,
  };
  errorMessage = '';
  //#endregion

  //#region Constcructor
  constructor(public modal: NgbActiveModal,public colorService: ColorService) { }
  //#endregion

  /**
   * NgOnInit
  */
  ngOnInit(): void {
    if (this.color) {
      this.colorFormObj = this.color;
    }

  }
  /**
   * submit
  */
  submit() {
    this.colorService.createItem(this.colorFormObj).subscribe((response: any) => {
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
  
  closeAlert() {
   this.errorMessage = '';
  }
}
