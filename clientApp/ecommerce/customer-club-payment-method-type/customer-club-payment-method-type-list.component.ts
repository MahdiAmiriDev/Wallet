					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		import {CustomerClubPaymentMethodTypeDetailComponent} from './customer-club-payment-method-type-detail.component';



		@Component({
		  selector: 'app-customer-club-payment-method-type-list',
		  templateUrl: './customer-club-payment-method-type-list.component.html'
		})
		export class CustomerClubPaymentMethodTypeListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'customerClubPaymentMethodType';
		  detailComponent =CustomerClubPaymentMethodTypeDetailComponent;
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
						_customerClubId:number;
			@Input()
			set customerClubId(val){
				if(val!==this._customerClubId){
					this._customerClubId=val;
				}
			}
			get customerClubId(): number {
					return this._customerClubId;
				}
					  private baseListUrl = 'customerClubPaymentMethodType/GetByCustomFilterForList?';
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
							this.columns.push({
					caption: 'شناسه باشگاه مشتریان',
					dataField: 'customerClubTitle'
					});
							this.columns.push({
					caption: 'عنوان',
					dataField: 'title'
					});
				}

	public bindCustomDataSources() {
				let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {
							if(this.paymentMethodType){
					customListUrl += `paymentMethodType=${this.paymentMethodType}&`;
				}
							if(this.customerClubId){
					customListUrl += `customerClubId=${this.customerClubId}&`;
				}
					}
				let res=false;
		if(this.paymentMethodType || this.customerClubId){

			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
					res=true;
		}
			return res;

    }
    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<CustomerClubPaymentMethodTypeDetailComponent>componentInstance.instance);

			if(this.paymentMethodType){
				comp.paymentMethodType = this.paymentMethodType;
			}

			if(this.customerClubId){
				comp.customerClubId = this.customerClubId;
			}

    }
}
