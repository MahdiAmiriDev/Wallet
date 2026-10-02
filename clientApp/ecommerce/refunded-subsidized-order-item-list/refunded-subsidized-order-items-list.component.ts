import {Component, OnInit, ViewChild} from "@angular/core";
import {Selectable} from "../../../shared/base/classes/selectable";
import {BaseList} from "../../../shared/base/classes/base-list";
import {FormControl} from "@angular/forms";
import * as moment from "jalali-moment";

@Component({
  selector: 'app-refunded-subsidized-order-items-list',
  templateUrl: './refunded-subsidized-order-items-list.component.html'
})

export class RefundedSubsidizedOrderItemsListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  radioButtonItems = [];
  subsidyType: number;
  contentHeight: number;
  orderCreationDateFilterFromDateDataValue = new FormControl();
  orderCreationDateFilterToDateDataValue = new FormControl();
  orderCreationDateFilterFromDate: number;
  orderCreationDateFilterToDate: number;
  datePickerConfig = {
    format: 'jYYYY/jMM/jDD',
    dir: 'rtl',
    locale: moment.locale('fa')
  };
  columns: any[] = [];

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.radioButtonItems =
      [
        {text: 'همه موارد', value: 0},
        {text: 'ساتا', value: 1106},
        {text: 'وزارت', value: 1107}];
    this.subsidyType = 0;

    this.orderCreationDateFilterFromDate = this.convertMomentDateToInt(moment().startOf('year'));
    this.orderCreationDateFilterToDate = this.convertMomentDateToInt(moment());
    this.bindColumns();
    this.bindCustomDataSources();
  }

  private bindColumns(): void {
    this.columns.push(
      {
        caption: 'شماره فاکتور',
        dataField: 'id',
        width: '80px'
      },
      {
        caption:'نوع یارانه',
        dataField: 'subsidyType',
        width: '80px'
      },
      {
      caption:'عنوان',
        dataField: 'factorGroupTitle',
      width: '120px'
      }
      ,{
        caption: 'شماره کارت',
        dataField: 'cardNumber',
        width: '120px'
      }
      ,{
        caption: 'کد ملی',
        dataField: 'nationalCode',
        width: '140px',
      },
      {
        caption: 'تاریخ خرید',
        dataField: 'paidDateInt',
        width: '120px',
        cellTemplate: 'intDate'
      }
      ,
      {
        caption: 'تاریخ عودت',
        dataField: 'refundDateInt',
        width: '120px',
        cellTemplate: 'intDate'
      },
      {
        caption: 'کانورت شده',
        dataField: 'isConverted',
        width: '80px',
      }
      , {
        caption: 'یارانه خام',
        dataField: 'subsidyPrice',
        width: '120px',
        cellTemplate: 'number',
      }
      , {
        caption: 'یارانه باکارمزد',
        dataField: 'subsidyPriceIncProfit',
        width: '120px',
        cellTemplate: 'number',
      }
      , {
        caption: 'یارانه با کارمزد و مالیات',
        dataField: 'subsidyPriceIncTaxIncProfit',
        width: '120px',
        cellTemplate: 'number',
      },
    );
  }
  //دریافت گزارش سفارش هایی که یارانه گرفته و عودت وجه رخ داده است
  bindCustomDataSources(): boolean {
    let customListUrl = `orderItem/GetAllRefundedOrderItemsByDate?subsidyType=${this.subsidyType}&`;

    if (this.listForm) {

      if (this.orderCreationDateFilterFromDate) {
        customListUrl += `orderCreationDateFromDate=${this.orderCreationDateFilterFromDate}&`;
      }
      if (this.orderCreationDateFilterToDate) {
        customListUrl += `orderCreationDateToDate=${this.orderCreationDateFilterToDate}&`;
      }
    }
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    return true;
  }
}
