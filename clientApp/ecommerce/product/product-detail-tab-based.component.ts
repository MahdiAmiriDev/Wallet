import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-product-detail-tab-based',
  templateUrl: './product-detail-tab-based.component.html'
})
export class ProductDetailTabBasedComponent extends Editable implements OnInit {
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
        {id: 1, title: 'توضیحات کوتاه', template: 'shortDescription', disabled: !this.allowEditDesc},
        {id: 2, title: 'توضیحات کامل', template: 'fullDescription', disabled: !this.allowEditDesc},
        {id: 3, title: 'توضیح مدیر', template: 'adminComment', disabled: !this.allowEditDesc},
        {id: 4, title: 'توضیحات', template: 'metaDescription', disabled: !this.allowEditDesc},
        {id: 5, title: 'متن توافقنامه کاربری', template: 'userAgreementText', disabled: !this.allowEditDesc},
      ];
    }else{
      this.tabs[1].disabled=!this.allowEditDesc;
      this.tabs[2].disabled=!this.allowEditDesc;
      this.tabs[3].disabled=!this.allowEditDesc;
      this.tabs[4].disabled=!this.allowEditDesc;
      this.tabs[5].disabled=!this.allowEditDesc;
    }

  }


}


