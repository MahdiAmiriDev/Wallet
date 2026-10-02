					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		import {ContractPaymentMethodDetailComponent} from './contract-payment-method-detail.component';



		@Component({
		  selector: 'app-contract-payment-method-list',
		  templateUrl: './contract-payment-method-list.component.html'
		})
		export class ContractPaymentMethodListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'contractPaymentMethod';
		  detailComponent =ContractPaymentMethodDetailComponent;
		 			_contractId:number;
			@Input()
			set contractId(val){
				if(val!==this._contractId){
					this._contractId=val;
				}
			}
			get contractId(): number {
					return this._contractId;
				}
						_paymentMethodType:number;
			@Input()
			set paymentMethodType(val){
				if(val!==this._paymentMethodType){
					this._paymentMethodType=val;
				}
			}
			get paymentMethodType(): number {
					return this._paymentMethodType;
				}
					  private baseListUrl = 'contractPaymentMethod/GetByCustomFilterForList?';
		  constructor() {
			super();
							this.columns.push({
					caption: 'قرارداد',
					dataField: 'contractTitle'
					});
							this.columns.push({
					caption: 'روش پرداخت',
					dataField: 'paymentMethodTypeTitle'
					});
							this.columns.push({
					caption: 'مبتنی بر قانون',
					dataField: 'applyByExpression'
					});
							this.columns.push({
					caption: 'قانون',
					dataField: 'expression'
					});

				}
			ngOnInit(): void {
					if(this.bindCustomDataSources()){
						this.loadAfterSetFilter=true;
					}
				}

	public bindCustomDataSources() {
				let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {
							if(this.contractId){
					customListUrl += `contractId=${this.contractId}&`;
				}
							if(this.paymentMethodType){
					customListUrl += `paymentMethodType=${this.paymentMethodType}&`;
				}
					}
		let res=false;
		if(this.contractId || this.paymentMethodType){
			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
			res=true;
		}
		return res;
    }
    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<ContractPaymentMethodDetailComponent>componentInstance.instance);

			if(this.contractId){
				comp.contractId = this.contractId;
			}

			if(this.paymentMethodType){
				comp.paymentMethodType = this.paymentMethodType;
			}

    }
}
