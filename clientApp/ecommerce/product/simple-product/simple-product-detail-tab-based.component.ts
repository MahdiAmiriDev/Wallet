import {Component, OnInit} from '@angular/core';
import {Editable} from '../../../../shared/base/classes/editable';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';

@Component({
  selector: 'app-simple-product-detail-tab-based',
  templateUrl: './simple-product-detail-tab-based.component.html'
})
export class SimpleProductDetailTabBasedComponent extends Editable implements OnInit {
  companyId: number;
  appEntityId: number;
  productTemplateId: number;
  giftCardTypeId: number;
  downloadActivationType: number;
  recurringCyclePeriodType: number;
  taxCategoryId: number;
  basePriceUnitType: number;
  basePriceBaseUnitType: number;

  allowEditDesc: boolean;
  tabs: any[] = [];
  selectedProduct: any;

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'product';
  }

  ngOnInit(): void {
    if (this.selectedEntityId && this.selectedEntityId !== 0) {
      this.allowEditDesc = true;
    } else {
      this.allowEditDesc = false;
    }
    this.bindTabs();
  }

  rowInserted(insertedRowId: any) {
    this.selectedEntityId = insertedRowId;
    this.allowEditDesc = true;
    this.bindTabs();
  }

  bindTabs() {
    if (this.tabs.length == 0) {
      this.tabs = [
        {id: 0, title: 'محصول', template: 'product'},
        {id: 1, title: 'ویژگی محصول', template: 'shortDescription', disabled: !this.allowEditDesc},
        {id: 2, title: 'برنامه ریزی فروش', template: 'salePlanning', disabled: !this.allowEditDesc},
        {id: 3, title: 'تصاویر محصول', template: 'pics', disabled: !this.allowEditDesc},
        {id: 4, title: 'فروش محصولات وابسته', template: 'dependentProductSale', disabled: !this.allowEditDesc}
      ];
    } else {
      this.tabs[1].disabled = !this.allowEditDesc;
      this.tabs[2].disabled = !this.allowEditDesc;
      this.tabs[3].disabled = !this.allowEditDesc;
      this.tabs[4].disabled = !this.allowEditDesc;
    }

  }


  dataLoaded(item: any) {
    this.selectedProduct = item;
  }
}


