					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		import {ApplicationCustomerClubDetailComponent} from './application-customer-club-detail.component';



		@Component({
		  selector: 'app-application-customer-club-list',
		  templateUrl: './application-customer-club-list.component.html'
		})
		export class ApplicationCustomerClubListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'applicationCustomerClub';
		  detailComponent =ApplicationCustomerClubDetailComponent;
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
					  private baseListUrl = 'applicationCustomerClub/GetByCustomFilterForList?';
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
					caption: 'باشگاه مشتریان',
					dataField: 'customerClubTitle'
					});
				}

	public bindCustomDataSources() {
				let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {
							if(this.customerClubId){
					customListUrl += `customerClubId=${this.customerClubId}&`;
				}
					}
				let res=false;
		if(this.customerClubId){

			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
					res=true;
		}
			return res;

    }
    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<ApplicationCustomerClubDetailComponent>componentInstance.instance);

			if(this.customerClubId){
				comp.customerClubId = this.customerClubId;
			}

    }
}
