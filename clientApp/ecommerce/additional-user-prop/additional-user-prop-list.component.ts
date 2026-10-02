					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		import {AdditionalUserPropDetailComponent} from './additional-user-prop-detail.component';



		@Component({
		  selector: 'app-additional-user-prop-list',
		  templateUrl: './additional-user-prop-list.component.html'
		})
		export class AdditionalUserPropListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'additionalUserProp';
		  detailComponent =AdditionalUserPropDetailComponent;
		 			_userId:number;
			@Input()
			set userId(val){
				if(val!==this._userId){
					this._userId=val;
				}
			}
			get userId(): number {
					return this._userId;
				}
						_companyId:number;
			@Input()
			set companyId(val){
				if(val!==this._companyId){
					this._companyId=val;
				}
			}
			get companyId(): number {
					return this._companyId;
				}
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
					  private baseListUrl = 'additionalUserProp/GetByCustomFilterForList?';
		  constructor() {
			super();
							this.columns.push({
					caption: 'کاربر',
					dataField: 'userTitle'
					});
							this.columns.push({
					caption: 'مرکز',
					dataField: 'companyTitle'
					});
							this.columns.push({
					caption: 'نمایندگی',
					dataField: 'representationTitle'
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
							if(this.userId){
					customListUrl += `userId=${this.userId}&`;
				}
							if(this.companyId){
					customListUrl += `companyId=${this.companyId}&`;
				}
							if(this.representationId){
					customListUrl += `representationId=${this.representationId}&`;
				}
					}
		let res=false;
		if(this.userId || this.companyId || this.representationId){
			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
			res=true;
		}
		return res;
    }
    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<AdditionalUserPropDetailComponent>componentInstance.instance);

			if(this.userId){
				comp.userId = this.userId;
			}

			if(this.companyId){
				comp.companyId = this.companyId;
			}

			if(this.representationId){
				comp.representationId = this.representationId;
			}

    }
}
