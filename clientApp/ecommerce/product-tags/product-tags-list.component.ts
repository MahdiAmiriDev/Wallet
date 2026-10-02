		import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';


		import {ProductTagsDetailComponent} from './product-tags-detail.component';



		@Component({
		  selector: 'app-product-tags-list',
		  templateUrl: './product-tags-list.component.html'
		})
		export class ProductTagsListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'productTags';
		  detailComponent =ProductTagsDetailComponent;

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

			_productId:number;
			@Input()
			set productId(val){
				if(val!==this._productId){
					this._productId=val;
				}
			}
			get productId(): number {
					return this._productId;
				}

			_tagType:number;
			@Input()
			set tagType(val){
				if(val!==this._tagType){
					this._tagType=val;
				}
			}
			get tagType(): number {
					return this._tagType;
				}

		  private baseListUrl = 'productTags/GetByCustomFilterForList?';

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
					caption: 'عنوان',
					dataField: 'title'
				},

				{
					caption: 'عنوان محصول',
					dataField: 'productTitle'
				},

				{
					caption: 'نوع تگ',
					dataField: 'tagTypeTitle'
				},
			 );
	}
	bindCustomDataSources(): boolean {
		let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {

			if(this.parentId){
				customListUrl += `parentId=${this.parentId}&`;
			}

			if(this.productId){
				customListUrl += `productId=${this.productId}&`;
			}

			if(this.tagType){
				customListUrl += `tagType=${this.tagType}&`;
			}


		}

		let res=false;
		if(this.parentId || this.productId || this.tagType){


			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();

			res=true;
		}
			return res;

    }
    afterInitialDetailComponent(componentInstance: any): void  {
        const comp =(<ProductTagsDetailComponent>componentInstance.instance);

			if(this.parentId){
				comp.parentId = this.parentId;
			}

			if(this.productId){
				comp.productId = this.productId;
			}

			if(this.tagType){
				comp.tagType = this.tagType;
			}

    }
}

