import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';

@Component({
  selector: 'app-factor-group-detail',
  templateUrl: './factor-group-detail.component.html'
})
export class FactorGroupDetailComponent extends BaseEdit implements OnInit {
  @Input()
  parentId: number;
  @Input()
  factorGroupStatus: number;
  parentDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'factorGroup';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.parentId) {
      this.entity.parentId = this.parentId;
    }
    if (this.factorGroupStatus) {
      this.entity.factorGroupStatus = this.factorGroupStatus;
    }
    this.parentDataSource = this._genericDataService.createCustomDataSourceWithAction('id', 'factorGroup', 'getForSelectParent');
  }
}
