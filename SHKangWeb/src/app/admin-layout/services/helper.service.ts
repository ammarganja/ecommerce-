import { Injectable } from '@angular/core';
import { Column } from '../_models/columns.model';

@Injectable({
  providedIn: 'root',
})
export class HelperService {
  //#region Variable Declarations
  columns: Column[];
  //#endregion
  //#region Constructor
  constructor() { }
  //#endregion

  //#region functions
  /**
   *
   * @returns Size Columns
   */
  getSizeColumns() {
    return (this.columns = [
      {
        columnName: 'Id',
        sortProprty: 'sizeId',
        canSort: true,
      },
      {
        columnName: 'Size group name',
        sortProprty: 'sizeName',
        canSort: true,
      },
      {
        columnName: 'Size Units',
        sortProprty: 'units',
        canSort: true,
      },
      {
        columnName: 'Total Products',
        sortProprty: 'total_products',
        canSort: false,
      },
      {
        columnName: 'Actions',
        sortProprty: '',
        canSort: false,
      },
    ]);
  }
  /**
   *
   * @returns Promocode Columns
   */
  getPromoCodeColumns() {
    return (this.columns = [
      {
        columnName: 'Id',
        sortProprty: 'promoCodeId',
        canSort: true,
      },
      {
        columnName: 'Name',
        sortProprty: 'name',
        canSort: true,
      },
      {
        columnName: 'Code',
        sortProprty: 'code',
        canSort: true,
      },
      {
        columnName: 'Start Date',
        sortProprty: 'startDate',
        canSort: true,
      },
      {
        columnName: 'Expiry Date',
        sortProprty: 'expiryDate',
        canSort: true,
      },
      {
        columnName: 'Discount',
        sortProprty: 'discount',
        canSort: true,
      },
      {
        columnName: 'code Status',
        sortProprty: 'codestatus',
        canSort: false,
      },
      {
        columnName: 'Actions',
        sortProprty: '',
        canSort: false,
      },
    ]);
  }

  /**
   *
   * @returns Promocode Columns
   */
  getCampaignsColumns() {
    return (this.columns = [
      {
        columnName: 'Id',
        sortProprty: 'campaignId',
        canSort: true,
      },
      {
        columnName: 'Image',
        sortProprty: 'image',
        canSort: false,
      },
      {
        columnName: 'Name',
        sortProprty: 'campaignName',
        canSort: true,
      },
      {
        columnName: 'Description',
        sortProprty: 'description',
        canSort: true,
      },
      // {
      //   columnName: 'Products',
      //   sortProprty: 'products',
      //   canSort: false,
      // },
      {
        columnName: 'Action',
        sortProprty: '',
        canSort: false,
      },
    ]);
  }

  getCampaignsProductColumns() {
    return (this.columns = [
      {
        columnName: '',
        sortProprty: 'productId',
        canSort: false,
      },
      {
        columnName: 'Image',
        sortProprty: 'image',
        canSort: false,
      },
      {
        columnName: 'Name/Style',
        sortProprty: 'productName',
        canSort: true,
      },
      {
        columnName: 'Category',
        sortProprty: 'category',
        canSort: true,
      },
      {
        columnName: 'Color',
        sortProprty: 'color',
        canSort: true,
      },
      {
        columnName: 'Size Ratio',
        sortProprty: 'sizeGroup',
        canSort: true,
      },
      {
        columnName: 'Price',
        sortProprty: 'price',
        canSort: true,
      }
    ]);
  }

  /**
   *
   * @returns Color Columns
   */
  getColorColumns() {
    return (this.columns = [
      {
        columnName: 'Id',
        sortProprty: 'colorId',
        canSort: true,
      },
      {
        columnName: 'Name',
        sortProprty: 'name',
        canSort: true,
      },
      {
        columnName: 'TOTAL PRODUCTS',
        sortProprty: 'products',
        canSort: false,
      },
      {
        columnName: 'Actions',
        sortProprty: '',
        canSort: false,
      },
    ]);
  }

