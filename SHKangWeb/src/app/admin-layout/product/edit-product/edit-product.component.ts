import { Component, Input, OnInit } from '@angular/core';
import {
  NgbActiveModal,
  NgbDateAdapter,
  NgbDateParserFormatter,
  NgbModal,
} from '@ng-bootstrap/ng-bootstrap';
import { CustomAdapter, CustomDateParserFormatter } from 'src/app/core';
import { NgForm } from '@angular/forms';
import { ProductsService } from '../../services/products.service';
import { EditColorComponent } from '../../master/colors/edit-color/edit-color.component';
import { EditCategoryComponent } from '../../master/category/edit-category/edit-category.component';
import { EditSizeComponent } from '../../master/size/edit-size/edit-size.component';
import { HttpClient } from '@angular/common/http';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { HelperService } from '../../services/helper.service';
import { element } from 'protractor';
@Component({
  selector: 'app-edit-product-model',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.scss'],
  providers: [
    { provide: NgbDateAdapter, useClass: CustomAdapter },
    { provide: NgbDateParserFormatter, useClass: CustomDateParserFormatter },
  ],
})
export class EditProductComponent implements OnInit {
  //#region Variable Declaration

  @Input() product: any;
  public categoryList: Array<any> = new Array<any>();
  public sizeGroupList: Array<any> = new Array<any>();
  public sizeUnitList: Array<any> = new Array<any>();
  public colorsList: Array<any> = new Array<any>();
  public sizeUnitObj: Array<any>;
  public productId: string;
  public productData: string;
  public categorySelectItem: Array<any> = new Array<any>();
  public sizeModel: Array<any> = new Array<any>();
  public colorsSelecteditem : Array<any> = new Array<any>();
  public colorImageContainer : Array<any> = new Array<any>();
  formGroup: any;
  subscriptions: any = [];
  deleteObjects : any = [];

  TypeSelectItem = [];

  addProductObj = {
    ProductId: null,
    Name: null,
    Price: null,
    ProductCategoryId: "",
    ProductDescription: null,
    CreatedBy: 2,
    ProductSizeId: null,
    ProductSizeValue: "",
    UpdatedBy: 2
  };

  disabled = false;
  ShowFilter = false;
  limitSelection = false;


  types = [];

  styleData = [];
  SizeSelectRatio = [];
  files: File[] = [];

  isMatCheckBox: any;

  styleModel: any;

  dropdownSettings: any = {};
  singleSelectsettings: any;
  multiSelectSettings: any;
  selectedCampaign = [];
  campaigns = [];
  color: any;
  countcolor: any;
  
  selectedColors: any[];
  config: any;

  files_dropped: File[] = [];
  dimensions: any = [];
  dataArr: any = [];
  customDropzone: string[];
  router: any;

  //#endregion

  //#region Constcructor
  constructor(
    public modal: NgbActiveModal,
    private modalService: NgbModal,
    public productService: ProductsService,
    public helperService: HelperService,
    private http: HttpClient
  ) { }

  //#endregion

  //#region Methods

  /**
   * NgOnInit
   */
  ngOnInit() {
    this.getCategoryList();
    this.getSizeGroupList();
    this.getColorList();
    
    this.singleSelectsettings = this.helperService.singleSelectSetting();
    this.multiSelectSettings = this.helperService.MultiSelectSetting();
    
    if (this.product) {
      this.editProductDetails(this.product.productId)
    }
  }

