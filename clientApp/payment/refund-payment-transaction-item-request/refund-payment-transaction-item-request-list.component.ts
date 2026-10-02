import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';


import {
  RefundPaymentTransactionItemRequestDetailComponent
} from './refund-payment-transaction-item-request-detail.component';
import {GenericDataService} from "../../../shared/base/services/generic-data.service";


@Component({
  selector: 'app-refund-payment-transaction-item-request-list',
  templateUrl: './refund-payment-transaction-item-request-list.component.html'
})
export class RefundPaymentTransactionItemRequestListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'refundPaymentTransactionItemRequest';
  detailComponent = RefundPaymentTransactionItemRequestDetailComponent;
  vendorDataSource: any;

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

  _refundPaymentTransactionItemRequestType: number;
  @Input()
  set refundPaymentTransactionItemRequestType(val) {
    if (val !== this._refundPaymentTransactionItemRequestType) {
      this._refundPaymentTransactionItemRequestType = val;
    }
  }

  get refundPaymentTransactionItemRequestType(): number {
    return this._refundPaymentTransactionItemRequestType;
  }

  _refundPaymentTransactionItemRequestStatus: number;
  @Input()
  set refundPaymentTransactionItemRequestStatus(val) {
    if (val !== this._refundPaymentTransactionItemRequestStatus) {
      this._refundPaymentTransactionItemRequestStatus = val;
    }
  }

  get refundPaymentTransactionItemRequestStatus(): number {
    return this._refundPaymentTransactionItemRequestStatus;
  }

  private baseListUrl = 'refundPaymentTransactionItemRequest/GetByCustomFilterForList?';

  constructor(private genericDataService:GenericDataService) {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();

    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }

    this.vendorDataSource = this.genericDataService
      .createCustomDataSourceWithAction('id','YashilHekmatTerminalSupplier','GetSupplierInformationForSelect');
  }

  private bindColumns(): void {
    this.columns.push(
      // Iterate all columns

      {
        caption: 'ParentId',
        dataField: 'parentTitle'
      },

      {
        caption: 'شناسه ترمینال',
        dataField: 'yashilHekmatTerminalTitle'
      },

      {
        caption: 'شناسه تراکنش پدیرنده',
        dataField: 'reservationNumber'
      },


      {
        caption: 'نوع درخواست',
        dataField: 'refundPaymentTransactionItemRequestTypeTitle'
      },

      {
        caption: 'وضعیت درخواست',
        dataField: 'refundPaymentTransactionItemRequestStatusTitle'
      },


      {
        caption: 'CodePath',
        dataField: 'codePath'
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

      if (this.refundPaymentTransactionItemRequestType) {
        customListUrl += `refundPaymentTransactionItemRequestType=${this.refundPaymentTransactionItemRequestType}&`;
      }

      if (this.refundPaymentTransactionItemRequestStatus) {
        customListUrl += `refundPaymentTransactionItemRequestStatus=${this.refundPaymentTransactionItemRequestStatus}&`;
      }


    }

    let res = false;
    if (this.parentId || this.yashilHekmatTerminalId || this.refundPaymentTransactionItemRequestType || this.refundPaymentTransactionItemRequestStatus) {


      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();

      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (<RefundPaymentTransactionItemRequestDetailComponent>componentInstance.instance);

    if (this.parentId) {
      comp.parentId = this.parentId;
    }

    if (this.yashilHekmatTerminalId) {
      comp.yashilHekmatTerminalId = this.yashilHekmatTerminalId;
    }

    if (this.refundPaymentTransactionItemRequestType) {
      comp.refundPaymentTransactionItemRequestType = this.refundPaymentTransactionItemRequestType;
    }

    if (this.refundPaymentTransactionItemRequestStatus) {
      comp.refundPaymentTransactionItemRequestStatus = this.refundPaymentTransactionItemRequestStatus;
    }

  }
}

