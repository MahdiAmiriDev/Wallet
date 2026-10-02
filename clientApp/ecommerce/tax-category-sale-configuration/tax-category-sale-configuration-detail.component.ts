import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {Observable} from "rxjs";


@Component({
  selector: 'app-tax-category-sale-configuration-detail',
  templateUrl: './tax-category-sale-configuration-detail.component.html'
})
export class TaxCategorySaleConfigurationDetailComponent extends BaseEdit implements OnInit {

  @Input()
  parentId: number;
  @Input()
  taxCategoryId: number;
  @Input()
  saleConfigurationId: number;
  @Input()
  factorReportType: number;


  taxCategorySaleConfigurationDataSource: any;
  parentDataSource: any;

  taxCategoryDataSource: any;

  saleConfigurationDataSource: any;

  factorReportTypes: any;


  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'taxCategorySaleConfiguration';
  }

  ngOnInit(): void {
    super.ngOnInit();

    this.entity.saleConfigurationId = this.saleConfigurationId;
    if (this.parentId)
      this.entity.parentId = this.parentId;
    if (this.taxCategoryId)
      this.entity.taxCategoryId = this.taxCategoryId;
    if (this.saleConfigurationId)
      this.entity.saleConfigurationId = this.saleConfigurationId;

    if (this.factorReportType)
      this.entity.factorReportType = this.factorReportType;

    this.createCustomDataSourceForTaxCategory();

    this.taxCategorySaleConfigurationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'taxCategorySaleConfiguration');


    this.saleConfigurationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'saleConfiguration');


    this._genericDataService.getCommonBaseDataForSelect('FactorReportType').subscribe(res => this.factorReportTypes = res);


  }

  // در حالت ثبت یا ذخیره فرم  عنوان به صورت پیش فرض اجباری و نال هست ، این متد قبل از ذخیره برای عنوان مقدار قرار می دهد
  doBeforeSubmit(e): Observable<boolean> {
    this.entity.title = 'taxCategoryTitle';
    return super.doBeforeSubmit(e);
  }
  //در صورت موجود بودن
  //selectedEntityId (اگر در حالت ویرایش باشیم)
  //api
  //را کال می کند
  //و selectedEntityId
  // را در انتهای آن قرار میدهد
  // در غیر این صورت پارامتر
  //taxCategorySaleConfigurationId
  //را ارسال نمی کند

  createCustomDataSourceForTaxCategory() {
    if (this.selectedEntityId) {
      this.taxCategoryDataSource = this._genericDataService.createCustomDataSourceWithCustomUrl(`id`,
        `TaxCategory/GetTaxCategoryNotUsedInTaxCategorySaleConfigurationForSelect?saleConfigurationId=${this.saleConfigurationId}&taxCategorySaleConfigurationId=${this.selectedEntityId}`);
    } else {
      this.taxCategoryDataSource = this._genericDataService.createCustomDataSourceWithCustomUrl(`id`,
        `TaxCategory/GetTaxCategoryNotUsedInTaxCategorySaleConfigurationForSelect?saleConfigurationId=${this.saleConfigurationId}`);
    }

  }
}
