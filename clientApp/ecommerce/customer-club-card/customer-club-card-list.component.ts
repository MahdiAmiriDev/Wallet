import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {CustomerClubCardDetailComponent} from './customer-club-card-detail.component';


@Component({
  selector: 'app-customer-club-card-list',
  templateUrl: './customer-club-card-list.component.html'
})
export class CustomerClubCardListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'customerClubCard';
  detailComponent = CustomerClubCardDetailComponent;
  _personId: number;
  @Input()
  set personId(val) {
    if (val !== this._personId) {
      this._personId = val;
    }
  }

  get personId(): number {
    return this._personId;
  }

  _customerClubId: number;
  @Input()
  set customerClubId(val) {
    if (val !== this._customerClubId) {
      this._customerClubId = val;
    }
  }

  get customerClubId(): number {
    return this._customerClubId;
  }

  private baseListUrl = 'customerClubCard/GetByCustomFilterForList?';

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns() {
    this.columns.push({
      caption: 'عنوان',
      dataField: 'title'
    });
    this.columns.push({
      caption: 'شخص',
      dataField: 'personTitle'
    });
    this.columns.push({
      caption: 'باشگاه',
      dataField: 'customerClubTitle'
    });
    this.columns.push({
      caption: 'شماره کارت',
      dataField: 'cardNumber',
      width: '170px'
    });
    this.columns.push({
      caption: 'سقف مبلغ خرید به ریال',
      dataField: 'maximumPurchaseAmount'
    });
    this.columns.push({
      caption: 'سقف تعداد خرید',
      dataField: 'maximumNumberPurchases'
    });
    this.columns.push({
      caption: 'جمع مبلغ استفاده شده به ریال',
      dataField: 'amountUsed'
    });
    this.columns.push({
      caption: 'تاریخ پایان',
      dataField: 'toDate',
      cellTemplate: 'intDate'
    });
    this.columns.push({
      caption: 'تاریخ شروع',
      dataField: 'fromDate',
      cellTemplate: 'intDate'
    });
    this.columns.push({
      caption: 'فعال',
      dataField: 'isActive'
    });
  }

  public bindCustomDataSources() {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.personId) {
        customListUrl += `personId=${this.personId}&`;
      }
      if (this.customerClubId) {
        customListUrl += `customerClubId=${this.customerClubId}&`;
      }
    }
    let res = false;
    if (this.personId || this.customerClubId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any) {
    const comp = (<CustomerClubCardDetailComponent> componentInstance.instance);

    if (this.personId) {
      comp.personId = this.personId;
    }

    if (this.customerClubId) {
      comp.customerClubId = this.customerClubId;
    }

  }
}