  editProductDetails(productId: number) {
    this.productService.getProductDetails(Number(productId)).subscribe((response: any) => {
      if (response.result) {
        this.productData = response.data;
        this.addProductObj = {
          ProductId: response.data.productId,
          Name: response.data.name,
          Price: response.data.price,
          ProductCategoryId: "",
          ProductDescription: response.data.productDescription,
          CreatedBy: 2,
          ProductSizeId: Number(response.data.productSizeId),
          ProductSizeValue: "",
          UpdatedBy: 2
        };

        //category 
        response.data.categoryList.forEach(element => {
          let categoryItem = {
            "id": element.productCategoryTypeId,
            "itemName": element.categoryType,
            "name": element.categoryType
          }
          this.categorySelectItem.push(categoryItem);
        });

        //color list
        response.data.colorList.forEach(element => {
          let colorItem = {
            "id": element.productColorId,
            "itemName": element.color,
            "name": element.color,
            "images":response.data.imageList
          }
          this.colorsSelecteditem.push(colorItem);
          
          //color images
          let img = [];
          response.data.imageList.forEach(imgElement => {
            if(imgElement.productColorId == element.productColorId){
              img.push(imgElement.image);
            }
          });
          let colorImages = {
            ...colorItem,
            image :img
          }
          this.colorImageContainer.push(colorImages);
        });

        //size group
        this.sizeGroupList.forEach(element => {
          if (element.id == response.data.productSizeId) {
            let sizeItem = {
              "id": element.id,
              "itemName": element.itemName,
              "name": element.name
            }
            this.sizeModel.push(sizeItem);
            this.sizeUnitObj = new Array<any>();
            response.data.sizeGroupList.forEach(element => {
              this.sizeUnitObj.push({
                name: element.itemName,
                value: element.itemValue
              })
            });
          }
        })
      } else {
        Swal.fire({
          title: 'Error',
          text: response.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { });
      }
    });
  }
  /**
   * GetColorList
   */
  getColorList() {
    this.productService.colorList().subscribe((response: any) => {
      if (response.result) {
        this.colorsList = response.data;
      } else {
        Swal.fire({
          title: 'Error',
          text: response.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { });
      }
    });
  }

  /**
   * getCategoryList
   */
  getCategoryList() {
    this.productService.categoryList().subscribe((response: any) => {
      if (response.result) {
        this.categoryList = response.data;
      } else {
        Swal.fire({
          title: 'Error',
          text: response.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { });
      }
    });
  }

  /**
   * GetSizeGroupList
   */
  getSizeGroupList() {
    this.productService.getSizeGroupList().subscribe((response: any) => {
      if (response.result) {
        this.sizeGroupList = response.data;
      } else {
        Swal.fire({
          title: 'Error',
          text: response.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { });
      }
    });
  }

  /**
   * GetSizeUnitList
   */
  getSizeUnitList(id: string) {
    this.productService.getSizeUnitList(id).subscribe((response: any) => {
      if (response.result) {
        this.sizeUnitList = response.data;
        this.createSizeUnitElement();
      } else {
        Swal.fire({
          title: 'Error',
          text: response.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { });
      }
    });
  }

  /**
   * CreateSizeUnitElement
   */
  createSizeUnitElement() {
    this.sizeUnitObj = new Array<any>();
    this.sizeUnitList.forEach(element => {
      this.sizeUnitObj.push({
        name: element,
        value: ''
      })
    });
  }

  /**
   * OnColorItemSelect
   */
  onColorItemSelect(item: any) {
    if (this.colorImageContainer.length) {
      let exist = this.colorImageContainer.filter((data) => data.id == item.id);
      if (exist && exist.length === 0) {
        ///this.countcolor = this.countcolor + 1;
        this.colorImageContainer.push(item);
      }
    } else {
      ///this.countcolor = this.countcolor + 1;
      this.colorImageContainer.push(item);
    }
  }

  /**
   * OnColorDeselect
   */
  OnColorDeselect(item: any) {
    this.colorImageContainer.forEach((value, index) => {
      if (value.id === item.id) {
        this.colorImageContainer.splice(index, 1);
        
        if(this.addProductObj.ProductId){
          let removeColorObject = {
            ProductId : this.addProductObj.ProductId, 
            ColorId : item.id
          }
          this.productService.removeProductColor(removeColorObject).subscribe((response: any) => {
            if (response.result) {      
            } else {
              Swal.fire({
                title: 'Error',
                text: response.message,
                icon: 'error',
                showCancelButton: false,
                confirmButtonText: 'Ok'
              }).then((result) => { });
            }
          });
        }
      }
    });
  }

  /**
   * onColorRemoveAll
   */
  onColorRemoveAll(items: any) {
    if (this.colorImageContainer.length != 0) {
      this.colorImageContainer.length = 0;
    }
  }

  /**
   * OnSelectColorFiles
   */
  onSelectColorFiles(event: any, color: any) {
    this.colorImageContainer.forEach((val, index) => {
      if (!val.image) {
        val.image = [];
      }
    });
    this.colorImageContainer.forEach((val, index) => {
      if (val.id == color.id) {
        val.image.push(...event.addedFiles);
      }
    });
    console.log(this.colorImageContainer);
  }

  /**
   * OnRemoveColorFiles
   */
  onRemoveColorFiles(event, color,i) {
    let removedImage:string;
    this.colorImageContainer.forEach((val, index) => {
      val.image.forEach((va1, index1) => {
        if (event.name && va1.name == event.name && color.id == val.id) {
          va1 = val.image.splice(i, 1);
          removedImage = va1;
        }else if(va1 == event && color.id == val.id){
          va1 = val.image.splice(i, 1);
          removedImage = va1;
        }
      });
    });
    color.images.forEach(element => {
      if (element.image==removedImage[0]) {
        this.deleteObjects.push({colorId:element.productColorId,productImageId: element.productImageId});
      }
    });
  }

  /**
   * OnSizeItemSelect
   */
  onSizeItemSelect(e) {
    if (e && e.id) {
      this.addProductObj.ProductSizeId = e.id;
      this.getSizeUnitList(e.id);
    } else {
      Swal.fire({
        title: 'Error',
        text: "Size Unit not found",
        icon: 'error',
        showCancelButton: false,
        confirmButtonText: 'Ok'
      }).then((result) => { });
    }
  }


  /**
   * OnSizeSelectAll
   */
  onSizeSelectAll(items: any) {
    console.log('onSelectAll', items);
  }


  /**
   * OnCategoryItemSelect
   */
  onCategoryItemSelect(item: any) {
  
  }

  /**
   * Addcategory
   */
  addcategory() {
    const modalRef = this.modalService.open(EditCategoryComponent, {
      size: 'sm',
    });
    modalRef.result.then(() => { 
      this.getCategoryList();
    });
  }

  /**
   * Addcolor
   */
  addcolor(customer: any) {
    const modalRef = this.modalService.open(EditColorComponent, { size: 'sm' });
    // modalRef.componentInstance.customer = customer;
    modalRef.result.then(() =>{
      this.getColorList();
     }
    );
  }

  /**
   * Addsize
   */
  addsize() {
    const modalRef = this.modalService.open(EditSizeComponent, { size: 'lg' });
    modalRef.result.then(() =>{
      this.getSizeGroupList();
     });
  }

  /**
   * OnCategoryAll
   */
  onCategoryAll(items: any) {
    console.log('onSelectAll', items);
  }

  /**
   * Submit
   */
  submit(productForm: NgForm) {
    if (this.product) {
      this.addProductObj.ProductId = this.product.productId;
    } else {
      this.addProductObj.ProductId = 0;
    }

    let commaSeperatedSizeUnit = this.sizeUnitObj.map(function (val) {
      return val.value;
    }).join(',');

    let commaSeperatedCategoryItems = this.categorySelectItem.map(function (val) {
      return val.id;
    }).join(',');

    this.addProductObj.ProductSizeValue = commaSeperatedSizeUnit;
    this.addProductObj.ProductCategoryId = commaSeperatedCategoryItems;

    this.productService.addProduct(this.addProductObj).subscribe((response: any) => {
      this.productId = response.data;
      if (response.result) {
        this.colorImageContainer.forEach((element, index, array) => {
          if(this.deleteObjects && this.deleteObjects.length > 0){
            var arr = [];
            this.deleteObjects.forEach(obj => {
              if(obj.colorId == element.id){
                arr.push(obj.productImageId);
              }
            });
          }
          let productColorImage = {
            productId: this.productId,
            ColorId: element.id,
            image: element.image,
            DeleteImageId : arr && arr.length > 0 ? arr.join(',') : ''
          }

          if(element.image.length > 0){
            this.productService.addProductColor(productColorImage).subscribe((response: any) => {
              if (response.result) {
                if (index === (array.length - 1)) {
                  this.modal.close()
                  // this.router.navigate(['/products']);
                  this.productService.fetch()
                }
              } else {
                Swal.fire({
                  title: 'Error',
                  text: response.message,
                  icon: 'error',
                  showCancelButton: false,
                  confirmButtonText: 'Ok'
                }).then((result) => { });
              }
            });
          }else{
            Swal.fire({
              title: 'Error',
              text: "Please add atleast one image for color " + element.itemName,
              icon: 'error',
              showCancelButton: false,
              confirmButtonText: 'Ok'
            }).then((result) => { });
          }
          
          
        });
        // if (this.product) {
        //   this.modal.close()
        //   this.productService.fetch()
        // }else{
          
        // }       
      } else {
        Swal.fire({
          title: 'Error',
          text: response.message,
          icon: 'error',
          showCancelButton: false,
          confirmButtonText: 'Ok'
        }).then((result) => { });
      }
    });
  }

  isImageFile(file){
    if(file && file.type){
      if(file.type.split('/')[0] == "video"){
        return false;
      }else{
        return true;
      }
    }else{
      if(file.split('.').pop() == "mp4" || file.split('.').pop() == "webm" || file.split('.').pop() == "wmv" || file.split('.').pop() == "MTS" ){
        return false
      }
      return true;
    }


  }
}