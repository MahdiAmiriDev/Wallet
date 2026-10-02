import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-contract-payment-method-detail',
  templateUrl: './contract-payment-method-detail.component.html'
})
export class ContractPaymentMethodDetailComponent extends BaseEdit implements OnInit {
  @Input()
  contractId: number;
  @Input()
  paymentMethodType: number;
  contractDataSource: any;
  paymentMethodTypes: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'contractPaymentMethod';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.contractId) {
      this.entity.contractId = this.contractId;
    }
    if (this.paymentMethodType) {
      this.entity.paymentMethodType = this.paymentMethodType;
    }

    this.contractDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'contract');
    this._genericDataService.getCommonBaseDataForSelect('PaymentMethodType').subscribe(res => this.paymentMethodTypes = res);
  }
}
