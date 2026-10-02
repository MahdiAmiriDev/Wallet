					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, OnInit, ViewChild} from '@angular/core';
		import {TaxRateDetailComponent} from './tax-rate-detail.component';



		@Component({
		  selector: 'app-tax-rate-list',
		  templateUrl: './tax-rate-list.component.html'
		})
		export class TaxRateListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'taxRate';
		  detailComponent =TaxRateDetailComponent;
		 		  private baseListUrl = 'taxRate/GetByCustomFilterForList?';
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
				}

	public bindCustomDataSources() {
				let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {
					}

			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();

		return false;

    }
    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<TaxRateDetailComponent>componentInstance.instance);

    }
}
