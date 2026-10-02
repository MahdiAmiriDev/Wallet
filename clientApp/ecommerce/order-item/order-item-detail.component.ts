import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-order-item-detail',
  templateUrl: './order-item-detail.component.html'
})
export class OrderItemDetailComponent extends BaseEdit implements OnInit {
  @Input()
  orderId: number;
  @Input()
  subsidyId: number;
  @Input()
  subsidyType: number;
  @Input()
  saleType: number;
  orderDataSource: any;
  subsidyDataSource: any;
  subsidyTypes: any;
  saleTypes: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'orderItem';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.orderId) {
      this.entity.orderId = this.orderId;
    }
    if (this.subsidyId) {
      this.entity.subsidyId = this.subsidyId;
    }
    if (this.subsidyType) {
      this.entity.subsidyType = this.subsidyType;
    }
    if (this.saleType) {
      this.entity.saleType = this.saleType;
    }
    this.orderDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'order');
    this.subsidyDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'subsidy');
    this._genericDataService.getCommonBaseDataForSelect('SubsidyType').subscribe(res => this.subsidyTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('SaleType').subscribe(res => this.saleTypes = res);
    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
  }
}
