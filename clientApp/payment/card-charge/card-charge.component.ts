import {YashilComponent} from '../../../core/baseClasses/yashilComponent';
import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {PaymentService} from '../../../_services';
import {CardChargeModel} from '../../../_models/payment/card-charge.model';
import {first} from 'rxjs/operators';

@Component({
  selector: 'app-card-charge-page',
  templateUrl: './card-charge.component.html'
})

export class CardChargeComponent extends YashilComponent implements OnInit {

  _genericDataService: GenericDataService;
  _paymentService: PaymentService;
  cardChargeModel: CardChargeModel;
  organizationDataSource: any;
  cardTypes: any;
  chargeTypes: any;
  error = '';
  private loading: boolean;
  private submitted: boolean;

  constructor(private genericDataService: GenericDataService, private paymentService: PaymentService) {
    super();
    this._genericDataService = genericDataService;
    this._paymentService = paymentService;
    this.cardChargeModel = new CardChargeModel();
    this.chargeTypes = [
      {text: 'بن ریالی', value: false},
      {text: 'اعتباری', value: true}
    ];
  }

  ngOnInit(): void {
    this.organizationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'organization');
    this._genericDataService.getCommonBaseDataForSelect('CardType').subscribe(res => this.cardTypes = res);
  }

  onSubmit(): void {
    this.setBusy(true);
    this.submitted = true;
    this.loading = true;
    this._paymentService.doCardCharge(this.cardChargeModel)
      .pipe(first())
      .subscribe(
        data => {
          this.setBusy(false);
          if (data && data.succeeded) {
            this.showMessage('شارژ کارت ها با موفقیت انجام شد');
          } else {
            this.showError('خطایی در شارژ کارت ها رخ داده است');
          }
        },
        error => {
          this.error = error;
          this.loading = false;
          this.setBusy(false);
        }
      );
  }

}
