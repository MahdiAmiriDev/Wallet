import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FactorPaymentDetailTabBasedComponent} from './factor-payment-detail-tab-based.component';


@Component({
  selector: 'app-custom-factor-payment-list',
  templateUrl: './custom-factor-payment-list.component.html'
})
export class CustomFactorPaymentListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  contentHeight: number;
  showAddButton = false;
  columns: any[] = [];
  entityName = 'factorPayment';
  detailComponent = FactorPaymentDetailTabBasedComponent;
  // tslint:disable-next-line:variable-name
  _factorId: number;
  @Input()
  set factorId(val) {
    if (val !== this._factorId) {
      this._factorId = val;
    }
  }

  get factorId(): number {
    return this._factorId;
  }

  private baseListUrl = 'factorPayment/GetFactorPaymentsList?';

  constructor() {
    super();
    this.contentHeight = window.innerHeight - 110;
  }

  ngOnInit(): void {
    this.bindColumns();
    this.listForm.customListUrl = this.baseListUrl;
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'مبلغ(ریال)',
      dataField: 'price',
      cellTemplate: 'number',
    },{
      caption: 'تعداد اقساط',
      dataField: 'installmentCount',
      cellTemplate: 'number',
    },{
      caption: 'تاریخ پرداخت',
      dataField: 'paymentDate',
      cellTemplate: 'intDate',
    });
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.factorId) {
        customListUrl += `factorId=${this.factorId}&`;
      }
    }
    let res = false;
    if (this.factorId) {
      this.listForm.customListUrl = customListUrl;
      this.showAddButton = true;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as FactorPaymentDetailTabBasedComponent);
    if (this.factorId) {
      comp.factorId = this.factorId;
    }
  }

  /*
  هنگامی که فاکتور انتخاب شده تغییر پیدا میکند لیست پرداختی ها باید رفرش شود
   */
  changeSelectedFactorId(item: any): void {
    if (item.id){
      this.factorId = item.id;
    }else{
      this.factorId = item.data.id;
    }
    this.bindCustomDataSources();
  }
}
