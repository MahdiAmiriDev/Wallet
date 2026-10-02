		import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';


		import {TaxCategorySaleConfigurationDetailComponent} from './tax-category-sale-configuration-detail.component';



		@Component({
		  selector: 'app-tax-category-sale-configuration-list',
		  templateUrl: './tax-category-sale-configuration-list.component.html'
		})
		export class TaxCategorySaleConfigurationListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'taxCategorySaleConfiguration';
		  detailComponent =TaxCategorySaleConfigurationDetailComponent;

			_parentId:number;
			@Input()
			set parentId(val){
				if(val!==this._parentId){
					this._parentId=val;
				}
			}
			get parentId(): number {
					return this._parentId;
				}

			_taxCategoryId:number;
			@Input()
			set taxCategoryId(val){
				if(val!==this._taxCategoryId){
					this._taxCategoryId=val;
				}
			}
			get taxCategoryId(): number {
					return this._taxCategoryId;
				}

			_saleConfigurationId:number;
			@Input()
			set saleConfigurationId(val){
				if(val!==this._saleConfigurationId){
					this._saleConfigurationId=val;
				}
			}
			get saleConfigurationId(): number {
					return this._saleConfigurationId;
				}

			_factorReportType:number;
			@Input()
			set factorReportType(val){
				if(val!==this._factorReportType){
					this._factorReportType=val;
				}
			}
			get factorReportType(): number {
					return this._factorReportType;
				}

		  private baseListUrl = 'taxCategorySaleConfiguration/GetByCustomFilterForList?';

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
			// Iterate all columns

				{
					caption: 'دسته بندی مالیاتی',
					dataField: 'taxCategoryTitle'
				},

				{
					caption: 'تنظیم فروش',
					dataField: 'saleConfigurationTitle'
				},

				{
					caption: 'نوع گزارش فاکتور : با جزییات  - بدون جزییات',
					dataField: 'factorReportTypeTitle'
				},

			 );
	}
	bindCustomDataSources(): boolean {
		let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {

			if(this.parentId){
				customListUrl += `parentId=${this.parentId}&`;
			}

			if(this.taxCategoryId){
				customListUrl += `taxCategoryId=${this.taxCategoryId}&`;
			}

			if(this.saleConfigurationId){
				customListUrl += `saleConfigurationId=${this.saleConfigurationId}&`;
			}

			if(this.factorReportType){
				customListUrl += `factorReportType=${this.factorReportType}&`;
			}


		}

		let res=false;
		if(this.parentId || this.taxCategoryId || this.saleConfigurationId || this.factorReportType){


			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();

			res=true;
		}
			return res;

    }
    afterInitialDetailComponent(componentInstance: any): void  {
        const comp =(<TaxCategorySaleConfigurationDetailComponent>componentInstance.instance);

			if(this.parentId){
				comp.parentId = this.parentId;
			}

			if(this.taxCategoryId){
				comp.taxCategoryId = this.taxCategoryId;
			}

			if(this.saleConfigurationId){
				comp.saleConfigurationId = this.saleConfigurationId;
			}

			if(this.factorReportType){
				comp.factorReportType = this.factorReportType;
			}

    }
}

