


import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
    selector: 'app-product-category-detail-tab-based',
    templateUrl: './product-category-detail-tab-based.component.html'
})
export class ProductCategoryDetailTabBasedComponent extends Editable implements OnInit {
             			    productId:number;
						    categoryId:number;

    allowEditDesc: boolean;
    tabs: any[] = [];
    constructor(private genericDataService: GenericDataService) {
        super();
        this.entityName = 'productCategory';
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
    bindTabs(){

    this.tabs = [
    {id: 1, title: 'دسته بندی محصول', template: 'productCategory'},
 ];

}


}


