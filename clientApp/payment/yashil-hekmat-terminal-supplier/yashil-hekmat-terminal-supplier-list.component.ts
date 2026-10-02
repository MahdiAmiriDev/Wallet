		import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		
		 
		import {YashilHekmatTerminalSupplierDetailComponent} from './yashil-hekmat-terminal-supplier-detail.component';
			
		

		@Component({
		  selector: 'app-yashil-hekmat-terminal-supplier-list',
		  templateUrl: './yashil-hekmat-terminal-supplier-list.component.html'
		})
		export class YashilHekmatTerminalSupplierListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'yashilHekmatTerminalSupplier';
		  detailComponent =YashilHekmatTerminalSupplierDetailComponent;
		 
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
		 
		  private baseListUrl = 'yashilHekmatTerminalSupplier/GetByCustomFilterForList?';
		 
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
					caption: 'ParentId',
					dataField: 'parentTitle'
				},
			
				
					
				{ 
					caption: 'ترمینال',
					dataField: 'yashilHekmatTerminalId'
				},
			
				{ 
					caption: 'کاربر',
					dataField: 'userTitle'
				},
			
				{ 
					caption: 'تامین کننده محصولات ترمینال',
					dataField: 'supplierId'
				},
			
				{ 
					caption: 'تامین کننده محصولات ترمینال',
					dataField: 'supplierTitle'
				},
			
				{ 
					caption: 'CodePath',
					dataField: 'codePath'
				},
			
			 );
	}
	bindCustomDataSources(): boolean {
		let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {
			
			if(this.parentId){
				customListUrl += `parentId=${this.parentId}&`;
			}
			
			if(this.userId){
				customListUrl += `userId=${this.userId}&`;
			}
			
		 
		}
	 	
		let res=false;
		if(this.parentId || this.userId){
		

			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
		
			res=true;
		}
			return res;
		
    }
    afterInitialDetailComponent(componentInstance: any): void  {
        const comp =(<YashilHekmatTerminalSupplierDetailComponent>componentInstance.instance);
		
			if(this.parentId){
				comp.parentId = this.parentId;
			}
		
			if(this.userId){
				comp.userId = this.userId;
			}
			       
    }
}

 