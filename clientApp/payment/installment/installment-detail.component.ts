import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-installment-detail',
  templateUrl: './installment-detail.component.html'
})
export class InstallmentDetailComponent extends BaseEdit implements OnInit {
  dueDateDataValue = new FormControl();
  @Input()
  paymentId: number;
  @Input()
  paymentStatus: number;
  accessLevels: any[] = [];
  paymentDataSource: any;
  paymentStatuss: any;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'installment';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.paymentId) {
      this.entity.paymentId = this.paymentId;
    }
    if (this.paymentStatus) {
      this.entity.paymentStatus = this.paymentStatus;
    }

    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
    this.paymentDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'creditOrCashPayment');
    this._genericDataService.getCommonBaseDataForSelect('PaymentStatusType').subscribe(res => this.paymentStatuss = res);
  }
}
