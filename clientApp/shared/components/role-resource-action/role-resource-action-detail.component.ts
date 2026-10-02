import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from "../../base/classes/base-edit";
import {GenericDataService} from "../../base/services/generic-data.service";


@Component({
  selector: 'app-role-resource-action-detail',
  templateUrl: './role-resource-action-detail.component.html'
})
export class RoleResourceActionDetailComponent extends BaseEdit implements OnInit {


  roleId: any;
  columns: any[] = [];
  test: boolean = false;
  notAssignedExtraQueryParam: any;
  assignedExtraQueryParam: any;

  @Input()
  resourceName: string;


  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'roleResourceAction';
  }

  ngOnInit(): void {
    this.bindColumns();
    this.setAssignableListExtraQueryParams();
    super.ngOnInit();
  }

  /* بایند کردن مقادیر column */
  public bindColumns(): void {
    this.columns.push({
      caption: 'عملیات',
      dataField: 'title'
    });
  }

  /* ست کردن مقادیر پارامتر های extra مربوط به assignableList*/
  public setAssignableListExtraQueryParams(): void {
    if (this.resourceName) {
      this.notAssignedExtraQueryParam = 'resourceName=' + this.resourceName;
      this.assignedExtraQueryParam = 'resourceName=' + this.resourceName;
    }
  }
}
