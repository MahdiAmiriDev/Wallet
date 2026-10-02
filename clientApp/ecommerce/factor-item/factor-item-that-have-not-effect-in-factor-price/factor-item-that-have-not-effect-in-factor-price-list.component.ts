import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Selectable} from "../../../../shared/base/classes/selectable";
import {GenericDataService} from "../../../../shared/base/services/generic-data.service";

@Component({
  selector: 'factor-item-that-have-not-effect-in-factor-price-list-component',
  templateUrl: './factor-item-that-have-not-effect-in-factor-price-list.component.html',
  styleUrls: ['./factor-item-that-have-not-effect-in-factor-price-list.component.css']
})

export class FactorItemThatHaveNotEffectInFactorPriceListComponent extends Selectable implements OnInit {

  @Output() afterUpdateDone: EventEmitter<any> = new EventEmitter();

  @Input() factorId: number;
  @Input() factorInstallmentCount: number = 0;


  columns: any[] = [];


  constructor(private genericDataService: GenericDataService) {
    super();
    this.updateInsurancePercentageFromSaleConfiguration = this.updateInsurancePercentageFromSaleConfiguration.bind(this);
    this.updateOtherDeductionFromSaleConfiguration = this.updateOtherDeductionFromSaleConfiguration.bind(this);
    this.updateFactorItemIsIncludeInsurance = this.updateFactorItemIsIncludeInsurance.bind(this);
    this.UpdateFactorInstallmentCountFromSaleConfiguration = this.UpdateFactorInstallmentCountFromSaleConfiguration.bind(this);
  }

  ngOnInit() {
    this.bindColumns();
  }

  //بایند کردن ستون های لیست داده
  bindColumns(): void {
    this.columns.push({
        caption: 'درصد بیمه فاکتور تامین کننده',
        dataField: 'orderItemInsurancePercentage',
        width: '150px',
      },
      {
        caption: 'مبلغ  بیمه فاکتور تامین کننده',
        dataField: 'orderItemInsurancePrice',
        width: '170px',
      },
      {
        caption: 'مبلغ سایر کسورات فاکتور تامین کننده',
        dataField: 'orderItemOtherDeductionsPrice',
        width: '170px',
      }
      , {
        caption: 'درصد سایر کسورات فاکتور تامین کننده',
        dataField: 'orderItemOtherDeductionsPercentage',
        width: '160px',
      }, {
        caption: 'درصد بیمه تنظیم فروش',
        dataField: 'saleConfigurationInsurancePercentage',
        width: '150px',
      }, {
        caption: 'شامل بیمه می شود',
        dataField: 'isIncludeInsurance',
        width: '70px',
      });
  }


  //بروزرسانی درصد بیمه از تنظیم فروش
  updateInsurancePercentageFromSaleConfiguration(): void {

    this.sendUpdateRequest('تایید بروزرسانی', 'آیا از بروزرسانی مقدار درصد بیمه بر اساس تنظیم فروش اطمینان دارید؟',
      'UpdateFactorItemsInsurancePercentageAndPriceToFactorSaleConfigurationInsurancePercentage', 'factorItem')
  }

  //برزورسانی سایر کسورات از تنظیم فروش
  updateOtherDeductionFromSaleConfiguration(): void {

    this.sendUpdateRequest('تایید بروزرسانی', 'آیا از بروزرسانی مقدار درصد و مبلغ سایر کسورات بر اساس تنظیم فروش اطمینان دارید؟',
      'UpdateFactorItemsOtherDeductionsPercentageAndPriceFromOrderItemSaleConfiguration', 'factorItem');
  }


  //بروز رسانی شامل بیمه بودن ایتم های سفارش
  updateFactorItemIsIncludeInsurance(): void {
    this.sendUpdateRequest('تایید بروزرسانی', 'آیا از بروزرسانی شامل بیمه بودن ایتم های سفارش اطمینان دارید؟',
      'UpdateFactorItemIsIncludeInsuranceFromProductCategory', 'factorItem');
  }

  //بروزرسانی تعداد اقساط تصویه فاکتور
  UpdateFactorInstallmentCountFromSaleConfiguration(): void {
    this.sendUpdateRequest(`تایید بروزرسانی`, `آیا از بروزرسانی مقدار تعداد اقطساط فاکتور از تنظیم فروش اطمینان دارید؟`,
      `UpdateFactorInstallmentCountFromSaleConfiguration`, 'factor');
  }


  //این متد جهت مدیریت ارسال درخواست های بروزرسانی به سرور کاربرد دارد.
  sendUpdateRequest(confirmMessageTitle: string, confirmMessage: string, actionName: string, entityName: string): void {
    this.confirmBeforeAction(confirmMessageTitle, confirmMessage, 'تایید')
      .show().then((confirmedRow: any) => {
      if (confirmedRow.res == 'ok') {
        this.setBusy(true);
        this.genericDataService.getEntitiesWithAction(entityName,
          actionName + '?factorId=' + this.factorId
          , null)
          .subscribe({
            next: (result: any) => {
              this.setBusy(false);
              this.afterUpdateDone.emit();
              if (result.isSuccess) {
                this.showMessage(result.message);
              } else {
                this.showError(result.message);
              }
            },
            error: () => {
              this.setBusy(false);
              this.afterUpdateDone.emit();
              this.showError('بروز خطا به هنگام انجام عمیات لطفا با مدیر سیستم تماس بگیرید.');
            }
          })
      }
    });
  }

}
