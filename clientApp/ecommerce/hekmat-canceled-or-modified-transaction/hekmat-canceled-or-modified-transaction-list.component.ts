import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HekmatTransactionModel} from './hekmat-transaction.model';


@Component({
  selector: 'app-hekmat-canceled-or-modified-transaction-list',
  templateUrl: './hekmat-canceled-or-modified-transaction-list.component.html'
})
export class HekmatCanceledOrModifiedTransactionListComponent extends Selectable implements OnInit {


  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'refundDetail';
  private baseListUrl = 'refundDetail/GetHekmatCanceledOrModifiedTransactions?';

  // tslint:disable-next-line:variable-name
  private _hekmatTransactionData: HekmatTransactionModel;
  get hekmatTransactionData(): HekmatTransactionModel {
    return this._hekmatTransactionData;
  }

  @Input()
  set hekmatTransactionData(value: HekmatTransactionModel) {
    if (this.hekmatTransactionData !== value) {
      this._hekmatTransactionData = value;
      this.bindCustomDataSources();
    }
  }

  constructor() {
    super();
    this.columns.push({
      caption: 'شماره کارت',
      dataField: 'cardNumber',
      allowHeaderFiltering: false,
      allowSorting: false,
    }, {
      caption: 'مبلغ تراکنش ارسالی(ریال)',
      dataField: 'price',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      allowSorting: false,
    }, {
      caption: 'مبلغ عودت(ریال)',
      dataField: 'refundPrice',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      allowSorting: false,
    }, {
      caption: 'تاریخ ثبت تراکنش',
      dataField: 'transactionDate',
      cellTemplate: 'intDate',
      allowHeaderFiltering: false,
      allowSorting: false,
    });

  }

  ngOnInit(): void {
    this.loadAfterSetFilter = true;
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.hekmatTransactionData) {
        if (this.hekmatTransactionData.CardNumber) {
          customListUrl += `cardNumber=${this.hekmatTransactionData.CardNumber}&`;
        }
        if (this.hekmatTransactionData.Price) {
          customListUrl += `price=${this.hekmatTransactionData.Price}&`;
        }
      }
    }
    let res = false;
    if (this.hekmatTransactionData && this.hekmatTransactionData.CardNumber && this.hekmatTransactionData.Price) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }
}
