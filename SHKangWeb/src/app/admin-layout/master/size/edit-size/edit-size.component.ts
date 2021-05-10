import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SizeService } from 'src/app/admin-layout/services/size.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-edit-size',
  templateUrl: './edit-size.component.html',
  styleUrls: ['./edit-size.component.scss']
})
export class EditSizeComponent implements OnInit {

  constructor(public modal: NgbActiveModal, public sizeService: SizeService) { }
  @Input() sizeFormData;
  sizeModel: any;
  sizeFormObj = {
    // flag:1,
    sizeId: null,
    sizeName: null,
    units: '',
    sizeList: [
      {
        id: 1,
        size_name: null
      }
    ]
  };


  ngOnInit(): void {
    if (this.sizeFormData) {
      this.sizeFormObj.sizeId = this.sizeFormData.sizeId;
      this.sizeFormObj.sizeName = this.sizeFormData.sizeName;
      this.sizeFormObj.units = this.sizeFormData.units;
      let sizes = this.sizeFormObj.units.split('/');
      if (sizes && sizes.length > 0) {
        sizes.forEach((size, index) => {
          if (index == 0) {
            this.sizeFormObj.sizeList[0].size_name = size;
          } else {
            this.sizeFormObj.sizeList.push({ id: index + 1, size_name: size });
          }
        })
      }


    }
  }

  submit() {
    let sizes = [];
    this.sizeFormObj.sizeList.forEach(item => {
      sizes.push(item.size_name);
    })
    let params = {
      SizeId: this.sizeFormObj.sizeId ? this.sizeFormObj.sizeId : 0,
      Name: this.sizeFormObj.sizeName,
      Units: sizes.join(',')
    }
    this.sizeService.createItem(params).subscribe((response: any) => {
      if (response.result) {
        this.modal.close();
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

  // submit() {
  //   if (this.sizeFormObj.size_name && this.sizeFormObj.sizeList.length != 0)
  //     this.modal.dismiss(EditSizeComponent);
  // }

  add() {
    let lastElement = this.sizeFormObj.sizeList[this.sizeFormObj.sizeList.length - 1];
    let data = {
      id: lastElement.id + 1,
      size_name: null
    }
    if (this.sizeFormObj.sizeList.length != 15) {
      this.sizeFormObj.sizeList.push(data)
    }
  }

  minus() {
    if (this.sizeFormObj.sizeList.length > 1) {
      this.sizeFormObj.sizeList.pop()
    }
  }

  remove(id) {
    if (this.sizeFormObj.sizeList.length > 1) {
      this.sizeFormObj.sizeList.forEach(function (item, index, object) {
        if (item.id === id) {
          object.splice(index, 1);
        }
      });
    }
  }
}
