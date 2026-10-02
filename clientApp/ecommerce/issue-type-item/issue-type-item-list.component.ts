import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {IssueTypeItemDetailComponent} from './issue-type-item-detail.component';


@Component({
  selector: 'app-issue-type-item-list',
  templateUrl: './issue-type-item-list.component.html'
})
export class IssueTypeItemListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'issueTypeItem';
  detailComponent = IssueTypeItemDetailComponent;
  _issueId: number;
  @Input()
  set issueId(val) {
    if (val !== this._issueId) {
      this._issueId = val;
    }
  }

  get issueId(): number {
    return this._issueId;
  }

  _issueType: number;
  @Input()
  set issueType(val) {
    if (val !== this._issueType) {
      this._issueType = val;
    }
  }

  get issueType(): number {
    return this._issueType;
  }

  private baseListUrl = 'issueTypeItem/GetByCustomFilterForList?';

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns() {
    this.columns.push({
      caption: 'شناسه مسئله',
      dataField: 'issueTitle'
    },{
      caption: 'نوع مسئله(فرزند)',
      dataField: 'issueTypeTitle'
    });
  }

  public bindCustomDataSources() {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.issueId) {
        customListUrl += `issueId=${this.issueId}&`;
      }
      if (this.issueType) {
        customListUrl += `issueType=${this.issueType}&`;
      }
    }
    let res = false;
    if (this.issueId || this.issueType) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any) {
    const comp = (<IssueTypeItemDetailComponent>componentInstance.instance);

    if (this.issueId) {
      comp.issueId = this.issueId;
    }

    if (this.issueType) {
      comp.issueType = this.issueType;
    }

  }
}
