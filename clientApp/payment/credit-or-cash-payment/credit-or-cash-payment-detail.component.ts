import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-credit-or-cash-payment-detail',
  templateUrl: './credit-or-cash-payment-detail.component.html'
})
export class CreditOrCashPaymentDetailComponent extends BaseEdit implements OnInit {
  buyDateDataValue = new FormControl();
  returnedDateDataValue = new FormControl();
  @Input()
  cardId: number;
  @Input()
  merchantId: number;
  @Input()
  status: number;
  accessLevels: any[] = [];
  cardDataSource: any;
  merchantDataSource: any;
  statuss: any;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'creditOrCashPayment';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.cardId) {
      this.entity.cardId = this.cardId;
    }
    if (this.merchantId) {
      this.entity.merchantId = this.merchantId;
    }
    if (this.status) {
      this.entity.status = this.status;
    }

    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
    this.cardDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'cardInfo');
    this.merchantDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'merchantInfo');
    this._genericDataService.getCommonBaseDataForSelect('Status').subscribe(res => this.statuss = res);
  }
}
