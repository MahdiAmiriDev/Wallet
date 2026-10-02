import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {createStore} from 'devextreme-aspnet-data-nojquery';
import {FormControl} from "@angular/forms";


@Component({
  selector: 'app-yashil-hekmat-terminal-supplier-detail',
  templateUrl: './yashil-hekmat-terminal-supplier-detail.component.html'
})
export class YashilHekmatTerminalSupplierDetailComponent extends BaseEdit implements OnInit {

	@Input()
	parentId:number;
	@Input()
	userId:number;


	parentDataSource:any;

	userDataSource:any;

	accessLevels: any[] = [];



	constructor(private genericDataService: GenericDataService) {
	super(genericDataService);
	this.entityName = 'yashilHekmatTerminalSupplier';
	}

  ngOnInit() : void{
    super.ngOnInit();


		if(this.parentId)
			this.entity.parentId=this.parentId;
		if(this.userId)
			this.entity.userId=this.userId;

			//this.yashilHekmatTerminalSupplierDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'yashilHekmatTerminalSupplier');


			this.userDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'user');


			this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);


  }
}
