import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from "../../base/classes/base-edit";
import {GenericDataService} from "../../base/services/generic-data.service";


@Component({
  selector: 'app-resource-app-action-detail',
  templateUrl: './resource-app-action-detail.component.html'
})
export class ResourceAppActionDetailComponent extends BaseEdit implements OnInit {

  @Input()
  resourceName: string;

  columns: any[] = [];
  resourceId: any;


  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'resourceAppAction';
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.getResourceIdByResourceName();
    this.bindColumns();
  }


  /* دریافت resourceId براساس resourceName */
  getResourceIdByResourceName() {
    if (this.resourceName) {
      this.genericDataService.getEntitiesWithAction(`Resource`, `GetByCustomFilterForSelect?resourceName=${this.resourceName}`, null)
        .subscribe((res: any) => {
          this.resourceId = res.data[0].id;
        })
    }
  }

  /* بایند ستنون ها به columns */
  private bindColumns(): void {
    this.columns.push({
      caption: 'عملیات',
      dataField: 'title'
    });
  }
}
