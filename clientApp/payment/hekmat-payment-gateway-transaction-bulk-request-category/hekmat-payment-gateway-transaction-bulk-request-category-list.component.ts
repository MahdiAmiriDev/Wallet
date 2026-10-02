import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';


import {
  HekmatPaymentGatewayTransactionBulkRequestCategoryDetailComponent
} from './hekmat-payment-gateway-transaction-bulk-request-category-detail.component';


@Component({
  selector: 'app-hekmat-payment-gateway-transaction-bulk-request-category-list',
  templateUrl: './hekmat-payment-gateway-transaction-bulk-request-category-list.component.html'
})
export class HekmatPaymentGatewayTransactionBulkRequestCategoryListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'hekmatPaymentGatewayTransactionBulkRequestCategory';
  detailComponent = HekmatPaymentGatewayTransactionBulkRequestCategoryDetailComponent;



  _parentId: number;
  @Input()
  set parentId(val) {
    if (val !== this._parentId) {
      this._parentId = val;
    }
  }
  get parentId(): number {
    return this._parentId;
  }

  _yashilHekmatTerminalId: number;
  @Input()
  set yashilHekmatTerminalId(val) {
    if (val !== this._yashilHekmatTerminalId) {
      this._yashilHekmatTerminalId = val;
    }
  }
  get yashilHekmatTerminalId(): number {
    return this._yashilHekmatTerminalId;
  }

  private baseListUrl = 'hekmatPaymentGatewayTransactionBulkRequestCategory/GetByCustomFilterForList?';

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push(
      {
        caption: 'عنوان',
        dataField: 'title'
      },
      {
        caption: 'ترمینال درگاه انستیتو',
        dataField: 'yashilHekmatTerminalTitle'
      },
      {
        caption: 'شناسه ترمینال درگاه انستیتو',
        dataField: 'yashilHekmatTerminalId'
      },
    );
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {

      if (this.parentId) {
        customListUrl += `parentId=${this.parentId}&`;
      }

      if (this.yashilHekmatTerminalId) {
        customListUrl += `yashilHekmatTerminalId=${this.yashilHekmatTerminalId}&`;
      }


    }

    let res = false;
    if (this.parentId || this.yashilHekmatTerminalId) {


      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();

      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp =
      (<HekmatPaymentGatewayTransactionBulkRequestCategoryDetailComponent>componentInstance.instance);

    if (this.parentId) {
      comp.parentId = this.parentId;
    }

    if (this.yashilHekmatTerminalId) {
      comp.yashilHekmatTerminalId = this.yashilHekmatTerminalId;
    }

  }
}

