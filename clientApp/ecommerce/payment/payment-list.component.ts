import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {PaymentDetailComponent} from './payment-detail.component';
import {PaymentStatusTypeEnum} from './payment-status-enums';


@Component({
  selector: 'app-payment-list',
  templateUrl: './payment-list.component.html'
})
export class PaymentListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'payment';
  detailComponent = PaymentDetailComponent;
  // tslint:disable-next-line:variable-name
  _orderId: number;
  @Input()
  fixHeight = true;

  @Input()
  set orderId(val) {
    if (val !== this._orderId) {
      this._orderId = val;
    }
  }

  get orderId(): number {
    return this._orderId;
  }

  // tslint:disable-next-line:variable-name
  _cardType: number;
  @Input()
  set cardType(val) {
    if (val !== this._cardType) {
      this._cardType = val;
    }
  }

  get cardType(): number {
    return this._cardType;
  }

  // tslint:disable-next-line:variable-name
  _saleType: number;
  @Input()
  set saleType(val) {
    if (val !== this._saleType) {
      this._saleType = val;
    }
  }

  get saleType(): number {
    return this._saleType;
  }

  // tslint:disable-next-line:variable-name
  _paymentStatusType: number;
  @Input()
  set paymentStatusType(val) {
    if (val !== this._paymentStatusType) {
      this._paymentStatusType = val;
    }
  }

  get paymentStatusType(): number {
    return this._paymentStatusType;
  }

  // tslint:disable-next-line:variable-name
  _paymentMethodType: number;
  @Input()
  set paymentMethodType(val) {
    if (val !== this._paymentMethodType) {
      this._paymentMethodType = val;
    }
  }

  get paymentMethodType(): number {
    return this._paymentMethodType;
  }

  private baseListUrl = 'payment/GetByCustomFilterForList?';

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
    this.columns.push({
      caption: 'شماره پیگیری تراکنش',
      allowHeaderFiltering: false,
      dataField: 'authorizationTransactionId'
    }, {
      caption: 'نوع پرداخت',
      dataField: 'saleTypeTitle',
    }, {
      caption: 'وضعیت پرداخت',
      dataField: 'paymentStatusTypeTitle'
    }, {
      caption: 'نتیجه تراکنش پرداخت',
      allowHeaderFiltering: false,
      dataField: 'authorizationTransactionResult'
    }, {
      caption: ' روش پرداخت',
      dataField: 'paymentMethodTypeTitle'
    }, {
      caption: 'تعداد اقساط',
      dataField: 'installmentCount'
    }, {
      caption: 'تاریخ ثبت پرداخت',
      dataField: 'creationDate',
      allowHeaderFiltering: false,
      cellTemplate: 'jalaliDate'
    }, {
      caption: 'تاریخ تایید پرداخت',
      dataField: 'paidDateUtc',
      allowHeaderFiltering: false,
      cellTemplate: 'jalaliDate'
    }, {
      caption: 'مبلغ پرداخت شده',
      dataField: 'payTotal',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
    }, {
      caption: 'مبلغ جدید اصلاح شده',
      dataField: 'newPaymentAmount',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
    }, {
      caption: 'مبلغ عودت',
      dataField: 'sumRefundAmount',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
    });
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.orderId) {
        customListUrl += `orderId=${this.orderId}&`;
      }
      if (this.cardType) {
        customListUrl += `cardType=${this.cardType}&`;
      }
      if (this.saleType) {
        customListUrl += `saleType=${this.saleType}&`;
      }
      if (this.paymentStatusType) {
        customListUrl += `paymentStatusType=${this.paymentStatusType}&`;
      }
      if (this.paymentMethodType) {
        customListUrl += `paymentMethodType=${this.paymentMethodType}&`;
      }
    }
    let res = false;
    if (this.orderId || this.cardType || this.saleType || this.paymentStatusType || this.paymentMethodType) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as PaymentDetailComponent);

    if (this.orderId) {
      comp.orderId = this.orderId;
    }

    if (this.cardType) {
      comp.cardType = this.cardType;
    }

    if (this.saleType) {
      comp.saleType = this.saleType;
    }

    if (this.paymentStatusType) {
      comp.paymentStatusType = this.paymentStatusType;
    }

    if (this.paymentMethodType) {
      comp.paymentMethodType = this.paymentMethodType;
    }

  }


  onListRowPrepared(e): void {
    if (e.rowType === 'data') {
      if (e.data.paymentStatusType === PaymentStatusTypeEnum.Successfull) {
        e.rowElement.bgColor = '#60d394';
        e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
      } else {
        e.rowElement.bgColor = '#ee6050';
        e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
      }
    }
  }
}
