import {Selectable} from '../../../shared/base/classes/selectable';
import {Component, OnInit, ViewChild} from '@angular/core';
import {DxFileUploaderComponent} from "devextreme-angular/ui/file-uploader";
import {
  HekmatPaymentGatewayTransactionBulkRequestListComponent
} from "./hekmat-payment-gateway-transaction-bulk-request-list.component";


@Component({
  selector: 'app-hekmat-payment-gateway-transaction-bulk-request',
  templateUrl: './hekmat-payment-gateway-transaction-bulk-request.component.html'
})
export class HekmatPaymentGatewayTransactionBulkRequestComponent extends Selectable implements OnInit {
  @ViewChild('transactionBulkRequest', {static: true}) transactionBulkRequest: HekmatPaymentGatewayTransactionBulkRequestListComponent;
  bulkRequestCategoryId: number;

  constructor() {
    super();
  }

  ngOnInit(): void {

  }

  /*
  ست کردن شناسه دسته بندی درخواست گروهی تراکنش حکمت
   */
  setBulkRequestCategoryId(bulkRequestCategory: any) {
    if (bulkRequestCategory) {
      this.transactionBulkRequest.setUploadUrl(bulkRequestCategory.id);
      this.bulkRequestCategoryId = bulkRequestCategory.id;
    } else {
      this.showMessage('خطا در ارسال دسته بندی');
    }
  }
}
