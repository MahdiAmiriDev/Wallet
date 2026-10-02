import {Component, OnInit, ViewChild} from '@angular/core';
import {customAnimations} from '../../../shared/animations/custom-animations';
import {YashilComponent} from '../../../core/baseClasses/yashilComponent';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {AppEntityGroupingListComponent} from '../../tms/app-entity-grouping/app-entity-grouping-list.component';
import {YashilCustomButton} from '../../../shared/base/models/yasil-custom-button';
import {HekmatTransactionModel} from '../hekmat-canceled-or-modified-transaction/hekmat-transaction.model';
import {ItemGroupKeyModel} from '../_models/item-group-key.model';

@Component({
  selector: 'app-refund-detail-grouping',
  templateUrl: './refund-detail-grouping.component.html',
  animations: customAnimations
})
export class RefundDetailGroupingComponent extends YashilComponent implements OnInit {
  itemGroupKeyModel: ItemGroupKeyModel;
  columns: any[] = [];
  customRowButtons: YashilCustomButtonColumn;
  showPopUp: boolean;
  selectedRefundDetailId: number;
  @ViewChild('entityGroupingList', {static: true}) entityGroupingList: AppEntityGroupingListComponent;
  hekmatTransactionData: HekmatTransactionModel;
  GetNotGroupedItems;

  constructor() {
    super();
    this.setRefundAmount = this.setRefundAmount.bind(this);
  }

  ngOnInit(): void {
    this.columns = [
      {
        caption: 'ردیف گردش ترمینال',
        dataField: 'installmentTrackingCode',
        allowHeaderFiltering: false,
        width: '70px',
      }, {
        caption: 'کد سفارش',
        dataField: 'orderTitle',
        allowHeaderFiltering: false,
        width: '100px',
      }, {
        caption: 'کدملی داوطلب',
        dataField: 'nationalCode',
        allowHeaderFiltering: false,
        width: '100px',
      },
      {
        caption: 'تاریخ ثبت پرداخت',
        dataField: 'paidDateInt',
        cellTemplate: 'intDate',
        allowHeaderFiltering: false,
        width: '110px',
      }, {
        caption: 'شماره کارت',
        dataField: 'cardNumber',
        allowHeaderFiltering: false,
        width: '150px',
      }, {
        caption: 'مبلغ تراکنش ارسالی(ریال)',
        dataField: 'oldPaymentAmount',
        allowHeaderFiltering: false,
        cellTemplate: 'number',
        width: '100px',
      },
      {
        caption: 'مبلغ تراکنش نهایی(ریال)',
        dataField: 'newPaymentAmount',
        allowHeaderFiltering: false,
        cellTemplate: 'number',
        width: '100px',
      },
      {
        caption: 'مبلغ عودتی به کاربر(ریال)',
        dataField: 'refundAmount',
        allowHeaderFiltering: false,
        cellTemplate: 'number',
        width: '100px',
      },
      {
        caption: 'نام محصول',
        dataField: 'productTitle',
        allowHeaderFiltering: false,
        allowFiltering: false,
        allowSorting: false,
        width: '150px',
      },
      {
        caption: 'شهر',
        dataField: 'cityTitle',
        allowHeaderFiltering: false,
        allowFiltering: false,
        allowSorting: false,
        width: '100px',
      },
      {
        caption: 'نام طرح',
        dataField: 'applicationName',
        allowHeaderFiltering: false,
        width: '100px',
      },

      {
        caption: 'تاریخ بررسی عودت',
        dataField: 'refundAmountSetDate',
        cellTemplate: 'jalaliDate',
        allowHeaderFiltering: false,
        width: '110px',
      },
      {
        caption: 'بررسی توسط',
        dataField: 'refundAmountSetByFullName',
        allowHeaderFiltering: false,
        width: '100px',
      },
      {
        caption: 'وضعیت بررسی',
        dataField: 'confirmed',
        allowHeaderFiltering: false,
        width: '80px',
      },
      {
        caption: 'اطلاعات حساب دریافت شده',
        dataField: 'isSetPersonBankAccount',
        width: '80px',
      }];

    this.itemGroupKeyModel = new ItemGroupKeyModel();
    this.itemGroupKeyModel.keyTitle = 'سال و ماه';
    this.itemGroupKeyModel.required = true;

    const rowButtons: YashilCustomButton[] = [];
    rowButtons.push({
      icon: 'todo',
      text: 'بررسی',
      hint: 'بررسی و تایید مبلغ عودت وجه',
      type: 'danger',
      // هنگامی که آیت متایید شده باشد دیگر این دکمه نشان داده نمیشود
      visibleProp: 'allowConfirm',
      onClick: this.setRefundAmount,
      width: '85px',
    });
    //  visibleProp: 'allowSetRefundAmount'
    this.customRowButtons = {
      customButtons: rowButtons,
      setAtFirstColumn: true,
      allowExporting: false,
      width: '100px'
    };
  }

  setRefundAmount(item: any): void {
    this.showPopUp = true;
    this.selectedRefundDetailId = item.data.id;

  }

  // این متد هنگامی صدا زده میشود که پس از تایید یا رد آیتم بخواهیم لیست ایتم های گروه بندی شده رفرش شود
  reloadRefundDetailList(): void {
    // باید لیست رفرش شود
    this.showPopUp = false;
    this.entityGroupingList.reloadAppEntityListForm();
  }


  receiveHekmatTransactionData(item: HekmatTransactionModel): void {
    this.hekmatTransactionData = item;
  }

  /*
  تعیین رنگ هر لیست
  هنگامی که اطلاعات شماره حساب گرفته شده باشد رنگ آن سبز میشود.
   */
  setColorForRefundDetailItemListRow(row: any): void {
    if (row.rowType === 'data') {
      if (row.data.isSetPersonBankAccount) {
        row.rowElement.bgColor = '#60d394';
        row.rowElement.className = row.rowElement.className.replace('dx-row-alt', '');
      }
    }
  }
}
