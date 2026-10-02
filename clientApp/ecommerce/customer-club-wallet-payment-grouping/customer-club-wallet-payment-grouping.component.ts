import {AfterViewInit, Component, OnInit} from '@angular/core';
import {customAnimations} from '../../../shared/animations/custom-animations';
import {YashilComponent} from '../../../core/baseClasses/yashilComponent';

@Component({
  selector: 'app-customer-club-wallet-payment-grouping',
  templateUrl: './customer-club-wallet-payment-grouping.component.html',
  animations: customAnimations
})
export class CustomerClubWalletPaymentGroupingComponent extends YashilComponent implements OnInit {
  columns: any[] = [];

  ngOnInit(): void {
    this.columns = [{
      caption: 'کد پرداخت',
      dataField: 'reservationNumber',
      width: '100px',
    }, {
      caption: 'کد سفارش',
      dataField: 'orderTitle',
      width: '100px',
    }, {
      caption: 'نام طرح',
      dataField: 'applicationName',
      width: '100px',
    }, {
      caption: 'کدملی داوطلب',
      dataField: 'nationalCode',
      width: '100px',
    }, {
      caption: 'نام محصول',
      dataField: 'productTitle',
      width: '150px',
    }, {
      caption: 'استان',
      dataField: 'provinceTitle',
      width: '100px',
    }, {
      caption: 'شهر',
      dataField: 'cityTitle',
      width: '100px',
    }, {
      caption: 'شماره کارت',
      dataField: 'cardNumber',
      width: '150px',
    }, {
      caption: 'مبلغ(ریال)',
      dataField: 'total',
      cellTemplate: 'number',
      width: '150px',
    }, {
      caption: 'تاریخ ثبت پرداخت',
      dataField: 'creationDate',
      cellTemplate: 'jalaliDate',
      width: '110px',
    }];
  }

}
