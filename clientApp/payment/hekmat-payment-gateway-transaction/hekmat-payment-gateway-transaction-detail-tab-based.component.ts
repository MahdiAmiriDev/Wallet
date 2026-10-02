import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-hekmat-payment-gateway-transaction-detail-tab-based',
  templateUrl: './hekmat-payment-gateway-transaction-detail-tab-based.component.html'
})
export class HekmatPaymentGatewayTransactionDetailTabBasedComponent extends Editable implements OnInit {
  hekmatTerminalId: number;

  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'hekmatPaymentGatewayTransaction';
  }

  ngOnInit() {
    if (this.selectedEntityId && this.selectedEntityId !== 0) {
      this.allowEditDesc = true;
    } else {
      this.allowEditDesc = false;
    }
    this.bindTabs();
  }

  rowInserted(insertedRowId: any) {
    this.selectedEntityId = insertedRowId;
    this.allowEditDesc = true;
    this.bindTabs();
  }

  bindTabs() {

    if (this.tabs.length == 0) {
      this.tabs = [
        {id: 0, title: 'تراکنش های درگاه پرداخت حکمت ', template: 'hekmatPaymentGatewayTransaction'},
        {id: 1, title: 'آدرس بازگشت', template: 'callBackUrl', disabled: !this.allowEditDesc},
        {id: 2, title: 'داده های تکمیلی', template: 'additionalDate', disabled: !this.allowEditDesc},
        {id: 3, title: 'توکن حکمت', template: 'hekmatToken', disabled: !this.allowEditDesc},
        {id: 4, title: 'نتیجه تراکنش حکمت', template: 'hekmatTransactionResultMessage', disabled: !this.allowEditDesc},
        {id: 5, title: 'توضیحات', template: 'description', disabled: !this.allowEditDesc},
      ];
    } else {
      this.tabs[1].disabled = !this.allowEditDesc;
      this.tabs[2].disabled = !this.allowEditDesc;
      this.tabs[3].disabled = !this.allowEditDesc;
      this.tabs[4].disabled = !this.allowEditDesc;
      this.tabs[5].disabled = !this.allowEditDesc;
    }

  }


}


