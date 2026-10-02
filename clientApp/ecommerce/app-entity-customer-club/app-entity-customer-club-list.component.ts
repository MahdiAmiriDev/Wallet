import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {AppEntityCustomerClubDetailComponent} from './app-entity-customer-club-detail.component';


@Component({
  selector: 'app-app-entity-customer-club-list',
  templateUrl: './app-entity-customer-club-list.component.html'
})
export class AppEntityCustomerClubListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'appEntityCustomerClub';
  detailComponent = AppEntityCustomerClubDetailComponent;
  _appEntityId: number;
  @Input()
  set appEntityId(val) {
    if (val !== this._appEntityId) {
      this._appEntityId = val;
    }
  }

  get appEntityId(): number {
    return this._appEntityId;
  }

  _customerclubId: number;
  @Input()
  set customerclubId(val) {
    if (val !== this._customerclubId) {
      this._customerclubId = val;
    }
  }

  get customerclubId(): number {
    return this._customerclubId;
  }

  private baseListUrl = 'appEntityCustomerClub/GetByCustomFilterForList?';

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
      caption: 'عنوان',
      dataField: 'title'

    });


    this.columns.push({
      caption: 'آیدی رکورد',
      dataField: 'objectId'

    });


    this.columns.push({
      caption: 'موجودیت رکورد',
      dataField: 'appEntityTitle'

    });


    this.columns.push({
      caption: 'باشگاه مشتریان',
      dataField: 'customerclubTitle'

    });
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.appEntityId) {
        customListUrl += `appEntityId=${this.appEntityId}&`;
      }
      if (this.customerclubId) {
        customListUrl += `customerclubId=${this.customerclubId}&`;
      }
    }
    let res = false;
    if (this.appEntityId || this.customerclubId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as AppEntityCustomerClubDetailComponent);

    if (this.appEntityId) {
      comp.appEntityId = this.appEntityId;
    }

    if (this.customerclubId) {
      comp.customerclubId = this.customerclubId;
    }

  }
}
