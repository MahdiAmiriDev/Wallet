import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-uploaded-factor-payment-detail',
  templateUrl: './uploaded-factor-payment-detail.component.html'
})
export class UploadedFactorPaymentDetailComponent extends BaseEdit implements OnInit {
  paymentDateDataValue = new FormControl();
  paymentDateIsInvalidDataValue = new FormControl();
  @Input()
  appDocumentId: number;
  appDocumentDataSource: any;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'uploadedFactorPayment';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.appDocumentId) {
      this.entity.appDocumentId = this.appDocumentId;
    }

    this.appDocumentDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'appDocument');
  }
}
