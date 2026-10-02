import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-refund-payment-transaction-item-request-detail',
  templateUrl: './refund-payment-transaction-item-request-detail.component.html'
})
export class RefundPaymentTransactionItemRequestDetailComponent extends BaseEdit implements OnInit {

	@Input()
	parentId:number;
	@Input()
	yashilHekmatTerminalId:number;
	@Input()
	refundPaymentTransactionItemRequestType:number;
	@Input()
	refundPaymentTransactionItemRequestStatus:number;


	parentDataSource:any;

	yashilHekmatTerminalDataSource:any;

	refundPaymentTransactionItemRequestTypes: any;

	refundPaymentTransactionItemRequestStatuss: any;

	accessLevels: any[] = [];



	constructor(private genericDataService: GenericDataService) {
	super(genericDataService);
	this.entityName = 'refundPaymentTransactionItemRequest';
	}

  ngOnInit() : void{
    super.ngOnInit();


		if(this.parentId)
			this.entity.parentId=this.parentId;
		if(this.yashilHekmatTerminalId)
			this.entity.yashilHekmatTerminalId=this.yashilHekmatTerminalId;
		if(this.refundPaymentTransactionItemRequestType)
			this.entity.refundPaymentTransactionItemRequestType=this.refundPaymentTransactionItemRequestType;
		if(this.refundPaymentTransactionItemRequestStatus)
			this.entity.refundPaymentTransactionItemRequestStatus=this.refundPaymentTransactionItemRequestStatus;

			//this.refundPaymentTransactionItemRequestDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'refundPaymentTransactionItemRequest');


			this.yashilHekmatTerminalDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'yashilHekmatTerminal');


			this._genericDataService.getCommonBaseDataForSelect('RefundPaymentTransactionItemRequestType').subscribe(res => this.refundPaymentTransactionItemRequestTypes = res);


			this._genericDataService.getCommonBaseDataForSelect('RefundPaymentTransactionItemRequestStatus').subscribe(res => this.refundPaymentTransactionItemRequestStatuss = res);


			this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);


  }
}
