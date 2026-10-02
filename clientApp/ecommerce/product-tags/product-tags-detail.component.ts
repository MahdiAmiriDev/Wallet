import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-product-tags-detail',
  templateUrl: './product-tags-detail.component.html'
})
export class ProductTagsDetailComponent extends BaseEdit implements OnInit {

	@Input()
	parentId:number;
	@Input()
	productId:number;
	@Input()
	tagType:number;


  productTagsDataSource: any;
	parentDataSource:any;
	productDataSource:any;
	tagTypes: any;
	accessLevels: any[] = [];



	constructor(private genericDataService: GenericDataService) {
	super(genericDataService);
	this.entityName = 'productTags';
	}

  ngOnInit() : void{
    super.ngOnInit();


		if(this.parentId)
			this.entity.parentId=this.parentId;
		if(this.productId)
			this.entity.productId=this.productId;
		if(this.tagType)
			this.entity.tagType=this.tagType;

			this.productTagsDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'productTags');


			this.productDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'product');


			this._genericDataService.getCommonBaseDataForSelect('TagType').subscribe(res => this.tagTypes = res);


			this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
  }
}
