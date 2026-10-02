import {AfterViewInit, Component, OnInit} from '@angular/core';
import {customAnimations} from '../../../shared/animations/custom-animations';
import {YashilComponent} from '../../../core/baseClasses/yashilComponent';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';

@Component({
  selector: 'app-refund-detail-for-payment-grouping',
  templateUrl: './refund-detail-for-payment-grouping.component.html',
  animations: customAnimations
})
export class RefundDetailForPaymentGroupingComponent extends YashilComponent implements OnInit {
  columns: any[] = [];
  customRowButtons: YashilCustomButtonColumn;
  showPopUp: boolean;

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.columns = [
      {
        caption: 'کد ملی',
        dataField: 'nationalCode',
        width: '100px',
      }, {
        caption: 'شماره تماس',
        dataField: 'mobileNumber',
        width: '100px',
      }, {
        caption: 'نام طرح',
        dataField: 'applicationName',
        width: '80px',
      }, {
        caption: 'تاریخ پرداخت',
        dataField: 'paidDateInt',
        cellTemplate: 'intDate',
        width: '100px',
      },
      {
        caption: 'سهم خانوار',
        dataField: 'oldPaymentAmount',
        cellTemplate: 'number',
        width: '100px',
      },
      {
        caption: 'مبلغ عودتی به کاربر',
        dataField: 'refundAmount',
        cellTemplate: 'number',
        width: '100px',
      }, {
        caption: 'شماره فیش/حکمت',
        dataField: 'cardNumber',
        width: '150px',
      }, {
        caption: 'شماره شبا',
        dataField: 'shebaNumber',
        width: '220px',
      },
      {
        caption: 'نام بانک',
        dataField: 'bankTypeTitle',
        width: '80px',
      },
      {
        caption: 'نام شعبه',
        dataField: 'branchName',
        width: '80px',
      },
      {
        caption: 'نام صاحب حساب',
        dataField: 'bankAccountOwnerFullName',
        width: '150px',
      },
      {
        caption: 'نام کاربر',
        dataField: 'refundCreatorName',
        width: '150px',
      }];
  }
}
