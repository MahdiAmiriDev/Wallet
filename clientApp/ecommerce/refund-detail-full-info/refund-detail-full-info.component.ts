import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {Selectable} from '../../../shared/base/classes/selectable';
import * as moment from 'jalali-moment';
import {FormControl} from '@angular/forms';

@Component({
  selector: 'app-refund-detail-full-info-list',
  templateUrl: './refund-detail-full-info.component.html'
})
export class RefundDetailFullInfoComponent extends Selectable implements OnInit {
  refundCreationDateFilterFromDateDataValue = new FormControl();
  refundCreationDateFilterToDateDataValue = new FormControl();
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'refundDetail';
  datePickerConfig = {
    format: 'jYYYY/jMM/jDD',
    dir: 'rtl',
    locale: moment.locale('fa')
  };
  refundCreationDateFilterFromDate: number;
  refundCreationDateFilterToDate: number;
  showGrouped = false;
  showPaymentGrouped = false;
  showHasBankAccount = false;


  private baseListUrl = 'refundDetail/getRefundDetailFullInfo?';
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
    this.refundCreationDateFilterFromDate = this.convertMomentDateToInt(moment().startOf('year'));
    this.refundCreationDateFilterToDate = this.convertMomentDateToInt(moment());
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  bindColumns(): void {
    this.columns.push({
      caption: 'نوع عودت وجه',
      dataField: 'refundTypeTitle',
      width: '80px'
    }, {
      caption: 'تاریخ ثبت عودت وجه',
      dataField: 'createDate',
      cellTemplate: 'jalaliDate',
      width: '100px'
    }, {
      caption: 'کد پیگیری سفارش',
      dataField: 'orderTitle',
      allowSorting: false,
      width: '100px'
    }, {
      caption: 'شماره کارت',
      dataField: 'cardNumber',
      allowSorting: false,
      width: '150px'
    }, {
      caption: 'مبلغ تراکنش ارسالی(یال)',
      dataField: 'oldPaymentAmount',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '100px'
    }, {
      caption: 'مبلغ تراکنش نهایی(ریالی)',
      dataField: 'newPaymentAmount',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '100px'
    }, {
      caption: 'مبلغ عودت جهت پرداخت',
      dataField: 'refundAmount',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '100px'
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
      caption: 'اطلاعات حساب دریافت شده',
      dataField: 'isSetPersonBankAccount',
      width: '80px'
    }, {
      caption: 'تایید شده',
      dataField: 'confirmed',
      width: '80px'
    });
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.showGrouped) {
        customListUrl += `showGrouped=${this.showGrouped}&`;
      }
      if (this.showPaymentGrouped) {
        customListUrl += `showPaymentGrouped=${this.showPaymentGrouped}&`;
      }
      if (this.showHasBankAccount) {
        customListUrl += `showHasBankAccount=${this.showHasBankAccount}&`;
      }
      if (this.refundCreationDateFilterFromDate) {
        customListUrl += `refundCreationDateFromDate=${this.refundCreationDateFilterFromDate}&`;
      }
      if (this.refundCreationDateFilterToDate) {
        customListUrl += `refundCreationDateToDate=${this.refundCreationDateFilterToDate}&`;
      }
    }
    let res = false;
    if (this.showGrouped || this.showPaymentGrouped || this.showHasBankAccount
      || this.refundCreationDateFilterFromDate || this.refundCreationDateFilterToDate) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }


}
