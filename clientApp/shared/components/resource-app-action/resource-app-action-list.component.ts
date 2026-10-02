import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ResourceAppActionDetailComponent} from './resource-app-action-detail.component';
import {BaseList} from "../../base/classes/base-list";


@Component({
  selector: 'app-resource-app-action-list',
  templateUrl: './resource-app-action-list.component.html'
})
export class ResourceAppActionListComponent implements OnInit {
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'resourceAppAction';
  detailComponent = ResourceAppActionDetailComponent;
  loadAfterSetFilter: boolean;
  private baseListUrl: string = `${this.entityName}/GetByCustomFilterForList?`;

  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @Input()
  enableResourceAppActionPopUp: boolean = false;
  @Input()
  resourceName: string;
  @Input()
  allowGrouping: boolean = false;

  constructor() {
  }

  ngOnInit() {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  /* هنگامی که کامپوننت detail در حال initial شدن است اگر دارای resourceName باشیم به کامپوننت detail پاس داده می شود */
  afterInitialDetailComponent(componentInstance: any) {
    if (this.resourceName) {
      const comp = (<ResourceAppActionDetailComponent>componentInstance.instance);
      comp.resourceName = this.resourceName;
    }
  }

  /*  ست کردن url با توجه به مقدار input ها */
  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.resourceName) {
        customListUrl += `resourceName=${this.resourceName}`;
      }
    }
    let res = false;
    if (this.resourceName) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }

  /* بایند کردن column ها با توجه به مقدار resourceName*/
  bindColumns(): void {
    if (this.resourceName) {
      this.columns.push({
        caption: 'عملیات',
        dataField: 'appActionTitle'
      });
    } else {
      this.columns.push({
        caption: 'عملیات',
        dataField: 'appActionTitle'
      }, {
        caption: 'منبع',
        dataField: 'resourceTitle'
      });
    }
  }

}
