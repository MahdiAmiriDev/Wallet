import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {YashilCustomButton} from '../../../shared/base/models/yasil-custom-button';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';

@Component({
  selector: 'app-factor-group-item-list',
  templateUrl: './factor-group-item-list.component.html',
})
export class FactorGroupItemListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  columns: any[] = [];
  entityName = 'factor';
  customButtons: YashilCustomButton[];
  @Input()
  gridHeight: number;
  @Input()
  factorGroupId: number;
  private baseListUrl = 'factor/GetFactorGroupItemList?';
  customRowButtons: YashilCustomButtonColumn;
  selectedFactorId: number;
  popUpTitle: string;
  showPopUp: boolean;

  constructor() {
    super();
    this.factorComparisonHekmatTransactions = this.factorComparisonHekmatTransactions.bind(this);

  }

  ngOnInit(): void {
    if (!this.gridHeight) {
      this.gridHeight = window.innerHeight - 280;
    }
    this.bindColumns();
    this.bindCustomButtons();
    this.bindCustomRowButtons();
    this.bindCustomDataSources();
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'آیا هزینه تبلیغات در فاکتور تامین کننده لحاظ شده است',
      dataField: 'isOtherDeductionsIncludeInSupplierInvoice',
      width: '100px',
    }, {
      caption: 'نام مرکز',
      dataField: 'companyTitle',
      width: '100px',
    }, {
      caption: 'نام نمایندگی',
      dataField: 'representationTitle',
      width: '120px',
    }, {
      caption: 'تخفیف(%)',
      dataField: 'discountPercentage',
      allowHeaderFiltering: false,
      width: '75px',
    }, {
      caption: 'سود(%)',
      dataField: 'profitPercentage',
      allowHeaderFiltering: false,
      width: '75px',
    }, {
      caption: 'کد مرکز یا نمایندگی',
      dataField: 'companyOrRepresentationCode',
      allowHeaderFiltering: false,
      width: '75px'
    }, {
      caption: 'کدفاکتور',
      dataField: 'code',
      allowHeaderFiltering: false,
      width: '150px',
    }, {
      caption: 'تعداد',
      dataField: 'factorItemCount',
      allowHeaderFiltering: false,
      width: '90px'
    }, {
      caption: 'مبلغ کل',
      dataField: 'sumTotal',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '120px',
    }, {
      caption: 'تخفیف',
      dataField: 'sumDiscountAmountExclTax',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '120px',
    }, {
      caption: 'سود',
      dataField: 'sumProfitPrice',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '120px',
    }, {
      caption: 'تخفیف فاکتور',
      dataField: 'sumDiscountWithProfitPriceWithOtherDeductionsPrice',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '120px',
    }, {
      caption: 'مبلغ کل باتخفیف در فاکتور',
      dataField: 'sumPriceExclDiscountExclProfitExclOtherDeductionsPrice',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '120px',
    }, {
      caption: 'هزینه ارسال',
      dataField: 'sumAdditionalShippingCharge',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'جمع مالیات و عوارض',
      dataField: 'sumTaxAmount',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'مبلغ کل با تخفیف بعلاوه جمع مالیات و عوارض و هزینه ارسال (مبلغ فروش موسسه)',
      dataField: 'sumDiscountedPriceSumTaxAmount',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'مبلغ بیمه',
      dataField: 'sumInsuranceAmount',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'هزینه تبلیغات',
      dataField: 'sumOtherDeductionsPrice',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'هزینه تبلیغات با مالیات',
      dataField: 'sumOtherDeductionsPriceIncTax',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'مبلغ پرداختی به تامین کننده',
      dataField: 'sumAmountPaidToSupplier',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'شماره حساب',
      dataField: 'accountNumber',
      allowHeaderFiltering: false,
      width: '160px'
    }, {
      caption: 'نام صاحب حساب',
      dataField: 'personBankAccountName',
      allowHeaderFiltering: false,
      width: '100px'
    }, {
      caption: 'نام خانوادگی صاحب حساب',
      dataField: 'personBankAccountLastName',
      allowHeaderFiltering: false,
      width: '100px'
    }, {
      caption: 'بانک',
      dataField: 'bankTypeTitle',
      width: '80px'
    }, {
      caption: 'کدتفصیلی مشتری',
      dataField: 'detailedCode',
      allowHeaderFiltering: false,
      width: '100px',
    }, {
      caption: 'کدملی صاحب حساب',
      dataField: 'personBankAccountNationalCode',
      allowHeaderFiltering: false,
      width: '100px',
    }, {
      caption: 'قیمت منهای تخفیف بعلاوه هزینه ارسال(قیمت فروش تامین کننده در سایت)',
      dataField: 'sumPriceIncDiscountWithAdditionalShippingCharge',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '100px',
    }, {
      caption: 'یارانه وزارت',
      dataField: 'sumVezaratSubsidy',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'یارانه وزارت با کارمزد',
      dataField: 'sumVezaratSubsidyPriceIncProfit',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'سهم وزارت (یارانه وزارت با کارمزد و مالیات)',
      dataField: 'vezaratPortion',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'یارانه ساتا',
      dataField: 'sumSataSubsidyPrice',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'یارانه ساتا با کارمزد',
      dataField: 'sumSataSataSubsidyPriceIncProfit',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'سهم ساتا(یارانه ساتا با کارمزد و مالیات)',
      dataField: 'sataPortion',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    },  {
      caption: 'قیمت پایه منهای تخفیف بعلاوه هزینه ارسال (قیمت فروش تامین کننده در سایت) منهای یارانه وزارت منهای یارانه ساتا',
      dataField: 'sumPriceIncDiscountWithAdditionalShippingChargeExclVezaratSubsidyExclSataSubsidy',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'کارمزد سایت',
      dataField: 'sumWageAmountExcTax',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'قیمت پایه منهای تخفیف بعلاوه هزینه ارسال (قیمت فروش تامین کننده در سایت) منهای یارانه وزارت منهای یارانه ساتا بعلاوه کارمزد',
      dataField: 'sumPriceIncDiscountWithAdditionalShippingChargeExclVezaratSubsidyExclSataSubsidyWithWageAmount',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    },{
      caption: 'سهم خانوار(مبلغ حکمت و آنلاین و ناجا با کارمزد و مالیات)',
      dataField: 'familyPortion',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '120px',
    }, {
      caption: 'تاریخ تایید نهایی فاکتور',
      dataField: 'factorConfirmDate',
      cellTemplate: 'intDate',
      allowHeaderFiltering: false,
      width: '120px'
    }, {
      caption: 'تاریخ گروه بندی',
      dataField: 'factorGroupConfirmDate',
      cellTemplate: 'intDate',
      allowHeaderFiltering: false,
      width: '120px'
    }, {
      caption: 'نوع پرداخت',
      dataField: 'paymentType',
      allowHeaderFiltering: false,
      width: '120px',
    }, {
      caption: 'سررسید اولین قسط',
      dataField: 'factorFirstInstallmentPayDate',
      cellTemplate: 'intDate',
      allowHeaderFiltering: false,
      width: '120px'
    }, {
      caption: 'اولین خرید',
      dataField: 'firstFactorItemDateInt',
      cellTemplate: 'intDate',
      allowHeaderFiltering: false,
      width: '120px'
    }, {
      caption: 'آخرین خرید',
      dataField: 'lastFactorItemDateInt',
      cellTemplate: 'intDate',
      allowHeaderFiltering: false,
      width: '120px'
    });
  }

  public bindCustomDataSources(): boolean {
    this.listForm.customListUrl = `${this.baseListUrl}` + `factorGroupId=${this.factorGroupId}`;
    this.listForm.refreshList();
    return true;
  }

  bindCustomButtons(): void {
    this.customButtons = [{
      text: 'دانلود گزارش',
      type: 'default',
      icon: 'xlsfile',
      key: this.factorGroupId
    }];
  }

  bindCustomRowButtons(): void {
    this.customRowButtons = {
      width: '200px',
      setAtFirstColumn: true,
      allowExporting: false,
      customButtons: [
        {
          text: 'اعتبارسنجی تراکنش حکمت',
          hint: 'اعتبار سنجی تراکنشات حکمت',
          icon: 'checklist',
          type: 'default',
          onClick: this.factorComparisonHekmatTransactions
        }]
    };
  }

  /*
  نمایش اعتبار سنجی تراکنشات حکمت مربوط به فاکتور
  در انی کامپوننت میتوان مغایرت ها را نیز مشاهده نمود
   */
  factorComparisonHekmatTransactions(item: any): void {// مقداردهی پارامترهای ورودی
    this.selectedFactorId = item.row.data.id;
    this.popUpTitle = 'اعتبار سنجی تراکنشات حکمت';
    this.showPopUp = true;
  }

  downloadFactorGroupItemReport(factorGroupId: any): void {
    const url = `${window.location.origin}/api/factorGroup/GenerateFactorGroupItemReportFile?factorGroupId=${factorGroupId}`;
    window.open(url, '_blank');
  }
}
