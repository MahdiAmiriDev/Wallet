import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {Editable} from '../../../shared/base/classes/editable';
import {FactorPaymentDetailComponent} from './factor-payment-detail.component';


@Component({
  selector: 'app-factor-payment-detail-tab-based',
  templateUrl: './factor-payment-detail-tab-based.component.html'
})
export class FactorPaymentDetailTabBasedComponent extends Editable implements OnInit {
  @ViewChild('factorPaymentDetail', {static: false}) factorPaymentDetail: FactorPaymentDetailComponent;
  allowEditDesc: boolean;
  tabs: any[] = [];
  @Input()
  factorId: number;

  constructor() {
    super();
    this.reloadDataForSelectedTab = this.reloadDataForSelectedTab.bind(this);
    this.entityName = 'factorPayment';
  }

  ngOnInit(): void {
    if (this.selectedEntityId && this.selectedEntityId !== 0) {
      this.allowEditDesc = true;
    } else {
      this.allowEditDesc = false;
    }
    this.bindTabs();
  }

  rowInserted(insertedRowId: any): void {
    this.selectedEntityId = insertedRowId;
    this.allowEditDesc = true;
    this.bindTabs();
  }

  bindTabs(): void {
    if (this.tabs.length == 0) {
      this.tabs = [
        {id: 1, title: 'پرداخت فاکتور', template: 'factorPayment'},
        {id: 2, title: 'کسورات فاکتور', template: 'factorDeduction'}
      ];
    }
    if (this.selectedEntityId && this.selectedEntityId !== 0) {
      this.tabs.push({id: 3, title: 'سند پرداخت فاکتور', template: 'factorPaymentDocs', disabled: !this.allowEditDesc});
    }
  }


  // هنگامی که تب ها تغییر میکند بدلیل اینکه ممکن است در تب سایر کسورات مبلغی را ثبت یا ویرایش کرده باشد
  // پس باید تب مربوط به اطلاعات پرداخت مجددا داده ها را از سرور بگیرد
  reloadDataForSelectedTab(selectedTab: any): void {
    if (selectedTab.addedItems[0].id === this.tabs[0].id) {
      this.factorPaymentDetail.reloadFactorStatisticData();
    }
  }
}


