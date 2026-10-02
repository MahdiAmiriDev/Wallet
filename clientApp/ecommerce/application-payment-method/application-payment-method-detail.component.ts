import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-application-payment-method-detail',
  templateUrl: './application-payment-method-detail.component.html'
})
export class ApplicationPaymentMethodDetailComponent extends BaseEdit implements OnInit {
  @Input()
  paymentMethodType: number;
  paymentMethodTypes: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'applicationPaymentMethod';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.paymentMethodType) {
      this.entity.paymentMethodType = this.paymentMethodType;
    }
    this._genericDataService.getCommonBaseDataForSelect('PaymentMethodType').subscribe(res => this.paymentMethodTypes = res);
  }
}
