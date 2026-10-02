					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		import {RecipientDetailComponent} from './recipient-detail.component';



		@Component({
		  selector: 'app-recipient-list',
		  templateUrl: './recipient-list.component.html'
		})
		export class RecipientListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'recipient';
		  detailComponent =RecipientDetailComponent;
		 			_membershipType:number;
			@Input()
			set membershipType(val){
				if(val!==this._membershipType){
					this._membershipType=val;
				}
			}
			get membershipType(): number {
					return this._membershipType;
				}
					  private baseListUrl = 'recipient/GetByCustomFilterForList?';
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
					caption: 'عنوان',
					dataField: 'title'
					});
							this.columns.push({
					caption: 'کد ملی',
					dataField: 'nationalCode'
					});
							this.columns.push({
					caption: 'شماره کارت',
					dataField: 'cardNumber'
					});
							this.columns.push({
					caption: 'نوع عضویت',
					dataField: 'membershipTypeTitle'
					});
				}

	public bindCustomDataSources() {
				let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {
							if(this.membershipType){
					customListUrl += `membershipType=${this.membershipType}&`;
				}
					}
				let res=false;
		if(this.membershipType){

			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
					res=true;
		}
			return res;

    }
    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<RecipientDetailComponent>componentInstance.instance);

			if(this.membershipType){
				comp.membershipType = this.membershipType;
			}

    }
}
