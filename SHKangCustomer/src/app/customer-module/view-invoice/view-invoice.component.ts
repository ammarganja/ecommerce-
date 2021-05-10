import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MyorderService } from 'src/app/core/services/customer_services/myorder.service';
import jspdf from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-view-invoice',
  templateUrl: './view-invoice.component.html',
  styleUrls: ['./view-invoice.component.scss'],
})
export class ViewInvoiceComponent implements OnInit {
  @Input() InvoiceId: any;
  orderInvoiceId: any;
  orderDetail: any;
  constructor(
    private myorderService: MyorderService,
    private model: NgbActiveModal
  ) {}

  ngOnInit(): void {
    this.orderInvoiceId = this.InvoiceId;
    if (this.orderInvoiceId) {
      let data = {
        orderInvoiceId: this.orderInvoiceId,
      };
      this.myorderService.getInvoiceDetails(data).subscribe((res: any) => {
        if (res.result) {
          this.orderDetail = res.data;
        }
      });
    }
  }
  close() {
    this.model.dismiss(ViewInvoiceComponent);
  }
  print() {
    let data:any = document.getElementById('contentToConvert');
    html2canvas(data).then((canvas) => {
      // Few necessary setting options
      var imgWidth = 200;
      var pageHeight = 'auto';
      var imgHeight = (canvas.height * imgWidth) / canvas.width;
      var heightLeft = imgHeight;

      const contentDataURL = canvas.toDataURL('image/png');
      
      let pdf = new jspdf('p', 'mm', 'a4'); // A4 size page of PDF
      // const imgProps= pdf.getImageProperties(contentDataURL);
      // const pdfWidth = pdf.internal.pageSize.getWidth();
      // const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;
      // pdf.text('Confidential', 490, pdf.internal.pageSize.height - 10);
      pdf.addImage(contentDataURL, 'PNG', 0, 0, imgWidth, imgHeight);
      pdf.save('new-file.pdf'); // Generated PDF
    });
  }
}
