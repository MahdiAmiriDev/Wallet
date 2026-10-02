import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
    selector: 'app-tax-category-sale-configuration-detail-tab-based',
    templateUrl: './tax-category-sale-configuration-detail-tab-based.component.html'
})
export class TaxCategorySaleConfigurationDetailTabBasedComponent extends Editable implements OnInit {
             
			    parentId:number;
			 
			    taxCategoryId:number;
			 
			    saleConfigurationId:number;
			 
			    factorReportType:number;
			 

    allowEditDesc: boolean;
    tabs: any[] = [];
    constructor(private genericDataService: GenericDataService) {
        super();
        this.entityName = 'taxCategorySaleConfiguration';
    }

    ngOnInit() {
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
    bindTabs(){

    this.tabs = [
    {id: 1, title: 'تنظیم فروش دسته مالیاتی', template: 'taxCategorySaleConfiguration'},
 	{id: 2, title: 'توضیحات', template: 'description', disabled: !this.allowEditDesc},];

}


}


