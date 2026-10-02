		import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		import {AdditionalUserPropCompanyGroupDetailComponent} from './additional-user-prop-company-group-detail.component';



		@Component({
		  selector: 'app-additional-user-prop-company-group-list',
		  templateUrl: './additional-user-prop-company-group-list.component.html'
		})
		export class AdditionalUserPropCompanyGroupListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'additionalUserPropCompanyGroup';
		  detailComponent =AdditionalUserPropCompanyGroupDetailComponent;
      showAddButton = true;
      allowEditInForm = false;
      allowDeleteInForm = false;
			_additionalUserPropId:number;
			@Input()
			set additionalUserPropId(val){
				if(val!==this._additionalUserPropId){
					this._additionalUserPropId=val;
				}
			}
			get additionalUserPropId(): number {
					return this._additionalUserPropId;
				}

			_companyGroupId:number;
			@Input()
			set companyGroupId(val){
				if(val!==this._companyGroupId){
					this._companyGroupId=val;
				}
			}
			get companyGroupId(): number {
					return this._companyGroupId;
				}

		  private baseListUrl = 'additionalUserPropCompanyGroup/GetByCustomFilterForList?';

		 constructor() {
		 super();
		 }
		 ngOnInit(): void {
		 		this.bindColumns();
		 		if(this.bindCustomDataSources()){
		 			this.loadAfterSetFilter=true;
		 		}
		 }
	private bindColumns(): void{
	this.columns.push(
				{
					caption: 'کاربر',
					dataField: 'additionalUserPropTitle'
				},

				{
					caption: 'گروه مرکز',
					dataField: 'companyGroupTitle'
				},
			 );
	}
	bindCustomDataSources(): boolean {
		let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {

			if(this.additionalUserPropId){
				customListUrl += `additionalUserPropId=${this.additionalUserPropId}&`;
			}

			if(this.companyGroupId){
				customListUrl += `companyGroupId=${this.companyGroupId}&`;
			}
		}

			this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();

			return true;
    }
    afterInitialDetailComponent(componentInstance: any): void  {
        const comp =(<AdditionalUserPropCompanyGroupDetailComponent>componentInstance.instance);

			if(this.additionalUserPropId){
				comp.additionalUserPropId = this.additionalUserPropId;
			}

			if(this.companyGroupId){
				comp.companyGroupId = this.companyGroupId;
			}

    }
}

