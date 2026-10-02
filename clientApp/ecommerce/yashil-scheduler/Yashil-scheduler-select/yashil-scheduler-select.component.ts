import {Component, Input, OnInit} from '@angular/core';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';
import {Selectable} from '../../../../shared/base/classes/selectable';


@Component({
  selector: 'app-yashil-scheduler-select',
  templateUrl: './yashil-scheduler-select.component.html'
})
export class YashilSchedulerSelectComponent extends Selectable implements OnInit {
  @Input()
  isRequired: boolean;
  @Input()
  requiredMessage: string;
  @Input()
  selectedItemId: number;
  columns: any[] = [];
  yashilSchedulerDataSource: any;
  @Input()
  customListUrl: string;

  constructor(private genericDataService: GenericDataService) {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
    if (!this.customListUrl) {
      this.yashilSchedulerDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'yashilScheduler');
    } else {
      this.yashilSchedulerDataSource = this.genericDataService.createCustomDataSourceWithCustomUrl('id', this.customListUrl);
    }
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'عنوان',
      dataField: 'title',
      allowSorting: false,
      allowHeaderFiltering: false,
    },{
      caption: 'تاریخ شروع',
      dataField: 'startDate',
      cellTemplate: 'intDate',
      allowSorting: false,
      allowHeaderFiltering: false,
    },{
      caption: 'تاریخ پایان',
      dataField: 'endDate',
      cellTemplate: 'intDate',
      allowSorting: false,
      allowHeaderFiltering: false,
    },{
      caption: 'بازه از',
      dataField: 'fromEra',
      allowSorting: false,
      allowHeaderFiltering: false,
    },{
      caption: 'بازه تا',
      dataField: 'toEra',
      allowSorting: false,
      allowHeaderFiltering: false,
    },{
      caption: 'حداکثر تکرار',
      dataField: 'repeatNumber',
      allowSorting: false,
      allowHeaderFiltering: false,
    });
  }

  selectedYashilSchedulerChanged(e: any): void {
    if (e && e.selectedRowsData && e.selectedRowsData.length > 0) {
      this.selectedItemId = e.selectedRowsData[0].id;
      this.selectedRowChanged.emit(this.selectedItemId);
    }
  }
}
