		

import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {createStore} from 'devextreme-aspnet-data-nojquery';


@Component({
  selector: 'app-hekmat-transaction-detail',
  templateUrl: './hekmat-transaction-detail.component.html'
})
export class HekmatTransactionDetailComponent extends BaseEdit implements OnInit {
						@Input()
			hekmatTransactionStatusType:number;
									hekmatTransactionStatusTypes: any;
		accessLevels: any[] = [];
	  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'hekmatTransaction';
  }

  ngOnInit() :void{
    super.ngOnInit();
				if(this.hekmatTransactionStatusType)
				this.entity.hekmatTransactionStatusType=this.hekmatTransactionStatusType;
			
							this._genericDataService.getCommonBaseDataForSelect('HekmatTransactionStatusType').subscribe(res => this.hekmatTransactionStatusTypes = res);
							this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
		  }      
}
