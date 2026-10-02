import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-hekmat-transaction-detail-tab-based',
  templateUrl: './hekmat-transaction-detail-tab-based.component.html'
})
export class HekmatTransactionDetailTabBasedComponent extends Editable implements OnInit {
  hekmatTransactionStatusType: number;

  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'hekmatTransaction';
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
        {id: 0, title: 'فایل تراکنش های پرداخت حکمت ', template: 'hekmatTransaction'},
        {id: 1, title: 'توضیحات', template: 'description', disabled: !this.allowEditDesc},
        {id: 2, title: 'نتیجه تراکنش حکمت', template: 'transactionResult', disabled: !this.allowEditDesc},
      ];
    } else {
      this.tabs[1].disabled = !this.allowEditDesc;
      this.tabs[2].disabled = !this.allowEditDesc;
    }

  }


}


