import {Selectable} from "../../../shared/base/classes/selectable";
import {Component, Input, OnInit, ViewChild} from "@angular/core";
import {BaseList} from "../../../shared/base/classes/base-list";
import {YashilCustomButton} from "../../../shared/base/models/yasil-custom-button";
import {GenericDataService} from '../../../shared/base/services/generic-data.service';

@Component({
  selector: 'app-factor-payment-installment-list',
  templateUrl: './factor-payment-installment-list.component.html'
})
export class FactorPaymentInstallmentListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  columns: any[] = [];
  entityName = 'factorPaymentInstallment'
  private baseListUrl = 'factorPaymentInstallment/getFactorPaymentInstallment?'
  @Input()
  yearMonth: number;
  @Input()
  categoryGroupId: number;
  selectedFactorPaymentInstallment: [];
  customButtons: YashilCustomButton[];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.bindCustomButton = this.bindCustomButton.bind(this);
  }

  ngOnInit() {
    this.bindColumns();
    this.bindCustomButton();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  public bindCustomDataSources(): boolean {
    let customerListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.yearMonth) {
        customerListUrl += `yearMonth=${this.yearMonth}&`;
      }
      if (this.categoryGroupId) {
        customerListUrl += `categoryGroupId=${this.categoryGroupId}&`;
      }
    }
    let res = false;
    if (this.yearMonth || this.categoryGroupId) {
      this.listForm.customListUrl = customerListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }

  bindColumns(): void {
    this.columns = [
      {
        caption: 'نام طرح',
        dataField: 'categoryGroupTitle',
        visibleIndex: 0,
        alignment: 'right'
      },
      {
        caption: 'نام مرکز و نمایندگی',
        dataField: 'companyOrRepresentationTitle',
        visibleIndex: 0,
        alignment: 'right'

      },
      {
        caption: 'کد نمایندگی یا مرکز',
        dataField: 'companyOrRepresentationCode',
        visibleIndex: 0,
        alignment: 'right'
      },
      {
        caption: 'مبلغ قسط',
        dataField: 'installmentAmount',
        visibleIndex: 0,
        alignment: 'right'
      },
      {
        caption: 'شماره قسط',
        dataField: 'installmentNumber',
        visibleIndex: 0,
        alignment: 'right'
      },
    ]
  }

  bindCustomButton() {
    this.customButtons = [{
      text: 'دریافت گزارش اقساط فاکتور انتخابی',
      type: 'primary',
      key: 'exportToExcelSelectedFactorPaymentInstallment',
    }]
  }

  /*
  وقتی کاربر ردیفی را انتخاب می کند
  شناسه تقسیط فاکتور در متغیر اقساط فاکتور پرداختی قرار داده می شود
   */
  onRowClicked(selectedInstallment: any) {
    this.selectedFactorPaymentInstallment = selectedInstallment;
  }

  customButtonClicked(key: any) {
    if (
      key === "exportToExcelSelectedFactorPaymentInstallment" &&
      this.selectedFactorPaymentInstallment?.length > 0
    ) {
      this.setBusy(true);

      const dto = {
        ids: this.selectedFactorPaymentInstallment,
        yearMonth: this.yearMonth
      };

      this.genericDataService
        .postAndDownload(
          'api/FactorPaymentInstallment/ExportToExcelFactorPaymentInstallment',
          dto
        )
        .subscribe({
          next: (result) => {
            this.downloadFile(result.blob, result.fileName);
            this.setBusy(false);
          },
          error: (err) => {
            console.error(err);
            this.setBusy(false);
          }
        });
    }
  }

  downloadFile(blob: Blob, fileName: string): void {
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
  }
}
