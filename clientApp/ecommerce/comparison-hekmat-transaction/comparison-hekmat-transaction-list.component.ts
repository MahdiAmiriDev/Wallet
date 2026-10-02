import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';


@Component({
  selector: 'app-comparison-hekmat-transaction-list',
  templateUrl: './comparison-hekmat-transaction-list.component.html'
})
export class ComparisonHekmatTransactionComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  columns: any[] = [];
  formHeight: number;
  // tslint:disable-next-line:variable-name
  private _factorId: number;
  customRowButtons: YashilCustomButtonColumn;
  paymentId: number;
  popUpTitle: string;
  showPopUp = false;

  @Input()
  set factorId(value: number) {
    if (value !== this.factorId) {
      this._factorId = value;
      this.bindCustomDataSources();
    }
  }

  get factorId(): number {
    return this._factorId;
  }

  private baseListUrl = 'factor/PaymentComparisonWithHekmatApprovedTransactionByFactorCode?';
  // جهت نمایش مغایرت ها
  showContrast = false;

  constructor() {
    super();
    this.loadAfterSetFilter = true;
    this.showRefundsForPayments = this.showRefundsForPayments.bind(this);

  }

  ngOnInit(): void {
    this.bindColumns();
    this.bindCustomDataSources();
  }


  private bindColumns(): void {
    this.columns.push({
        caption: 'روش پرداخت',
        dataField: 'paymentMethodTitle',
        allowHeaderFiltering: false,
        width: '150px'
      },
      {
        caption: 'کدرهگیری پذیرنده',
        dataField: 'installmentTrackingCode',
        allowHeaderFiltering: false,
        width: '80px'
      },
      {
        caption: 'کدرهگیری پذیرنده در سی دی',
        dataField: 'hekmatInstallmentTrackingCode',
        allowHeaderFiltering: false,
        width: '80px'
      },
      {
        caption: 'شماره کارت',
        dataField: 'cardNumber',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '140px'
      },
      {
        caption: 'شماره کارت حکمت در سی دی',
        dataField: 'hekmatCardNumber',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '140px'
      },
      {
        caption: 'شماره مرجع',
        dataField: 'customerRefNum',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '130px'
      },
      {
        caption: 'تاریخ پرداخت',
        dataField: 'paymentPaidDateInt',
        cellTemplate: 'intDate',
        allowHeaderFiltering: false,
        width: '110px'
      },
      {
        caption: 'تاریخ پرداخت حکمت در سی دی',
        dataField: 'hekmatTransactionDate',
        cellTemplate: 'intDate',
        allowHeaderFiltering: false,
        width: '110px'
      },
      {
        caption: ' مبلغ پرداخت',
        dataField: 'paymentTotalInt',
        cellTemplate: 'number',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '120px'
      },
      {
        caption: 'مبلغ پرداخت حکمت در سی دی',
        dataField: 'hekmatPrice',
        cellTemplate: 'number',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '120px'
      },
      {
        caption: 'تعداد قسط',
        dataField: 'installmentCount',
        cellTemplate: 'number',
        allowHeaderFiltering: false,
        width: '60px'
      },
      {
        caption: 'عنوان پروژه',
        dataField: 'applicationTitle',
        allowHeaderFiltering: false,
        width: '100px'
      },
      {
        caption: 'مغایرت قیمت',
        dataField: 'isNotSamePrice',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '80px'
      },
      {
        caption: 'مغایرت شماره کارت',
        dataField: 'isNotSameCardNumber',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '80px'
      },
      {
        caption: 'مغایرت کدرهگیری پذیرنده',
        dataField: 'isNotSameInstallmentTrackingCode',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '80px'
      }
    );
    this.customRowButtons = {
      width: '150px',
      allowExporting: false,
      setAtFirstColumn: true,
      customButtons: [
        {
          text: 'عودت وجه پرداخت',
          hint: 'عودت وجه های مربوط به پرداخت',
          icon: 'money',
          type: 'default',
          stylingMode: 'contained',
          visibleProp: 'allowShowRefundForPaymentBtn',
          onClick: this.showRefundsForPayments
        }]
    };
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this._factorId) {
        customListUrl += `factorId=${this._factorId}&`;
      }
    }
    customListUrl += `showContrast=${this.showContrast}&`;
    let res = false;
    if (this._factorId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }


  /*
  درصورتی که تراکنش های فاکتور در سی دی نباشد پیام به کاربر نمایش داده می شود
   */
  onListRowPrepared(e): void {
    if (e.rowType === 'data') {
      if (e.data.isNotSameCardNumber || e.data.isNotSameInstallmentTrackingCode
        || e.data.isNotSamePrice || e.data.notExistInHekmat) {
        e.rowElement.bgColor = '#ec5252';
        e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
      }
    }
  }

  /*
  جهت نمایش عودت وجه های پردخت
   */
  showRefundsForPayments(item: any): void {
    // مقداردهی پارامترهای ورودی
    this.paymentId = item.row.data.paymentId;
    this.popUpTitle = 'عودت وجه های مربوط به پرداخت';
    this.showPopUp = true;
  }


}
