import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';
import {FactorReportTypeEnums} from '../factor/factor-report-type-enums';
import {observable, Observable} from 'rxjs';
import {FormControl} from '@angular/forms';

@Component({
  selector: 'app-contract-detail',
  templateUrl: './contract-detail.component.html'
})
export class ContractDetailComponent extends BaseEdit implements OnInit {
  licenseExpireDateDataValue = new FormControl();
  arrangementDateDataValue = new FormControl();
  fromDateDataValue = new FormControl();
  toDateDataValue = new FormControl();

  @ViewChild(DxTreeViewComponent) treeView;

  @Input()
  companyId: number;
  @Input()
  representationId: number;
  @Input()
  taxCategoryId: number;
  @Input()
  contractType: number;
  @Input()
  contractStatus: number;
  companyDataSource: any;
  representationDataSource: any;
  taxCategoryDataSource: any;
  contractTypes: any;
  contractStatuss: any;
  addressDataSource: any;
  accessLevels: any[] = [];
  factorReportTypes: any;
  cityDataSource: any;
  errorStyle: any = {};
  callbacks = [];
  invalidCss = '1px solid red';
  dateFromValidationAdapter = {
    getValue: () => {
      return this.entity.saleConfiguration.fromDate;
    },
    applyValidationResults: (e) => {
      this.errorStyle.dateFromValidationCss = e.isValid ? 'none' : this.invalidCss;
    },
    validationRequestsCallbacks: this.callbacks
  };
  dateToValidationAdapter = {
    getValue: () => {
      return this.entity.saleConfiguration.toDate;
    },
    applyValidationResults: (e) => {
      this.errorStyle.dateToValidationCss = e.isValid ? 'none' : this.invalidCss;
    },
    validationRequestsCallbacks: this.callbacks
  };
  licenseExpireDateValidationAdapter = {
    getValue: () => {
      return this.entity.saleConfiguration.licenseExpireDate;
    },
    applyValidationResults: (e) => {
      this.errorStyle.licenseExpireDateValidationCss = e.isValid ? 'none' : this.invalidCss;
    },
    validationRequestsCallbacks: this.callbacks
  };
  arrangementDateValidationAdapter = {
    getValue: () => {
      return this.entity.arrangementDate;
    },
    applyValidationResults: (e) => {
      this.errorStyle.arrangementDateValidationCss = e.isValid ? 'none' : this.invalidCss;
    },
    validationRequestsCallbacks: this.callbacks
  };
  withoutDetailFactorReportTypeSelected: boolean;


  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'contract';
    this.selectedCityChanged = this.selectedCityChanged.bind(this);
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.companyId) {
      this.entity.companyId = this.companyId;
    }
    if (this.representationId) {
      this.entity.representationId = this.representationId;
    }
    if (this.taxCategoryId) {
      this.entity.taxCategoryId = this.taxCategoryId;
    }
    if (this.contractType) {
      this.entity.contractType = this.contractType;
    }
    if (this.contractStatus) {
      this.entity.contractStatus = this.contractStatus;
    }
    this.cityDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'city');
    this.companyDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'company');
    this.representationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'representation');
    this.taxCategoryDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'taxCategory');
    this._genericDataService.getCommonBaseDataForSelect('ContractType').subscribe(res => this.contractTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('ContractStatus').subscribe(res => this.contractStatuss = res);
    this.addressDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'address');
    this._genericDataService.getCommonBaseDataForSelect('FactorReportType').subscribe(res => this.factorReportTypes = res);
    if (!this.selectedEntityId) {
      this._genericDataService.getCustomEntitiesByUrl('/api/SaleConfiguration/GetApplicationDefaultIsGroupedByTaxCategory')
        .subscribe(res => {
          this.entity.saleConfiguration.isGroupedByTaxCategory = res;
        });
      this._genericDataService.getEntitiesWithAction('TaxCategory', 'GetDefaultTaxValuesForFactor', null)
        .subscribe((res: any) => {
          if (res && res.data) {
            const tax = res.data.find(e => e.percentage > 0);
            const taxExempt = res.data.find(e => e.percentage === 0);
            this.entity.saleConfiguration.taxTitleForFactor = tax.titleForConvert;
            this.entity.saleConfiguration.taxCodeForFactor = tax.code;
            this.entity.saleConfiguration.taxUnitOfMeasurementForFactor = tax.unitOfMeasureMent;
            this.entity.saleConfiguration.taxExemptTitleForFactor = taxExempt.titleForConvert;
            this.entity.saleConfiguration.taxExemptCodeForFactor = taxExempt.code;
            this.entity.saleConfiguration.taxExemptUnitOfMeasurementForFactor = taxExempt.unitOfMeasureMent;
          }
        });
    }

  }

  reValidate(): void {
    this.callbacks.forEach(func => {
      func();
    });
  }

  selectedPersonEconomicProfileChanged(selectedPersonEconomicProfile: any): void {
    this.entity.saleConfiguration.founderId = selectedPersonEconomicProfile.id;
  }

  selectedPersonBankAccountChanged(selectedPersonBankAccountId: any): void {
    this.entity.saleConfiguration.personBankAccountId = selectedPersonBankAccountId;
  }

  selectedCityChanged(selectedCity: any): void {
    this.entity.address.cityId = selectedCity;
  }

  doBeforeSubmit(e): Observable<boolean> {
    this.entity.saleConfiguration.title = this.entity.title;
    if (!this.entity.saleConfiguration.founderId || this.entity.saleConfiguration.founderId == 0) {
      this.showError('کاربر گرامی، لطفا موسس را انتخاب نمایید.');
      return new Observable<boolean>(c => c.next(false));
    }
    return super.doBeforeSubmit(e);
  }


  beforeLoadData(): void {
    this.entity.saleConfiguration = {};
    this.entity.address = {};
    super.beforeLoadData();
  }

  syncTreeViewSelection(e): void {
    const component = (e && e.component) || (this.treeView && this.treeView.instance);

    if (!component) {
      return;
    }
  }

  treeView_itemSelectionChanged(e): void {
    if (e.itemData.parentId) {
      this.entity.address.cityId = e.itemData.id;
    }
  }

  onSelectedCompanyChanged(selectedItem: any): void {

    this.entity.saleConfiguration.companyId = selectedItem.company.id;
    if (selectedItem.company) {
      this.entity.saleConfiguration.companyId = selectedItem.company.id;
    } else {
      this.entity.saleConfiguration.companyId = null;
      this.entity.saleConfiguration.representationId = null;
    }
  }

  onSelectedRepresentationChanged(selectedItem: any): void {
    if (selectedItem.representation) {
      this.entity.saleConfiguration.representationId = selectedItem.representation.id;
    } else {
      this.entity.saleConfiguration.representationId = null;
    }
  }

  selectedSchedulerChanged(selectedYashilSchedulerId: any): void {
    this.entity.saleConfiguration.yashilSchedulerId = selectedYashilSchedulerId;
  }

  checkValidationByFactorReportType(item: any): void {
    this.withoutDetailFactorReportTypeSelected = false;
    if (item && item.value && item.value === FactorReportTypeEnums.WithoutDetail) {
      this.withoutDetailFactorReportTypeSelected = true;
    }
  }
}
