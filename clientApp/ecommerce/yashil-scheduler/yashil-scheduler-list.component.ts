import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {YashilSchedulerDetailComponent} from './yashil-scheduler-detail.component';


@Component({
  selector: 'app-yashil-scheduler-list',
  templateUrl: './yashil-scheduler-list.component.html'
})
export class YashilSchedulerListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'yashilScheduler';
  detailComponent = YashilSchedulerDetailComponent;
  // tslint:disable-next-line:variable-name
  _occuranceType: number;
  @Input()
  set occuranceType(val) {
    if (val !== this._occuranceType) {
      this._occuranceType = val;
    }
  }

  get occuranceType(): number {
    return this._occuranceType;
  }

  private baseListUrl = 'yashilScheduler/GetByCustomFilterForList?';

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
      dataField: 'title',
      allowHeaderFiltering: false,
    },{
      caption: 'نوع زمانبندی',
      dataField: 'occuranceTypeTitle'
    },{
      caption: 'تاریخ شروع',
      dataField: 'startDate',
      cellTemplate: 'intDate',
      allowHeaderFiltering: false,
    },{
      caption: 'تاریخ پایان',
      dataField: 'endDate',
      cellTemplate: 'intDate',
      allowHeaderFiltering: false,
    },{
      caption: 'بازه از',
      dataField: 'fromEra',
      allowHeaderFiltering: false,
    },{
      caption: 'بازه تا',
      dataField: 'toEra',
      allowHeaderFiltering: false,
    },{
      caption: 'حداکثر تکرار',
      dataField: 'repeatNumber'
    });
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.occuranceType) {
        customListUrl += `occuranceType=${this.occuranceType}&`;
      }
    }
    let res = false;
    if (this.occuranceType) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as YashilSchedulerDetailComponent);

    if (this.occuranceType) {
      comp.occuranceType = this.occuranceType;
    }

  }
}
