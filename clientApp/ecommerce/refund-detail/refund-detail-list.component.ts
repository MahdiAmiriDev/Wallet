import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';


@Component({
  selector: 'app-refund-detail-list',
  templateUrl: './refund-detail-list.component.html'
})
export class RefundDetailListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'refundDetail';
  // tslint:disable-next-line:variable-name
  private _orderId: number;
  @Input()
  set orderId(value: number) {
    this._orderId = value;
  }

  get orderId(): number {
    return this._orderId;
  }

  // tslint:disable-next-line:variable-name
  _refundType: number;
  @Input()
  set refundType(val) {
    if (val !== this._refundType) {
      this._refundType = val;
    }
  }

  get refundType(): number {
    return this._refundType;
  }

  // tslint:disable-next-line:variable-name
  _refundStatusType: number;
  @Input()
  set refundStatusType(val) {
    if (val !== this._refundStatusType) {
      this._refundStatusType = val;
    }
  }

  get refundStatusType(): number {
    return this._refundStatusType;
  }

  // tslint:disable-next-line:variable-name
  _refundId: number;
  @Input()
  set refundId(val) {
    if (val !== this._refundId) {
      this._refundId = val;
    }
  }

  get refundId(): number {
    return this._refundId;
  }

  // tslint:disable-next-line:variable-name
  _refundDetailGroupId: number;
  @Input()
  set refundDetailGroupId(val) {
    if (val !== this._refundDetailGroupId) {
      this._refundDetailGroupId = val;
    }
  }

  get refundDetailGroupId(): number {
    return this._refundDetailGroupId;
  }

  // tslint:disable-next-line:variable-name
  _refundDetailForPaymentGroupId: number;
  @Input()
  set refundDetailForPaymentGroupId(val) {
    if (val !== this._refundDetailForPaymentGroupId) {
      this._refundDetailForPaymentGroupId = val;
    }
  }

  get refundDetailForPaymentGroupId(): number {
    return this._refundDetailForPaymentGroupId;
  }

  // tslint:disable-next-line:variable-name
  _paymentId: number;
  @Input()
  set paymentId(val) {
    if (val !== this._paymentId) {
      this._paymentId = val;
    }
  }

  get paymentId(): number {
    return this._paymentId;
  }

  // tslint:disable-next-line:variable-name
  _personBankAccountId: number;
  @Input()
  set personBankAccountId(val) {
    if (val !== this._personBankAccountId) {
      this._personBankAccountId = val;
    }
  }

  get personBankAccountId(): number {
    return this._personBankAccountId;
  }

  // tslint:disable-next-line:variable-name
  _orderItemId: number;
  @Input()
  set orderItemId(val) {
    if (val !== this._orderItemId) {
      this._orderItemId = val;
    }
  }

  get orderItemId(): number {
    return this._orderItemId;
  }

  private baseListUrl = 'refundDetail/GetByCustomFilterForList?';
  @Input()
  showAddButton = true;
  @Input()
  showEditInform = true;
  @Input()
  allowDeleteInForm = true;
  @Input()
  fixHeight = true;
  @Input()
  allowEditInForm = true;

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  bindColumns(): void {
    this.columns.push({
      caption: 'نوع عودت وجه',
      dataField: 'refundTypeTitle',
      width: '140px'
    }, {
      caption: 'وضعیت عودت وجه',
      dataField: 'refundStatusTypeTitle',
      width: '130px'
    }, {
      caption: 'شناسه پرداخت',
      dataField: 'reservationNumber',
      allowSorting: false,
      width: '150px'
    }, {
      caption: 'مبلغ تراکنش ارسالی(یال)',
      dataField: 'oldPaymentAmount',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '150px'
    }, {
      caption: 'مبلغ تراکنش نهایی(ریالی)',
      dataField: 'newPaymentAmount',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '150px'
    }, {
      caption: 'مبلغ عودت جهت پرداخت',
      dataField: 'refundAmount',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '150px'
    }, {
      caption: 'گروه بندی عودت وجه',
      dataField: 'refundDetailGroupTitle',
      allowSorting: false,
      allowHeaderFiltering: false,
      width: '190px'
    }, {
      caption: 'گروه بندی پرداخت عودت وجه',
      dataField: 'refundDetailForPaymentGroupTitle',
      allowSorting: false,
      allowHeaderFiltering: false,
      width: '190px'
    }, {
      caption: 'تاریخ بازپرداخت',
      dataField: 'refundTime',
      cellTemplate: 'jalaliDate',
      width: '120px'
    }, {
      caption: 'تایید شده',
      dataField: 'confirmed',
      width: '80px'
    });
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.refundType) {
        customListUrl += `refundType=${this.refundType}&`;
      }
      if (this.refundStatusType) {
        customListUrl += `refundStatusType=${this.refundStatusType}&`;
      }
      if (this.refundId) {
        customListUrl += `refundId=${this.refundId}&`;
      }
      if (this.refundDetailGroupId) {
        customListUrl += `refundDetailGroupId=${this.refundDetailGroupId}&`;
      }
      if (this.refundDetailForPaymentGroupId) {
        customListUrl += `refundDetailForPaymentGroupId=${this.refundDetailForPaymentGroupId}&`;
      }
      if (this.paymentId) {
        customListUrl += `paymentId=${this.paymentId}&`;
      }
      if (this.personBankAccountId) {
        customListUrl += `personBankAccountId=${this.personBankAccountId}&`;
      }
      if (this.orderItemId) {
        customListUrl += `orderItemId=${this.orderItemId}&`;
      }
      if (this.orderId) {
        customListUrl += `orderId=${this.orderId}&`;
      }
    }
    let res = false;
    if (this.refundType || this.refundStatusType
      || this.refundId || this.refundDetailGroupId
      || this.refundDetailForPaymentGroupId || this.paymentId
      || this.personBankAccountId || this.orderItemId || this.orderId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }
}
