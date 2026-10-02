import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HekmatPaymentGatewayTransactionDetailComponent} from './hekmat-payment-gateway-transaction-detail.component';
import * as moment from 'jalali-moment';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-hekmat-payment-gateway-transaction-list',
  templateUrl: './hekmat-payment-gateway-transaction-list.component.html'
})
export class HekmatPaymentGatewayTransactionListComponent extends Selectable implements OnInit {
  fromDateFilterDataValue = new FormControl();
  toDateFilterDataValue = new FormControl();
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  fromDateFilter: number;
  toDateFilter: number;
  columns: any[] = [];
  selectedStatus: number;
  statusCode: number;
  statusDataSource = [{
    title: 'همه موارد'
  }, {
    id: 0,
    title: 'در انتظار بررسی'
  }, {
    id: 1,
    title: 'تایید شده'
  },
    {
      id: 2,
      title: 'عدم تایید'
    }];
  entityName = 'hekmatPaymentGatewayTransaction';
  detailComponent = HekmatPaymentGatewayTransactionDetailComponent;
  // tslint:disable-next-line:variable-name
  _terminalId: number;
  datePickerConfig = {
    format: 'jYYYY/jMM/jDD',
    dir: 'rtl',
    locale: moment.locale('fa')
  };

  @Input()
  set terminalId(val) {
    if (val !== this._terminalId) {
      this._terminalId = val;
    }
  }

  get terminalId(): number {
    return this._terminalId;
  }

  // tslint:disable-next-line:variable-name
  _merchantId: number;
  @Input()
  set merchantId(val) {
    if (val !== this._merchantId) {
      this._merchantId = val;
    }
  }

  get merchantId(): number {
    return this._merchantId;
  }

  private baseListUrl = 'hekmatPaymentGatewayTransaction/GetByCustomFilterForList?';

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.fromDateFilter = this.convertMomentDateToInt(moment().startOf('year'));
    this.toDateFilter = this.convertMomentDateToInt(moment());
    this.gridHeight = window.innerHeight - 190;
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push(
      {
        caption: 'شماره پیگیری دارنده (شماره پیگیری درگاه)',
        dataField: 'hekmatReservationNumber',
        width: '100px'
      },
      {
        caption: 'شماره سفارش پذیرنده',
        dataField: 'localReservationNumber',
        width: '100px'
      },
      {
        caption: 'نام پذیرنده',
        dataField: 'merchantTitle',
        width: '100px'
      },
      {
        caption: 'نام ترمینال حکمت',
        dataField: 'hekmatTerminalTitle',
        width: '100px'
      },
      {
        caption: 'شماره حکمت کارت',
        dataField: 'hekmatCardNumber',
        width: '130px'
      },
      {
        caption: 'کد ملی',
        dataField: 'nationalCode',
        width: '100px'
      },
      {
        caption: 'قیمت',
        dataField: 'price',
        cellTemplate: 'number',
        width: '100px'
      },
      {
        caption: 'تعداد اقساط',
        dataField: 'installmentCount',
        width: '70px'
      },
      {
        caption: 'وضعیت',
        dataField: 'statusCode',
        width: '70px'
      },
      {
        caption: 'کد خطا',
        dataField: 'errorCode',
        width: '70px'
      },
      {
        caption: 'تاریخ ثبت تراکنش',
        dataField: 'creationDateInt',
        cellTemplate: 'intDate',
        width: '110px'
      },
      {
        caption: 'تاریخ تعیین تکلیف تراکنش',
        dataField: 'approveDateInt',
        cellTemplate: 'intDate',
        width: '110px'
      },
      {
        caption: 'شماره همراه',
        dataField: 'mobile',
        width: '120px'
      },
      {
        caption: 'شماره تلفن',
        dataField: 'tell',
        width: '120px'
      },


      {
        caption: 'کد قدیمی درگاه پرداخت',
        dataField: 'oldHekmatId',
        width: '150px'
      },
      {
        caption: 'فاکتور شده؟',
        dataField: 'hasBeenInvoiced'
      },
      {
        caption: 'شناسه فاکتور',
        dataField: 'factorId',
        width: '120px'
      },
      {
        caption: 'پیغام نتیجه تراکنش',
        dataField: 'hekmatTransactionResultMessage',
        width: '250px'
      }
    );
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.merchantId) {
        customListUrl += `merchantId=${this.merchantId}&`;
      }
      if (this.terminalId) {
        customListUrl += `yashilHekmatTerminalId=${this.terminalId}&`;
      }
      if (this.fromDateFilter) {
        customListUrl += `fromDate=${this.fromDateFilter}&`;
      }
      if (this.toDateFilter) {
        customListUrl += `toDate=${this.toDateFilter}&`;
      }
      if (this.statusCode || this.statusCode >= 0) {
        customListUrl += `statusCode=${this.statusCode}&`;
      }
    }
    let res = false;
    if (this.merchantId || this.terminalId || this.fromDateFilter || this.toDateFilter || this.statusCode || this.statusCode >= 0) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as HekmatPaymentGatewayTransactionDetailComponent);
    if (this.terminalId) {
      comp.hekmatTerminalId = this.terminalId;
    }
  }

  onSelectedMerchantChange(item: any): void {
    if (item && item.merchant && item.merchant.id) {
      this.merchantId = item.merchant.id;
      this.terminalId = null;
      this.bindCustomDataSources();
    }
  }

  onSelectedTerminalChange(item: any): void {
    if (item && item.terminal && item.terminal.id) {
      this.terminalId = item.terminal.id;
      this.bindCustomDataSources();
    }
  }


  onSelectedStatusChanged(item: any): void {
    if (item && item.selectedItem) {
      this.statusCode = item.selectedItem.id;
      this.bindCustomDataSources();
    }
  }
}
