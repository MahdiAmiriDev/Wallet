import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-payment-detail',
  templateUrl: './payment-detail.component.html'
})
export class PaymentDetailComponent extends BaseEdit implements OnInit {
  paidDateIntDataValue = new FormControl();
  paidDateUtcDataValue = new FormControl();
  @Input()
  orderId: number;
  @Input()
  cardType: number;
  @Input()
  saleType: number;
  @Input()
  paymentStatusType: number;
  @Input()
  paymentMethodType: number;
  orderDataSource: any;
  cardTypes: any;
  saleTypes: any;
  paymentStatusTypes: any;
  paymentMethodTypes: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'payment';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.orderId) {
      this.entity.orderId = this.orderId;
    }
    if (this.cardType) {
      this.entity.cardType = this.cardType;
    }
    if (this.saleType) {
      this.entity.saleType = this.saleType;
    }
    if (this.paymentStatusType) {
      this.entity.paymentStatusType = this.paymentStatusType;
    }
    if (this.paymentMethodType) {
      this.entity.paymentMethodType = this.paymentMethodType;
    }

    this.orderDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'order');
    this._genericDataService.getCommonBaseDataForSelect('CardType').subscribe(res => this.cardTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('SaleType').subscribe(res => this.saleTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('PaymentStatusType').subscribe(res => this.paymentStatusTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('PaymentMethodType').subscribe(res => this.paymentMethodTypes = res);
    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
  }
}
