					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		import {RepresentationEstablishedLicenseTypeDetailComponent} from './representation-established-license-type-detail.component';



		@Component({
		  selector: 'app-representation-established-license-type-list',
		  templateUrl: './representation-established-license-type-list.component.html'
		})
		export class RepresentationEstablishedLicenseTypeListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'representationEstablishedLicenseType';
		  detailComponent =RepresentationEstablishedLicenseTypeDetailComponent;
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
						_establishedLicenseType:number;
			@Input()
			set establishedLicenseType(val){
				if(val!==this._establishedLicenseType){
					this._establishedLicenseType=val;
				}
			}
			get establishedLicenseType(): number {
					return this._establishedLicenseType;
				}
					  private baseListUrl = 'representationEstablishedLicenseType/GetByCustomFilterForList?';
		  constructor() {
			super();
							this.columns.push({
					caption: 'نمایندگی',
					dataField: 'representationTitle'
					});
							this.columns.push({
					caption: 'مجوز تاسیس',
					dataField: 'establishedLicenseTypeTitle'
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
							if(this.representationId){
					customListUrl += `representationId=${this.representationId}&`;
				}
							if(this.establishedLicenseType){
					customListUrl += `establishedLicenseType=${this.establishedLicenseType}&`;
				}
					}
		let res=false;
		if(this.representationId || this.establishedLicenseType){
			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
			res=true;
		}
		return res;
    }
    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<RepresentationEstablishedLicenseTypeDetailComponent>componentInstance.instance);

			if(this.representationId){
				comp.representationId = this.representationId;
			}

			if(this.establishedLicenseType){
				comp.establishedLicenseType = this.establishedLicenseType;
			}

    }
}
