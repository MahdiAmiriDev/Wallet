import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';


@Component({
  selector: 'app-pre-registration-list',
  templateUrl: './pre-registration-list.component.html'
})
export class PreRegistrationListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'preRegistration';
  detailComponent = null;
  _userId: number;
  @Input()
  set userId(val) {
    if (val !== this._userId) {
      this._userId = val;
    }
  }

  get userId(): number {
    return this._userId;
  }

  _productSalesPlanningId: number;
  @Input()
  set productSalesPlanningId(val) {
    if (val !== this._productSalesPlanningId) {
      this._productSalesPlanningId = val;
    }
  }

  get productSalesPlanningId(): number {
    return this._productSalesPlanningId;
  }

  private baseListUrl = 'preRegistration/GetByCustomFilterForList?';

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

    if (!this.productSalesPlanningId){
      this.columns.push(
        {
          caption: 'کد برنامه ریزی دوره',
          dataField: 'productSalesPlanningId'
        },
        {
          caption: 'نام کالا',
          dataField: 'producttitle'

        });
    }
    this.columns.push(
      {
        caption: 'نام کاربری',
        dataField: 'userName'

      },
      {
        caption: 'تاریخ پیش ثبت نام',
        dataField: 'creationDate',
        cellTemplate: 'jalaliDate'
      });



  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.userId) {
        customListUrl += `userId=${this.userId}&`;
      }
      if (this.productSalesPlanningId) {
        customListUrl += `productSalesPlanningId=${this.productSalesPlanningId}&`;
      }
    }
    let res = false;
    if (this.userId || this.productSalesPlanningId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }
}
