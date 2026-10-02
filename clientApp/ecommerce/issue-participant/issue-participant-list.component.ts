import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {IssueParticipantDetailComponent} from './issue-participant-detail.component';


@Component({
  selector: 'app-issue-participant-list',
  templateUrl: './issue-participant-list.component.html'
})
export class IssueParticipantListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'issueParticipant';
  detailComponent = IssueParticipantDetailComponent;
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

  _userId: number;
  @Input()
  set userId(val) {
    if (val !== this._userId) {
      this._userId = val;
    }
  }

  get userId(): number {
    return this._userId;
  }

  private baseListUrl = 'issueParticipant/GetByCustomFilterForList?';

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
    });
    this.columns.push({
      caption: 'مرجع رسیدگی',
      dataField: 'userTitle'
    });
  }

  public bindCustomDataSources() {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.issueId) {
        customListUrl += `issueId=${this.issueId}&`;
      }
      if (this.userId) {
        customListUrl += `userId=${this.userId}&`;
      }
    }
    let res = false;
    if (this.issueId || this.userId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any) {
    const comp = (<IssueParticipantDetailComponent>componentInstance.instance);

    if (this.issueId) {
      comp.issueId = this.issueId;
    }

    if (this.userId) {
      comp.userId = this.userId;
    }

  }
}
