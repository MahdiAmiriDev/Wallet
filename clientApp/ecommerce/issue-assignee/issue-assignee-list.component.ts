import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';


@Component({
  selector: 'app-issue-assignee-list',
  templateUrl: './issue-assignee-list.component.html'
})
export class IssueAssigneeListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'issue';

  // tslint:disable-next-line:variable-name
  _recipientNationalCode: string;
  @Input()
  set recipientNationalCode(val) {
    if (val !== this._recipientNationalCode) {
      this._recipientNationalCode = val;
    }
  }

  get recipientNationalCode(): string {
    return this._recipientNationalCode;
  }

  // tslint:disable-next-line:variable-name
  _mobileNumber: string;
  @Input()
  set mobileNumber(val) {
    if (val !== this._mobileNumber) {
      this._mobileNumber = val;
    }
  }

  get mobileNumber(): string {
    return this._mobileNumber;
  }

  // tslint:disable-next-line:variable-name
  _orderItemId: number;
  @Input()
  set orderItemId(val) {
    if (val !== this._orderItemId) {
      this._orderItemId = val;
    }
  }

  get orderItemId(): number {
    return this._orderItemId;
  }

  private baseListUrl = 'issue/GetAssigneeByFiler?';
  @Input()
  isForSelectReadOnly: false;

  constructor() {
    super();
    this.loadAfterSetFilter = true;
  }

  ngOnInit(): void {
    this.bindColumns();
    this.bindCustomDataSources();
    this.loadAfterSetFilter = true;
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'نام کارشناس',
      dataField: 'fullName'
    });
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.recipientNationalCode) {
        customListUrl += `recipientNationalCode=${this.recipientNationalCode}&`;
      }
      if (this.mobileNumber) {
        customListUrl += `mobileNumber=${this.mobileNumber}&`;
      }
      if (this.orderItemId) {
        customListUrl += `orderItemId=${this.orderItemId}&`;
      }
    }
    let res = false;
    if (this.recipientNationalCode || this.mobileNumber || this.orderItemId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }
}
