import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {YashilCustomButton} from '../../../shared/base/models/yasil-custom-button';

@Component({
  selector: 'app-factor-group-convert-item-list',
  templateUrl: './factor-group-convert-list.component.html',
})
export class FactorGroupConvertListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  columns: any[] = [];
  entityName = 'factorGroup';
  customButtons: YashilCustomButton[] = [];
  @Input()
  gridHeight: number;
  @Input()
  factorGroupId: number;
  private baseListUrl = 'factorGroup/GetFactorGroupConvert?';
  @Input()
  showDownloadReportBtn = true;

  constructor() {
    super();
  }

  ngOnInit(): void {
    if (!this.gridHeight) {
      this.gridHeight = window.innerHeight - 170;
    }
    this.bindColumns();
    if (this.showDownloadReportBtn) {
      this.bindCustomButtons();
    }
    this.bindCustomDataSources();
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'نام کالا',
      dataField: 'mainCategoryOrTaxTitle',
      allowHeaderFiltering: false,
      width: '150px',
    }, {
      caption: 'کد کالا',
      dataField: 'categoryOrTaxId',
      allowHeaderFiltering: false,
      width: '150px',
    }, {
      caption: 'واحد سنجش',
      dataField: 'unitOfMeasurement',
      allowHeaderFiltering: false,
      width: '150px',
    }, {
      caption: 'کد فاکتور',
      dataField: 'factorCode',
      width: '150px',
    }, {
      caption: 'مرکز',
      dataField: 'companyTitle',
      width: '100px',
    }, {
      caption: 'نمایندگی',
      dataField: 'representationTitle',
      width: '120px',
    }, {
      caption: 'کدتفصیلی مشتری',
      dataField: 'detailedCode',
      allowHeaderFiltering: false,
      width: '120px'
    }, {
      caption: 'تعداد وزارت',
      dataField: 'vezaratCount',
      allowHeaderFiltering: false,
      width: '90px',
    }, {
      caption: 'تعداد ساتا',
      dataField: 'sataCount',
      allowHeaderFiltering: false,
      width: '70px',
    }, {
      caption: 'تعداد پرداختی',
      dataField: 'totalOrderItemCount',
      allowHeaderFiltering: false,
      width: '90px',
    }, {
      caption: 'فی وزارت',
      dataField: 'feevezarat',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '130px',
    }, {
      caption: 'فی ساتا',
      dataField: 'feeSata',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '130px'
    }, {
      caption: 'فی پرداختی',
      dataField: 'feePayment',
      allowHeaderFiltering: false,
      cellTemplate: 'number',
      width: '150px'
    }, {
      caption: 'فی خرید',
      dataField: 'feePurchaseAmount',
      cellTemplate: 'number',
      allowHeaderFiltering: false,
      width: '150px'
    }, {
      caption: 'تاریخ تایید نهایی فاکتور',
      dataField: 'confirmDate',
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

  downloadFactorGroupConvertReport(factorGroupId: any): void {
    const url = `${window.location.origin}/api/factorGroup/GenerateFactorGroupConvertFile?factorGroupId=${factorGroupId}`;
    window.open(url, '_blank');
  }

  /*
  هر سطری که نام کالا ، کد کالا و یا واحد سنجش ان مقدار نداشته باشد را رنگ قرمز میکنیم
   */
  validateEmptyDataRowAndColorThem(item: any): void {
    if (item.rowType === 'data') {
      if (!item.data.mainCategoryOrTaxTitle ||
        !item.data.categoryOrTaxId
        || !item.data.unitOfMeasurement
        || item.data.mainCategoryOrTaxTitle.trim().length === 0
        || item.data.categoryOrTaxId.trim().length === 0
        || item.data.unitOfMeasurement.trim().length === 0) {
        item.rowElement.bgColor = '#ec5252';
        item.rowElement.className = item.rowElement.className.replace('dx-row-alt', '');
      }
    }
  }
}
