import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomersService } from '../../services/customers.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-edit-customer',
  templateUrl: './edit-customer.component.html',
  styleUrls: ['./edit-customer.component.scss'],
})
export class EditCustomerComponent implements OnInit {
  //#region Variable Declaration

  @Input() customer: any;

  singleSelectsettings;
  customerStatus: { id: number; itemName: string }[];
  statusModel: any[];
  public modelType: any;
  customerFormObj : any = {}; 
  confirmPassword : any = '';

  //#endregion

  //#region Constcructor

  constructor(public modal: NgbActiveModal,private userService : CustomersService) {}
  //#endregion

  //#region Methods

  /**
   * NgOnInit
   */
  ngOnInit(): void {
    this.customerStatus = [
      { id: 1, itemName: 'Active' },
      { id: 2, itemName: 'Inactive' },
    ];

    this.singleSelectsettings = {
      singleSelection: true,
      enableSearchFilter: false,
    };

    if (this.customer) {
      this.customerFormObj = this.customer;
    }
  }
/**
 * Form action submit
 *
 */
  submit() {
    var params = {
      UserId: this.customerFormObj.userId ? this.customerFormObj.userId : 0,
      FirstName: this.customerFormObj.firstName + (this.customerFormObj.lastName ? this.customerFormObj.lastName : ''),
      LastName : '',
      PhoneNumber: this.customerFormObj.phoneNumber,
      EmailAddress: this.customerFormObj.emailAddress,
      Password: this.customerFormObj.password,
      CompanyName: this.customerFormObj.companyName,
      RoleType: this.customerFormObj.roleType ? this.customerFormObj.roleType : 2,
    }

    this.userService.saveCustomer(params).subscribe((response: any) => {
      if (response.result) {
        this.modal.close(EditCustomerComponent);
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
}
