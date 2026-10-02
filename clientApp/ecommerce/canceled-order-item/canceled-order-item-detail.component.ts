import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-canceled-order-item-detail',
  templateUrl: './canceled-order-item-detail.component.html'
})
export class CanceledOrderItemDetailComponent extends BaseEdit implements OnInit {
  @Input()
  orderItemId: number;
  orderItemDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'canceledOrderItem';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.orderItemId) {
      this.entity.orderItemId = this.orderItemId;
    }
    this.orderItemDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'orderItem');
  }
}
