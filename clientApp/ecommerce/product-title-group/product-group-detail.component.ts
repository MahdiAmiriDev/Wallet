import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-product-group-detail',
  templateUrl: './product-group-detail.component.html'
})
export class ProductGroupDetailComponent extends BaseEdit implements OnInit {

	@Input()
	parentId:number;
	@Input()
	categoryGroupId:number;


	parentDataSource:any;

	accessLevels: any[] = [];

	categoryGroupDataSource:any;



	constructor(private genericDataService: GenericDataService) {
	super(genericDataService);
	this.entityName = 'productGroup';
	}

  ngOnInit() : void{
    super.ngOnInit();


		if(this.parentId)
			this.entity.parentId=this.parentId;
		if(this.categoryGroupId)
			this.entity.categoryGroupId=this.categoryGroupId;

			this.categoryGroupDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'categoryGroup');


  }
}
