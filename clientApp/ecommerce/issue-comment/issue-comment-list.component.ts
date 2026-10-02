					import {Selectable} from '../../../shared/base/classes/selectable';
		import {BaseList} from '../../../shared/base/classes/base-list';
		import {Component, Input, OnInit, ViewChild} from '@angular/core';
		import {IssueCommentDetailComponent} from './issue-comment-detail.component';



		@Component({
		  selector: 'app-issue-comment-list',
		  templateUrl: './issue-comment-list.component.html'
		})
		export class IssueCommentListComponent extends Selectable  implements OnInit {
		  @ViewChild('listForm', {static: true}) listForm: BaseList;
		  loadAfterSetFilter:boolean;
		  selectedItemId: number;
		  columns: any[] = [];
		  entityName = 'issueComment';
		  detailComponent =IssueCommentDetailComponent;
		 			_issueId:number;
			@Input()
			set issueId(val){
				if(val!==this._issueId){
					this._issueId=val;
				}
			}
			get issueId(): number {
					return this._issueId;
				}
					  private baseListUrl = 'issueComment/GetByCustomFilterForList?';
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
					caption: 'شناسه محصول',
					dataField: 'issueTitle'
					});
				}

	public bindCustomDataSources() {
				let customListUrl = `${this.baseListUrl}`;
		if (this.listForm) {
							if(this.issueId){
					customListUrl += `issueId=${this.issueId}&`;
				}
					}
				let res=false;
		if(this.issueId){

			this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
					res=true;
		}
			return res;

    }
    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<IssueCommentDetailComponent>componentInstance.instance);

			if(this.issueId){
				comp.issueId = this.issueId;
			}

    }
}
