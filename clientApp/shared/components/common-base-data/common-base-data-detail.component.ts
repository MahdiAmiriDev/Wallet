import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../base/classes/base-edit';
import {GenericDataService} from '../../base/services/generic-data.service';
import {Entity} from '../../base/base-data/entity.enum';


@Component({
  selector: 'app-common-base-data-detail',
  templateUrl: './common-base-data-detail.component.html'
})
export class CommonBaseDataDetailComponent extends BaseEdit implements OnInit {
  @Input()
  commonBaseTypeId: number;
  pagePositionTypes: any;
  parents: any;
  commonBaseTypeDataSource: any;
  accessLevels: any[] = [];
  //آیا در حالت افزودن برچسب هستیم
  isTagTypeModeActive:boolean = false;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'commonBaseData';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.commonBaseTypeId && !this.selectedEntityId) {
      this.entity.pagePositionId = this.commonBaseTypeId;
    }
    this.parents = this._genericDataService.createCustomDataSourceForSelect('id', 'commonBaseData');
    this.commonBaseTypeDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'commonBaseType');
    this._genericDataService.getCommonBaseDataForSelect('PagePositionType').subscribe(res => this.pagePositionTypes = res);
    this._genericDataService.getEntitiesByEntityName(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
  }
}
