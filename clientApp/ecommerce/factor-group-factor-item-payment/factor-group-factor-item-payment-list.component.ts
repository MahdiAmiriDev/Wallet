import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {YashilCustomButton} from '../../../shared/base/models/yasil-custom-button';

@Component({
  selector: 'app-factor-group-factor-item-payment-list',
  templateUrl: './factor-group-factor-item-payment-list.component.html'
})
export class FactorGroupFactorItemPaymentListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'payment';
  customButtons: YashilCustomButton[];
  @Input()
  gridHeight: number;
  private baseListUrl = 'factor/GetFactorItemPaymentFullInfoViewList?';

  constructor() {
    super();
  }

  // tslint:disable-next-line:variable-name
  _factorGroupId: number;

  get factorGroupId(): number {
    return this._factorGroupId;
  }

  @Input()
  set factorGroupId(val) {
    if (val !== this._factorGroupId) {
      this._factorGroupId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _orderItemId: number;

  get orderItemId(): number {
    return this._orderItemId;
  }

  @Input()
  set orderItemId(val) {
    if (val !== this._orderItemId) {
      this._orderItemId = val;
    }
  }

  ngOnInit(): void {
    if (!this.gridHeight) {
      this.gridHeight = window.innerHeight - 170;
    }
    this.bindColumns();
    this.bindCustomButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.factorGroupId) {
        customListUrl += `factorGroupId=${this.factorGroupId}&`;
      }
    }
    let res = false;
    if (this.factorGroupId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  bindCustomButtons(): void {
    this.customButtons = [{
      text: 'دانلود گزارش',
      type: 'default',
      icon: 'xlsfile',
      key: this.factorGroupId
    }];
  }

  downloadFactorGroupFactorItemPaymentReport(factorGroupId: any): void {
    const url = `${window.location.origin}/api/factorGroup/GenerateFactorGroupFactorItemPaymentReportFile?factorGroupId=${factorGroupId}`;
    window.open(url, '_blank');
  }

  private bindColumns(): void {
    this.columns.push({
      width: '100px',
      caption: 'کد رهگیری تراکنش',
      allowHeaderFiltering: false,
      dataField: 'trackingCode'
    }, {
      width: '100px',
      caption: 'کدملی',
      allowHeaderFiltering: false,
      dataField: 'nationalCode'
    }, {
      width: '50px',
      caption: 'تعداد اقساط',
      allowHeaderFiltering: false,
      dataField: 'installmentCount'
    }, {
      width: '100px',
      caption: 'سفارش',
      allowHeaderFiltering: false,
      dataField: 'orderNumber'
    }, {
      width: '160px',
      caption: 'فاکتور',
      dataField: 'factorCode'
    }, {
      width: '180px',
      caption: 'نام محصول',
      allowHeaderFiltering: false,
      dataField: 'productTitle'
    }, {
      width: '150px',
      caption: 'نام مرکز',
      dataField: 'companyTitle'
    }, {
      width: '150px',
      caption: 'نام نمایندگی',
      dataField: 'representationTitle'
    }, {
      width: '130px',
      allowHeaderFiltering: false,
      caption: 'شماره حکمت یا شماره مرجع',
      dataField: 'transactionNumber'
    }, {
      width: '110px',
      caption: 'مبلغ تراکنش',
      allowHeaderFiltering: false,
      dataField: 'paymentTotal',
      cellTemplate: 'number'
    }, {
      width: '110px',
      caption: 'مبلغ اصلاحی تراکنش',
      allowHeaderFiltering: false,
      dataField: 'newPaymentAmount',
      cellTemplate: 'number'
    }, {
      width: '110px',
      allowHeaderFiltering: false,
      caption: 'مبلغ کل با مالیات',
      dataField: 'totalIncludeTax',
      cellTemplate: 'number'
    }, {
      width: '110px',
      allowHeaderFiltering: false,
      caption: 'مبلغ کل بدون مالیات',
      dataField: 'totalExclTax',
      cellTemplate: 'number'
    }, {
      width: '100px',
      caption: 'یارانه',
      allowHeaderFiltering: false,
      dataField: 'subsidyPrice',
      cellTemplate: 'number'
    }, {
      width: '100px',
      caption: 'یارانه وزارت',
      allowHeaderFiltering: false,
      dataField: 'vezaratSubsidy',
      cellTemplate: 'number'
    }, {
      width: '100px',
      allowHeaderFiltering: false,
      caption: 'یارانه وزارت با کارمزد',
      dataField: 'vezaratSubsidyPriceIncProfit',
      cellTemplate: 'number'
    }, {
      width: '100px',
      allowHeaderFiltering: false,
      caption: 'یارانه وزارت با کارمزد و مالیات',
      dataField: 'vezaratSubsidyPriceIncTaxIncProfit',
      cellTemplate: 'number'
    }, {
      width: '100px',
      caption: 'یارانه ساتا',
      allowHeaderFiltering: false,
      dataField: 'sataSubsidy',
      cellTemplate: 'number'
    }, {
      width: '100px',
      caption: 'یارانه ساتا با کارمزد',
      allowHeaderFiltering: false,
      dataField: 'sataSubsidyPriceIncProfit',
      cellTemplate: 'number'
    }, {
      width: '100px',
      caption: 'یارانه ساتا با کارمزد و مالیات',
      allowHeaderFiltering: false,
      dataField: 'sataSubsidyPriceIncTaxIncProfit',
      cellTemplate: 'number'
    }, {
      width: '100px',
      caption: 'قیمت پایه با لحاظ تعداد',
      allowHeaderFiltering: false,
      dataField: 'price',
      cellTemplate: 'number'
    }, {
      caption: 'تاریخ خرید',
      allowHeaderFiltering: false,
      dataField: 'paidDateInt',
      cellTemplate: 'intDate',
      width: '120px'
    });

  }

}
