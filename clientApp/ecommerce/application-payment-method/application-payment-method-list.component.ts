					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		import {ApplicationPaymentMethodDetailComponent} from './application-payment-method-detail.component';



		@Component({
		  selector: 'app-application-payment-method-list',
		  templateUrl: './application-payment-method-list.component.html'
		})
		export class ApplicationPaymentMethodListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'applicationPaymentMethod';
		  detailComponent =ApplicationPaymentMethodDetailComponent;
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
					  private baseListUrl = 'applicationPaymentMethod/GetByCustomFilterForList?';
		  constructor() {
			super();
				}
			ngOnInit(): void {
					this.bindColumns();
					if(this.bindCustomDataSources()){
						this.loadAfterSetFilter=true;
					}
				}
	private bindColumns(){
					this.columns.push({
					caption: 'روش پرداخت',
					dataField: 'paymentMethodTypeTitle'
					});
				}

	public bindCustomDataSources() {
				let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {
							if(this.paymentMethodType){
					customListUrl += `paymentMethodType=${this.paymentMethodType}&`;
				}
					}
				let res=false;
		if(this.paymentMethodType){

			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
					res=true;
		}
			return res;

    }
    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<ApplicationPaymentMethodDetailComponent>componentInstance.instance);

			if(this.paymentMethodType){
				comp.paymentMethodType = this.paymentMethodType;
			}

    }
}
