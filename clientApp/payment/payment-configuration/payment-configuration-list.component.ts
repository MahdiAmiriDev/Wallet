					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, OnInit, ViewChild} from '@angular/core';
		import {PaymentConfigurationDetailComponent} from './payment-configuration-detail.component';



		@Component({
		  selector: 'app-payment-configuration-list',
		  templateUrl: './payment-configuration-list.component.html'
		})
		export class PaymentConfigurationListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'paymentConfiguration';
		  detailComponent =PaymentConfigurationDetailComponent;
		 		  private baseListUrl = 'paymentConfiguration/GetByCustomFilterForList?';
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
        const comp = (<PaymentConfigurationDetailComponent>componentInstance.instance);

    }
}
