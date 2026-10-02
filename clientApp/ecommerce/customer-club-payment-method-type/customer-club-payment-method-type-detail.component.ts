import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-customer-club-payment-method-type-detail',
  templateUrl: './customer-club-payment-method-type-detail.component.html'
})
export class CustomerClubPaymentMethodTypeDetailComponent extends BaseEdit implements OnInit {
  @Input()
  paymentMethodType: number;
  @Input()
  customerClubId: number;
  paymentMethodTypes: any;
  customerClubDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'customerClubPaymentMethodType';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.paymentMethodType) {
      this.entity.paymentMethodType = this.paymentMethodType;
    }
    if (this.customerClubId) {
      this.entity.customerClubId = this.customerClubId;
    }

    this._genericDataService.getCommonBaseDataForSelect('PaymentMethodType').subscribe(res => this.paymentMethodTypes = res);
    this.customerClubDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'customerClub');
  }
}
