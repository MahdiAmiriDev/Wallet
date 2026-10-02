import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-additional-user-prop-company-group-detail',
  templateUrl: './additional-user-prop-company-group-detail.component.html'
})
export class AdditionalUserPropCompanyGroupDetailComponent extends BaseEdit implements OnInit {

  columns: any[] = [];

  @Input()
  userId: number;
	@Input()
	additionalUserPropId:number;
	@Input()
	companyGroupId:number;

  notAssignedExtraQueryParam: any;
  assignedExtraQueryParam: any;


  @Input()
  resourceName: string;
  accessLevels: any[] = [];


	constructor(private genericDataService: GenericDataService) {
	super(genericDataService);
	this.entityName = 'AdditionalUserPropCompanyGroup';
	}

  ngOnInit() : void{
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
