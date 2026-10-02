					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		import {RepresentationPersonDetailComponent} from './representation-person-detail.component';



		@Component({
		  selector: 'app-representation-person-list',
		  templateUrl: './representation-person-list.component.html'
		})
		export class RepresentationPersonListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'representationPerson';
		  detailComponent =RepresentationPersonDetailComponent;
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
						_personId:number;
			@Input()
			set personId(val){
				if(val!==this._personId){
					this._personId=val;
				}
			}
			get personId(): number {
					return this._personId;
				}
						_postId:number;
			@Input()
			set postId(val){
				if(val!==this._postId){
					this._postId=val;
				}
			}
			get postId(): number {
					return this._postId;
				}
					  private baseListUrl = 'representationPerson/GetByCustomFilterForList?';
		  constructor() {
			super();
							this.columns.push({
					caption: 'نمایندگی',
					dataField: 'representationTitle'
					});
							this.columns.push({
					caption: 'شخص',
					dataField: 'personTitle'
					});
							this.columns.push({
					caption: 'سمت',
					dataField: 'postTitle'
					});
							this.columns.push({
					caption: 'تاریخ جذب',
					dataField: 'fromDate'
					});
							this.columns.push({
					caption: 'تاریخ رهایی',
					dataField: 'toDate'
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
							if(this.personId){
					customListUrl += `personId=${this.personId}&`;
				}
							if(this.postId){
					customListUrl += `postId=${this.postId}&`;
				}
					}
		let res=false;
		if(this.representationId || this.personId || this.postId){
			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
			res=true;
		}
		return res;
    }
    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<RepresentationPersonDetailComponent>componentInstance.instance);

			if(this.representationId){
				comp.representationId = this.representationId;
			}

			if(this.personId){
				comp.personId = this.personId;
			}

			if(this.postId){
				comp.postId = this.postId;
			}

    }
}
