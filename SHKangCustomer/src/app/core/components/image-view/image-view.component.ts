import { ToastrService } from 'ngx-toastr';
import { ProductService } from 'src/app/core/services/customer_services/product.service';
import { ChangeDetectorRef,Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-image-view',
  templateUrl: './image-view.component.html',
  styleUrls: ['./image-view.component.scss']
})
export class ImageViewComponent implements OnInit {

  SlideOptions = { items: 1, dots: false, nav: true };  
  @Input() productId: any;
  public imagesList :any;
  public colorsList :any;
  public allImagesList :any;
  constructor(private ngbActiveModal: NgbActiveModal,
    private changedetactionRef : ChangeDetectorRef,
    private productDetailService: ProductService,
    private toastr: ToastrService,) { }

  ngOnInit(): void {
    this.getProductDetail();
  }

  getProductDetail(){
    var params = {
      productId : this.productId
    }
    this.productDetailService.getProductDetail(params).subscribe((response: any) => {
      if(response.result && response.status == 200){
        this.colorsList = response.data.colorList;
        this.imagesList = response.data.imageList;
        this.allImagesList = response.data.imageList;
        this.setSelectedColor();
        if(this.colorsList && this.colorsList.length > 0){
          var firstColor = this.colorsList[0];
          this.getColorImage(firstColor.productColorId);
        }
        //console.log(this.imagesList);
        this.changedetactionRef.markForCheck();
      }else{
        this.toastr.error(response.message);
        return;
      }
    });
  }
  close() {
    this.ngbActiveModal.close();
  }

  /**
   * set selected color
   */
   setSelectedColor(){
    this.colorsList.forEach((color:any,index:any) => {
      if(index == 0){
        color.selected = true;
      }else{
        color.selected = false;
      }
    });
  }


  selectColor(productColorId:any){
    this.colorsList.forEach((color:any,index:any) => {
      color.selected = false;
      if(color.productColorId == productColorId){
        color.selected = true;
      }
    })
   this.getColorImage(productColorId);
  }
  getColorImage(productColorId:any){
    if(this.allImagesList && this.allImagesList.length > 0){
      var filterImage = this.allImagesList.filter((a: any) => a.productColorId == productColorId);
      if(filterImage && filterImage.length > 0){
        this.imagesList = filterImage;
      }
    }
  }
}
