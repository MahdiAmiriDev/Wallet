import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {YashilComponent} from '../../../core/baseClasses/yashilComponent';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {IBusyIndicator} from '../../../core/interfaces';
import {OrderStatusTypeEnum} from '../order-item-full-info/order-status-enums';
import {SubsidyStatusTypeEnum} from '../subsidy/subsidy-status-enums';


@Component({
  selector: 'app-order-statistic',
  templateUrl: './order-statistic.component.html'
})
export class OrderStatisticComponent extends YashilComponent implements OnInit {
  @ViewChild('busyIndicatorComponent', {static: true}) busyIndicator: IBusyIndicator;
  // سفارش تایید شده است یا خیر
  orderConfirmed = false;
  // سفارش پرداخت ناقص است یا خیر
  orderIncompletePayment = false;
  // یارانه تایید شده است یا خیر
  orderSubsidyConfirmed = false;
  // سفارش یارانه دارد یا خیر
  orderHasSubsidy = false;
  entityName: string;
  entity: any = {};

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'order';
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
      this.entity = {};
      this.loadData();
    }
  }

  ngOnInit(): void {
    if (this.orderId) {
      this.entity.orderId = this.orderId;
    }
    // this.loadData();
  }

  /*
  دریافت اطلاعات
   */
  loadData(): void {
    if (this.orderId) {
      this.busyIndicator.setBusy(true);
      let url = 'GetOrderStatistic?';
      url = url + `orderId=${this.orderId}&`;
      this.genericDataService.getEntity(this.entityName, this.orderId, url)
        .subscribe(
          res => {
            this.busyIndicator.setBusy(false);
            if (res) {
              this.entity = res;
              // بررسی وضعیت سفارش
              /*
              اگر سفارش موفق باشد orderConfirmed مقدار true میگیرد و از آن برای تغییر رنگ سبز وضعیت سفارش استفاده میشود
              و اگر مقدار false داشته باشد زمینه وضعیت سفارش قرمز میشود
              همچنین از پزاپزتی orderIncompletePayment جهت نمایش و عدم نمایش مبلغ باقیمانده ای که کاربر باید پرداخت کند اتفاده میشود
               */
              if (this.entity.orderStatusType) {
                if (this.entity.orderStatusType === OrderStatusTypeEnum.Confirm) {
                  this.orderConfirmed = true;
                  this.orderIncompletePayment = false;
                } else if (this.entity.orderStatusType === OrderStatusTypeEnum.IncompletePayment) {
                  this.orderConfirmed = false;
                  this.orderIncompletePayment = true;
                } else {
                  this.orderConfirmed = false;
                  this.orderIncompletePayment = false;
                }
              }
              /*
              از پراپرتی orderHasSubsidy , orderSubsidyConfirmed
              جهت نمایش پس رنگ وضعیت یارانه استفاده میشود
              اگر هر دو مقدار true باشند آنگاه رنگ وضعیت یارانه سبز و اگر
              orderHasSubsidy true و
              orderSubsidyConfirmed false باشد آنگاه رنگ قرمز
              و اگر هر دو false باشند آنگا رنگ عادی نمایش داده میشود
               */
              if (this.entity.subsidyStatusType) {
                this.orderHasSubsidy = true;
                if (this.entity.subsidyStatusType === SubsidyStatusTypeEnum.Confirmed) {
                  this.orderSubsidyConfirmed = true;
                } else {
                  this.orderSubsidyConfirmed = false;
                }
              } else {
                this.orderSubsidyConfirmed = false;
                this.orderHasSubsidy = false;

              }
            } else {
              this.showError('کاربر گرامی آیتم مورد نظر یافت نشد');
            }
          }
        );
    }
  }
}
