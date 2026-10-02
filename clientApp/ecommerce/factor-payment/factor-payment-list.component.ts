import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FactorPaymentDetailComponent} from './factor-payment-detail.component';


@Component({
  selector: 'app-factor-payment-list',
  templateUrl: './factor-payment-list.component.html'
})
export class FactorPaymentListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'factorPayment';
  detailComponent = FactorPaymentDetailComponent;
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
  @Input()
  showAddButton = true;
  private baseListUrl = 'factorPayment/GetByCustomFilterForList?';

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'مبلغ(ریال)',
      dataField: 'price',
      cellTemplate: 'number',
      width: '160px'
    },{
      caption: 'اقساط',
      dataField: 'installmentCount',
      cellTemplate: 'number',
      width: '160px'
    },{
      caption: 'تاریخ پرداخت',
      dataField: 'paymentDate',
      cellTemplate: 'intDate',
      width: '110px'
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
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as FactorPaymentDetailComponent);

    if (this.factorId) {
      comp.factorId = this.factorId;
    }

  }
}
