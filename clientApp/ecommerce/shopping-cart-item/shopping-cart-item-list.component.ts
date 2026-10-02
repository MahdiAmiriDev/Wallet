					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		import {ShoppingCartItemDetailComponent} from './shopping-cart-item-detail.component';



		@Component({
		  selector: 'app-shopping-cart-item-list',
		  templateUrl: './shopping-cart-item-list.component.html'
		})
		export class ShoppingCartItemListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'shoppingCartItem';
		  detailComponent =ShoppingCartItemDetailComponent;
		 			_representationId:number;
			@Input()
			set representationId(val){
				if(val!==this._representationId){
					this._representationId=val;
				}
			}
			get representationId(): number {
					return this._representationId;
				}
						_shoppingCartType:number;
			@Input()
			set shoppingCartType(val){
				if(val!==this._shoppingCartType){
					this._shoppingCartType=val;
				}
			}
			get shoppingCartType(): number {
					return this._shoppingCartType;
				}
						_productSalesPlanningId:number;
			@Input()
			set productSalesPlanningId(val){
				if(val!==this._productSalesPlanningId){
					this._productSalesPlanningId=val;
				}
			}
			get productSalesPlanningId(): number {
					return this._productSalesPlanningId;
				}
					  private baseListUrl = 'shoppingCartItem/GetByCustomFilterForList?';
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
					caption: 'نمایندگی',
					dataField: 'representationTitle'
					});
							this.columns.push({
					caption: 'کاربر',
					dataField: 'shoppingCartTypeTitle'
					});
							this.columns.push({
					caption: 'برنامه ریزی فروش',
					dataField: 'productSalesPlanningTitle'
					});
							this.columns.push({
					caption: 'قیمت مشتری',
					dataField: 'customerEnteredPrice'
					});
							this.columns.push({
					caption: 'تعداد',
					dataField: 'quantity'
					});
							this.columns.push({
					caption: 'تاریخ شروع اجاره',
					dataField: 'rentalStartDateUtc'
					});
							this.columns.push({
					caption: 'تاریخ پایان اجاره',
					dataField: 'rentalEndDateUtc'
					});
				}

	public bindCustomDataSources() {
				let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {
							if(this.representationId){
					customListUrl += `representationId=${this.representationId}&`;
				}
							if(this.shoppingCartType){
					customListUrl += `shoppingCartType=${this.shoppingCartType}&`;
				}
							if(this.productSalesPlanningId){
					customListUrl += `productSalesPlanningId=${this.productSalesPlanningId}&`;
				}
					}
				let res=false;
		if(this.representationId || this.shoppingCartType || this.productSalesPlanningId){

			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
					res=true;
		}
			return res;

    }
    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<ShoppingCartItemDetailComponent>componentInstance.instance);

			if(this.representationId){
				comp.representationId = this.representationId;
			}

			if(this.shoppingCartType){
				comp.shoppingCartType = this.shoppingCartType;
			}

			if(this.productSalesPlanningId){
				comp.productSalesPlanningId = this.productSalesPlanningId;
			}

    }
}
