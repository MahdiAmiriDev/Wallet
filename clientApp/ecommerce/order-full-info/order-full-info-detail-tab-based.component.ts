import {Component, Input, OnInit} from '@angular/core';


@Component({
  selector: 'app-order-full-info-detail-tab-based',
  templateUrl: './order-full-info-detail-tab-based.component.html'
})
export class OrderFullInfoDetailTabBasedComponent implements OnInit {

  @Input()
  fixHeight = true;


  tabSimpleComponentGridHeight: number;
  issueLogWorkList: number;
  tabs: any[] = [];
  formHeight: number;
  selectedIndex: number;

  constructor() {
    this.tabSimpleComponentGridHeight = window.innerHeight - 415;
    this.issueLogWorkList = window.innerHeight - 550;
    if (this.fixHeight) {
      this.formHeight = window.innerHeight - 320;
    }
  }

  // tslint:disable-next-line:variable-name
  private _orderTitle: string;

  get orderTitle(): string {
    return this._orderTitle;
  }

  @Input()
  set orderTitle(value: string) {
    this._orderTitle = value;
  }

  // tslint:disable-next-line:variable-name
  private _orderId: number;

  get orderId(): number {
    return this._orderId;
  }

  @Input()
  set orderId(value: number) {
    if (value !== this.orderId) {
      this._orderId = value;
      this.bindTabs();
    }
  }

  ngOnInit(): void {
  }

  bindTabs(): void {
    this.selectedIndex = 0;
    this.tabs = [];
    if (this.orderId && this.orderId !== 0) {
      this.tabs.push({id: 1, title: 'آیتم های سفارش', template: 'orderItems'});
      this.tabs.push({id: 2, title: 'پرداختی های سفارش', template: 'orderPayments'});
      this.tabs.push({id: 3, title: 'عودت وجه های سفارش', template: 'refundDetails'});
      this.tabs.push({id: 4, title: 'ثبت و نمایش پیگیری', template: 'issueLogWorks'});
    }
  }

  refreshIssueLogWorkList(item: any): void {

  }
}


