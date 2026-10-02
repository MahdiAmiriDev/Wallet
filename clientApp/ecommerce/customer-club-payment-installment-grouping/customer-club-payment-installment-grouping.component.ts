import {Component, OnInit} from '@angular/core';
import {customAnimations} from '../../../shared/animations/custom-animations';
import {YashilComponent} from '../../../core/baseClasses/yashilComponent';

@Component({
  selector: 'app-customer-club-payment-installment-grouping',
  templateUrl: './customer-club-payment-installment-grouping.component.html',
  animations: customAnimations
})
export class CustomerClubPaymentInstallmentGroupingComponent extends YashilComponent implements OnInit {
  columns: any[] = [];

  ngOnInit(): void {
    this.columns = [
      {
        caption: 'نام طرح',
        dataField: 'applicationTitle',
        width: '100px',
      }, {
        caption: 'کد پرداخت',
        dataField: 'reservationNumber',
        width: '100px',
      }, {
        caption: 'مبلغ هر قسط',
        dataField: 'installmentAmount',
        cellTemplate: 'number',
        width: '100px',
      }, {
        caption: 'تعداد اقساط',
        dataField: 'installmentCount',
        width: '100px',
      }, {
        caption: 'شماره قسط',
        dataField: 'installmentNumber',
        width: '100px',
      }, {
        caption: 'شماره کارت',
        dataField: 'cardNumber',
        width: '150px',
      }, {
        caption: 'کد ملی صاحب حکمت',
        dataField: 'customerClubCardCode',
        width: '150px',
      }, {
        caption: 'سال',
        dataField: 'yearNumber',
        width: '150px',
      }, {
        caption: 'ماه',
        dataField: 'monthTitle',
        width: '150px',
      }, {
        caption: 'تاریخ ثبت پرداخت',
        dataField: 'paidDateInt',
        cellTemplate: 'intDate',
        width: '110px'
      }];
  }

}
