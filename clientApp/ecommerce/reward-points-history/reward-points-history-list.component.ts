import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {RewardPointsHistoryDetailComponent} from './reward-points-history-detail.component';


@Component({
  selector: 'app-reward-points-history-list',
  templateUrl: './reward-points-history-list.component.html'
})
export class RewardPointsHistoryListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'rewardPointsHistory';
  detailComponent = RewardPointsHistoryDetailComponent;
  private baseListUrl = 'rewardPointsHistory/GetByCustomFilterForList?';

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
      caption: 'شناسه مشتری',
      dataField: 'customerId'
    },{
      caption: 'شناسه فروشگاه',
      dataField: 'storeId'
    },{
      caption: 'امتیاز اضافه شده است',
      dataField: 'points'
    },{
      caption: 'تعادل امتیاز',
      dataField: 'pointsBalance'
    },{
      caption: 'مقدار استفاده شده',
      dataField: 'usedAmount'
    },{
      caption: 'تاریخ و زمانی که امتیاز دیگر معتبر نخواهد بود',
      dataField: 'endDateUtc'
    },{
      caption: 'تعداد امتیازهای معتبر که هنوز هزینه نکرده اید',
      dataField: 'validPoints'
    },{
      caption: 'عنوان',
      dataField: 'title'
    });
  }

  public bindCustomDataSources() {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
    }

    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();

    return false;

  }

  afterInitialDetailComponent(componentInstance: any) {
    const comp = (<RewardPointsHistoryDetailComponent> componentInstance.instance);

  }
}
