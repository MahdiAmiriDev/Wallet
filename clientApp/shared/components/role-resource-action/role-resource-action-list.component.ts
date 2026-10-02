import {Component, Input, ViewChild} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';
import {RoleResourceActionDetailComponent} from './role-resource-action-detail.component';
import {BaseList} from "../../base/classes/base-list";


@Component({
  selector: 'app-role-resource-action-list',
  templateUrl: './role-resource-action-list.component.html'
})
export class RoleResourceActionListComponent {
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'roleResourceAction';
  detailComponent = RoleResourceActionDetailComponent;
  private baseListUrl: string = `${this.entityName}/GetByCustomFilterForList?`;
  loadAfterSetFilter: boolean;

  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @Input()
  enableRoleResourceActionPopUp: boolean = false;
  @Input()
  resourceName: string;
  @Input()
  allowGrouping: boolean = false;


  constructor(private translate: TranslateService) {
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSource()) {
      this.loadAfterSetFilter = true;
    }
  }

  /* هنگامی که کامپوننت detail در حال initial شدن است اگر دارای resourceName باشیم به کامپوننت detail پاس داده می شود */
  afterInitialDetailComponent(componentInstance: any) {
    if (this.resourceName) {
      const comp = (<RoleResourceActionDetailComponent>componentInstance.instance);
      comp.resourceName = this.resourceName;
    }
  }

  /* بایند کردن column ها با توجه به مقدار resourceName*/
  bindColumns(): void {
    let result = forkJoin({
      roleIdTitle: this.translate.get('um.RoleResourceAction.RoleId'),
      actionTitle: this.translate.get('um.ResourceAppAction.AppActionId'),
      resourceTitle: this.translate.get('um.ResourceAppAction.ResourceId')
    })

    result.subscribe(({resourceTitle, actionTitle, roleIdTitle}) => {
      if (this.resourceName) {
        this.columns.push({
          caption: actionTitle,
          dataField: 'actionTitle',
          allowGrouping: 1,
          groupIndex: 1,
        }, {
          caption: roleIdTitle,
          dataField: 'roleTitle',
          allowGrouping: 0
        });
      } else {
        this.columns.push({
          caption: resourceTitle,
          dataField: 'resourceTitle'
        }, {
          caption: actionTitle,
          dataField: 'actionTitle',
        }, {
          caption: roleIdTitle,
          dataField: 'roleTitle',
        });
      }
    })
  }

  /*  ست کردن url با توجه به مقدار input ها */
  public bindCustomDataSource(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.resourceName) {
        customListUrl += `resourceName=${this.resourceName}`
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

}