  /**
   *
   * @returns Category Columns
   */
  getCategoryColumns() {
    return [
      {
        columnName: 'Id',
        sortProprty: 'productCategoryTypeId',
        canSort: true,
      },
      {
        columnName: 'Name',
        sortProprty: 'name',
        canSort: true,
      },
      {
        columnName: 'Total Products',
        sortProprty: 'total_products',
        canSort: false,
      },
      {
        columnName: 'Action',
        sortProprty: '',
        canSort: false,
      },
    ];
  }
  /**
   *
   * @returns Customer Columns
   */
  getCustomerColumns() {
    return [
      {
        columnName: 'Id',
        sortProprty: 'userId',
        canSort: true,
      },
      {
        columnName: 'FullName',
        sortProprty: 'name',
        canSort: true,
      },
      {
        columnName: 'Company Name',
        sortProprty: 'company',
        canSort: true,
      },
      {
        columnName: 'Email',
        sortProprty: 'email',
        canSort: true,
      },
      {
        columnName: 'Phone Number',
        sortProprty: 'mobilenumber',
        canSort: true,
      },
      {
        columnName: 'Status',
        sortProprty: 'status',
        canSort: false,
      },
      {
        columnName: 'Address',
        sortProprty: 'address',
        canSort: false,
      },
      {
        columnName: 'Action',
        sortProprty: '',
        canSort: false,
      },
    ];
  }
  /**
   *
   * @returns Order Columns
   */
  getOrderColumns() {
    return [
      {
        columnName: 'Id',
        sortProprty: 'orderId',
        canSort: true,
      },
      {
        columnName: 'Date',
        sortProprty: 'orderDate',
        canSort: true,
      },
      {
        columnName: 'PO Number',
        sortProprty: 'poNumber',
        canSort: true,
      },
      {
        columnName: 'Invoices',
        sortProprty: 'invoice',
        canSort: false,
      },

      {
        columnName: 'Email',
        sortProprty: 'emailAddress',
        canSort: true,
      },
      {
        columnName: 'Phone',
        sortProprty: 'mobileNumber',
        canSort: true,
      },

      {
        columnName: 'Total Amount',
        sortProprty: 'payableAmount',
        canSort: true,
      },
      {
        columnName: 'Actions',
        sortProprty: '',
        canSort: false,
      },
    ];
  }

  getProductColumns() {
    return [
      {
        columnName: 'Id',
        sortProprty: 'productId',
        canSort: true,
      },
      {
        columnName: 'Image',
        sortProprty: 'image',
        canSort: false,
      },
      {
        columnName: 'Name',
        sortProprty: 'productName',
        canSort: true,
      },
      {
        columnName: 'Category',
        sortProprty: 'categoryType',
        canSort: true,
      },
      {
        columnName: 'Price',
        sortProprty: 'price',
        canSort: true,
      },
      // {
      //   columnName: 'Type',
      //   sortProprty: 'type',
      //   canSort: true,
      // },
      {
        columnName: 'Color',
        sortProprty: 'color',
        canSort: true,
      },
      {
        columnName: 'Size Group Name',
        sortProprty: 'size',
        canSort: true,
      },
      {
        columnName: 'Status',
        sortProprty: 'status',
        canSort: true,
      },
      {
        columnName: 'Action',
        sortProprty: '',
        canSort: false,
      },

    ];
  }


  /**
   *
   * @returns Content Columns
   */
  getContentColumns() {
    return [
      {
        columnName: 'Id',
        sortProprty: 'contentId',
        canSort: true,
      },
      {
        columnName: 'Name',
        sortProprty: 'name',
        canSort: true,
      },
      {
        columnName: 'Action',
        sortProprty: '',
        canSort: false,
      },
    ];
  }

  getAddressColumns() {
    return [
      {
        columnName: 'Id',
        sortProprty: 'useraddressid',
        canSort: true,
      },
      {
        columnName: 'Full Name',
        sortProprty: 'fullname',
        canSort: false,
      },
      {
        columnName: 'Phone Number',
        sortProprty: 'mobilenumber',
        canSort: true,
      },
      {
        columnName: 'Address1',
        sortProprty: 'address1',
        canSort: true,
      },
      {
        columnName: 'Address2',
        sortProprty: 'address2',
        canSort: true,
      },
      {
        columnName: 'Country',
        sortProprty: 'country',
        canSort: true,
      },
      {
        columnName: 'State',
        sortProprty: 'state',
        canSort: true,
      },
      {
        columnName: 'Town/City',
        sortProprty: 'city',
        canSort: true,
      },
      {
        columnName: 'ZipCode',
        sortProprty: 'zipcode',
        canSort: true,
      },
      {
        columnName: 'Action',
        sortProprty: '',
        canSort: false,
      },

    ];
  }

  /*
   * To check Input is Decimal Value
   */
  validateDecimal(val) {
    let res = val.match('^[1-9]d*(.d+)?$');
    if (res == null) {
      return true;
    }
  }

  singleSelectSetting() {
    return {
      classes: 'add-product-width',
      singleSelection: true,
      enableSearchFilter: true,
    };
  }
  MultiSelectSetting() {
    return {
      classes: 'add-product-width',
      enableSearchFilter: true,
      enableCheckAll: false,
    };
  }

  getUserDetail() {
    var user = JSON.parse(localStorage.getItem('userdata'));
    return user;
  }
  //#endregion
}
