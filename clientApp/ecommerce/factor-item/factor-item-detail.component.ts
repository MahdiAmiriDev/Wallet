import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';

@Component({
  selector: 'app-factor-item-detail',
  templateUrl: './factor-item-detail.component.html'
})
export class FactorItemDetailComponent extends BaseEdit implements OnInit {
  @Input()
  factorId: number;
  @Input()
  orderItemId: number;
  factorDataSource: any;
  orderItemDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'factorItem';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.factorId) {
      this.entity.factorId = this.factorId;
    }
    if (this.orderItemId) {
      this.entity.orderItemId = this.orderItemId;
    }

    this.factorDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'factor');
    this.orderItemDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'orderItem');
  }
}
