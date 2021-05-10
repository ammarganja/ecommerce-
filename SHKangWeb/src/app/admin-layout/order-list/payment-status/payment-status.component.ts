import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { orderDetail } from '../../create-invoice/product.related.data';
import { orderService } from '../../services/order.services';
import { jsPDF } from "jspdf";
import html2canvas from 'html2canvas';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-payment-status',
  templateUrl: './payment-status.component.html',
  styleUrls: ['./payment-status.component.scss'],
})
export class PaymentStatusComponent implements OnInit {
  @Input() invoiceDataId;

  orderDetail 
  // = orderDetail;
  orderDetails
  invoiceData
  invoiceDataRes
  USerOrderId: any;
  constructor(
    public modal: NgbActiveModal,
    private orderService: orderService
  ) {}

  ngOnInit(): void {
    let data = {
      orderInvoiceId: this.invoiceDataId,
    };
    this.orderService.getInvoiceDataById(data).subscribe((res) => {
      this.invoiceDataRes = res;
      if (this.invoiceDataRes.result) {
        this.orderDetail = this.invoiceDataRes.data;
      }
    });
  }
  cancel() {
    this.modal.dismiss(PaymentStatusComponent);
  }

  emailInvoice(){
    let data = {
      orderInvoiceId:this.invoiceDataId,
    }
    this.orderService.sendEmailInvoice(data).subscribe((data:any)=>{
      if(data.result){
        Swal.fire({
          title: 'Success',
          text: "Successfully sent email",
          icon: 'success',
          showCancelButton: false,
          confirmButtonText: 'Ok',
        });      
      }
    })    
  }
  print(){
    let data:any = document.getElementById('contentToConvert');
    html2canvas(data).then((canvas) => {
      // Few necessary setting options
      var imgWidth = 500;
      var pageHeight = 1500;
      var imgHeight = (canvas.height * imgWidth) / canvas.width;
      var heightLeft = imgHeight;

      const contentDataURL = canvas.toDataURL('image/png');

      html2canvas(data).then(canvas => {  
        // Few necessary setting options  
        var imgWidth = 190;   
        var pageHeight = 1050;    
        var imgHeight = canvas.height * imgWidth / canvas.width;  
        var heightLeft = imgHeight;  
    
        const contentDataURL = canvas.toDataURL('image/png')  
        let pdf = new jsPDF('p', 'mm', 'a4'); // A4 size page of PDF  
        var position = 0; 
        pdf.setFontSize(10)
        pdf.internal.scaleFactor = 1.50;
        pdf.addImage(contentDataURL, 'PNG', 0, position, imgWidth, imgHeight)  
        pdf.save('MYPdf.pdf'); // Generated PDF   
      });  
      // let pdf = new jsPDF('p', 'mm', 'a4'); // A4 size page of PDF
      // // const imgProps= pdf.getImageProperties(contentDataURL);
      // // const pdfWidth = pdf.internal.pageSize.getWidth();
      // // const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;
      // // pdf.text('Confidential', 490, pdf.internal.pageSize.height - 10);
      // pdf.addImage(contentDataURL, 'PNG', 0, 0, imgWidth, imgHeight);
      // // pdf.fromHTML(contentDataURL, 15, 15);
      // pdf.save('new-file.pdf'); // Generated PDF
    });
  }

}
