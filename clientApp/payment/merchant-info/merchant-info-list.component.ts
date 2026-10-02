import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {MerchantInfoDetailComponent} from './merchant-info-detail.component';
import {MerchantInfoDetailTabBasedComponent} from "./merchant-info-detail-tab-based.component";


@Component({
  selector: 'app-merchant-info-list',
  templateUrl: './merchant-info-list.component.html'
})
export class MerchantInfoListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'merchantInfo';
  detailComponent = MerchantInfoDetailTabBasedComponent;
  _state: number;
  @Input()
  set state(val) {
    if (val !== this._state) {
      this._state = val;
    }
  }

  get state(): number {
    return this._state;
  }

  _city: number;
  @Input()
  set city(val) {
    if (val !== this._city) {
      this._city = val;
    }
  }

  get city(): number {
    return this._city;
  }

  private baseListUrl = 'merchantInfo/GetByCustomFilterForList?';

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
      caption: 'شناسه پذیرنده',
      dataField: 'id'
    });
    this.columns.push({
      caption: 'نام پذیرنده',
      dataField: 'merchantName'
    });
    this.columns.push({
      caption: 'کاربر',
      dataField: 'username'
    });
    this.columns.push({
      caption: 'شهر',
      dataField: 'cityTitle'
    });
    this.columns.push({
      caption: 'شماره تلفن ثابت',
      dataField: 'phoneNumber'
    });
    this.columns.push({
      caption: 'شماره تلفن همراه',
      dataField: 'mobileNumber'
    });
  }

  public bindCustomDataSources() {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.state) {
        customListUrl += `state=${this.state}&`;
      }
      if (this.city) {
        customListUrl += `city=${this.city}&`;
      }
    }
    let res = false;
    if (this.state || this.city) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any) {
    const comp = (<MerchantInfoDetailComponent>componentInstance.instance);

    if (this.state) {
      comp.state = this.state;
    }

    if (this.city) {
      comp.city = this.city;
    }

  }
}
