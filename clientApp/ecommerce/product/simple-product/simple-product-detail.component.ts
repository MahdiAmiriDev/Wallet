import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';

@Component({
  selector: 'app-simple-product-detail',
  templateUrl: './simple-product-detail.component.html'
})
export class ProductSimpleDetailComponent extends BaseEdit implements OnInit {
  @Input()
  companyId: number;
  @Input()
  appEntityId: number;
  @Input()
  productTemplateId: number;
  @Input()
  giftCardTypeId: number;
  @Input()
  downloadActivationType: number;
  @Input()
  recurringCyclePeriodType: number;
  @Input()
  taxCategoryId: number;
  @Input()
  basePriceUnitType: number;
  @Input()
  basePriceBaseUnitType: number;
  companyDataSource: any;
  appEntityDataSource: any;
  productTemplateDataSource: any;
  giftCardTypeDataSource: any;
  downloadActivationTypes: any;
  salesModelTypes: any;
  recurringCyclePeriodTypes: any;
  taxCategoryDataSource: any;
  basePriceUnitTypes: any;
  basePriceBaseUnitTypes: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'product';
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.initDefaultFields();
    // if (!this.entity.categories) {
    //     this.entity.categories = [];
    // }
    if (this.companyId) {
      this.entity.companyId = this.companyId;
    }
    if (this.appEntityId) {
      this.entity.appEntityId = this.appEntityId;
    }
    if (this.productTemplateId) {
      this.entity.productTemplateId = this.productTemplateId;
    }
    if (this.giftCardTypeId) {
      this.entity.giftCardTypeId = this.giftCardTypeId;
    }
    if (this.downloadActivationType) {
      this.entity.downloadActivationType = this.downloadActivationType;
    }
    if (this.recurringCyclePeriodType) {
      this.entity.recurringCyclePeriodType = this.recurringCyclePeriodType;
    }
    if (this.taxCategoryId) {
      this.entity.taxCategoryId = this.taxCategoryId;
    }
    if (this.basePriceUnitType) {
      this.entity.basePriceUnitType = this.basePriceUnitType;
    }
    if (this.basePriceBaseUnitType) {
      this.entity.basePriceBaseUnitType = this.basePriceBaseUnitType;
    }

    this.companyDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'company');
    this.appEntityDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'appEntity');
    this.productTemplateDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'productTemplate');
    this.giftCardTypeDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'giftCardType');
    this.taxCategoryDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'taxCategory');
    this._genericDataService.getCommonBaseDataForSelect('DownloadActivationType')
      .subscribe(res => this.downloadActivationTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('SalesModelType')
      .subscribe(res => this.salesModelTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('RecurringCyclePeriodType')
      .subscribe(res => this.recurringCyclePeriodTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('BasePriceUnitType')
      .subscribe(res => this.basePriceUnitTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('BasePriceBaseUnitType')
      .subscribe(res => this.basePriceBaseUnitTypes = res);
  }

  selectedSpecificationCategoryChanged(item: any): void {
    this.entity.specificationCategoryId = item;
  }

  selectedCategoryChanged(e): void {
    this.entity.categories = [e];
  }

  selectedDependentProductsChanged(items): void {
    this.entity.dependentProducts = items;
  }

  private initDefaultFields(): void {
    this.entity.visibleIndividually = true;
    this.entity.salesModelType = 1087;
  }
}
