import {Component, OnInit, ViewChild} from '@angular/core';
import {Selectable} from "../../../../shared/base/classes/selectable";
import {YashilCustomButtonColumn} from "../../../../shared/base/classes/yashil-cutom-button-column";
import {GenericDataService} from "../../../../shared/base/services/generic-data.service";
import {BaseList} from "../../../../shared/base/classes/base-list";


@Component({
  selector: 'app-converted-factor-component-list',
  templateUrl: './converted-factor-list.component.html'
})

export class ConvertedFactorListComponent extends Selectable implements OnInit {

  @ViewChild('listForm', {static: true}) listForm: BaseList;

  columns: any[] = [];
  customRowButtons: YashilCustomButtonColumn;
  activeCheckingFactorItemPopup: boolean = false;
  userDataSource: any;
  factorInstallmentCount: number;
  selectedFactorGroupId: number;
  customListUrl: string = 'factor/GetConvertedFactors';
  selectedFactorId: number;

  constructor(private genericDataService: GenericDataService) {
    super();
    this.showCheckingFactorItemPopup = this.showCheckingFactorItemPopup.bind(this);
    this.onSelectedFactorGroupChange = this.onSelectedFactorGroupChange.bind(this);
  }

  ngOnInit() {
    this.bindColumns();
    this.bindCustomRowButtons();
    this.userDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'factorGroup');
  }

  //بایند کردن ستون های لیست داده
  bindColumns(): void {
    this.columns.push({
        caption: 'کد',
        dataField: 'code',
        width: '150px',
      }, {
        caption: 'عنوان',
        dataField: 'title',
        width: '170px',
      }
      ,
      {
        caption: 'دسته مالیاتی',
        dataField: 'taxCategoryTitle',
        width: '120px',
      },
      {
        caption: 'نمایندگی',
        dataField: 'representationTitle',
        width: '120px',
      }
      , {
        caption: 'قرارداد',
        dataField: 'saleConfigurationTitle',
        width: '150px',
      }, {
        caption: 'سود(%)',
        dataField: 'profitPercentage',
        width: '70px',
      }, {
        caption: 'تخفیف(%)',
        dataField: 'discountPercentage',
        width: '80px',
      }, {
        caption: 'تاریخ صدور',
        dataField: 'issueDate',
        cellTemplate: 'intDate',
        width: '120px'
      }, {
        caption: 'وضعیت',
        dataField: 'factorReviewStatusTypeTitle',
        width: '95px'
      });
  }

  //بایند کردن دکمه ی مربوط به هر  ردیف داده
  bindCustomRowButtons(): void {
    this.customRowButtons = {
      width: '150px',
      customButtons: [{
        text: 'بررسی ایتم های سفارش',
        onClick: this.showCheckingFactorItemPopup,
        type: 'default'
      }]
    }
  }

  //زمانی که کاربر بر روی یکی از دکمه های بررسی ایتم های سفارش کلیک می کند این متد فراخوانی می شود.
  showCheckingFactorItemPopup(selectedRow: any): void {
    if (selectedRow?.data?.factorInstallmentCount){
      this.factorInstallmentCount = selectedRow.data.factorInstallmentCount;
      this.selectedFactorId = selectedRow.data.id;
      this.activeCheckingFactorItemPopup = true;
    }
  }


  //زمانی که کاربر گروه فاکتور جدیدی انتخاب می کند این متد فراخوانی می شود.
  onSelectedFactorGroupChange(selectedFactorGroup: any): void {
    if (selectedFactorGroup?.value) {
      this.selectedFactorGroupId = selectedFactorGroup?.value;
      this.bindCustomDataSources();
      return;
    }
    this.listForm.customListUrl = this.customListUrl;
    this.listForm.refreshList();
    return;
  }


  //زمانی که لازم باشد به فرم لیست آدرس api دلخواه بدیهم به عنوان منبع داده این متد را فراخوانی می کنیم.
  bindCustomDataSources(): boolean {
    if (this.selectedFactorGroupId) {
      this.listForm.customListUrl = this.customListUrl + '?factorGroupId=' + this.selectedFactorGroupId;
      this.listForm.refreshList();
      return true;
    }
    return true;
  }

  //بعد از این که عملیات بروزرسانی تمام می شود این متد فراخوانی می شود.
  onAfterUpdateDone(): void {
    this.listForm.refreshList();
  }
}
