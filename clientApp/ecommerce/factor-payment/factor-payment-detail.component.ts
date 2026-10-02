import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {FactorStatisticComponent} from '../factor-statistic/factor-statistic.component';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-factor-payment-detail',
  templateUrl: './factor-payment-detail.component.html'
})
export class FactorPaymentDetailComponent extends BaseEdit implements OnInit {
  paymentDateDataValue = new FormControl();
  @ViewChild('factorPaymentStatistic', {static: true}) factorPaymentStatistic: FactorStatisticComponent;
  @Input()
  factorId: number;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.getFactorPaymentInstallmentPrice = this.getFactorPaymentInstallmentPrice.bind(this);
    this.reloadFactorStatisticData = this.reloadFactorStatisticData.bind(this);
    this.entityName = 'factorPayment';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.factorId) {
      this.entity.factorId = this.factorId;
    }
  }

  afterLoadData(res: any): any {
    return super.afterLoadData(res);
  }

  // دریافت مبلغ پرداختی اقساط
  // پس از آنکه کاربر تعداد اقساط را وارد کرد
  // مبلغ پرداختی به از سمت سرور دریافت میشود و به کاربر نشان داده میشود
  getFactorPaymentInstallmentPrice($event: Event): void {
    let url = 'GetFactorPaymentInstallmentPrice?';
    if (this.factorId) {
      url = url + `factorId=${this.factorId}&`;
      if (this.entity.id) {
        url = url + `factorPaymentId=${this.entity.id}&`;
      }
      if (this.entity.installmentCount) {
        url = url + `installmentCount=${this.entity.installmentCount}&`;
        this.busyIndicator.setBusy(true);
        this.genericDataService.getEntity('factorPayment', this.factorId, url)
          .subscribe(
            res => {
              this.busyIndicator.setBusy(false);
              if (res && res > 0) {
                this.entity.price = res;
              } else {
                this.showError('کاربر گرامی تعداد اقساط وارد شده صحیح نمی باشد');
              }
            });
      }
    } else {
      this.showError('کاربر گرامی فاکتوری انتخاب نشده است');
    }
  }

  public reloadFactorStatisticData(): void {
    this.factorPaymentStatistic.loadData();
  }
}
