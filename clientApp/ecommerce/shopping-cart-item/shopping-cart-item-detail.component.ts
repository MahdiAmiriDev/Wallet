

import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-shopping-cart-item-detail',
  templateUrl: './shopping-cart-item-detail.component.html'
})
export class ShoppingCartItemDetailComponent extends BaseEdit implements OnInit {
						@Input()
			representationId:number;
						@Input()
			shoppingCartType:number;
						@Input()
			productSalesPlanningId:number;
					 representationDataSource:any;
	 					shoppingCartTypes: any;
	 productSalesPlanningDataSource:any;
	   constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'shoppingCartItem';
  }

  ngOnInit(): void {
    super.ngOnInit();
				if(this.representationId)
				this.entity.representationId=this.representationId;
						if(this.shoppingCartType)
				this.entity.shoppingCartType=this.shoppingCartType;
						if(this.productSalesPlanningId)
				this.entity.productSalesPlanningId=this.productSalesPlanningId;

							this.representationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'representation');
									this._genericDataService.getCommonBaseDataForSelect('ShoppingCartType').subscribe(res => this.shoppingCartTypes = res);
									this.productSalesPlanningDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'productSalesPlanning');
				  }
}
