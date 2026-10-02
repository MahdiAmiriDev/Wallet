import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Observable} from 'rxjs';


@Component({
  selector: 'app-issue-log-work-detail',
  templateUrl: './issue-log-work-detail.component.html'
})
export class IssueLogWorkDetailComponent extends BaseEdit implements OnInit {

  selectedIssueTypes: any;
  issueTitle: string;
  @Input()
  fixHeight = false;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'issueLogWork';
    this.customizeIssueTitle = this.customizeIssueTitle.bind(this);
  }

  // tslint:disable-next-line:variable-name
  private _customIssueTitle: string;

  get customIssueTitle(): string {
    return this._customIssueTitle;
  }

  @Input()
  set customIssueTitle(value: string) {
    this._customIssueTitle = value;
  }

  // tslint:disable-next-line:variable-name
  private _issueId: number;

  get issueId(): number {
    return this._issueId;
  }

  @Input()
  set issueId(value: number) {
    this._issueId = value;
  }

  // tslint:disable-next-line:variable-name
  private _issueType: number;

  get issueType(): number {
    return this._issueType;
  }

  @Input()
  set issueType(value: number) {
    this._issueType = value;
  }

  // tslint:disable-next-line:variable-name
  private _paymentId: number;

  get paymentId(): number {
    return this._paymentId;
  }

  @Input()
  set paymentId(value: number) {
    this._paymentId = value;
  }

  // tslint:disable-next-line:variable-name
  private _orderId: number;

  get orderId(): number {
    return this._orderId;
  }

  @Input()
  set orderId(value: number) {
    this._orderId = value;
  }

  // tslint:disable-next-line:variable-name
  private _orderItemId: number;

  get orderItemId(): number {
    return this._orderItemId;
  }

  @Input()
  set orderItemId(value: number) {
    this._orderItemId = value;
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.issueId) {
      this.entity.issueId = this.issueId;
    }
    if (!this.issueType) {
      this._genericDataService.getCommonBaseDataForSelect('issueType')
        .subscribe(res => this.selectedIssueTypes = res);
    }
  }

  doBeforeSubmit(e): Observable<boolean> {
    if (!this.entity.issueId && this.issueId) {
      this.entity.issueId = this.issueId;
    }
    if (this.orderId) {
      this.entity.orderId = this.orderId;
    }
    if (this.issueTitle) {
      this.entity.customIssueTitle = this.issueTitle;
    } else if (this.customIssueTitle) {
      this.entity.customIssueTitle = this.customIssueTitle;
    }
    if (this.orderItemId) {
      this.entity.orderItemId = this.orderItemId;
    }
    if (this.paymentId) {
      this.entity.paymentId = this.paymentId;
    }
    return super.doBeforeSubmit(e);
  }

  customizeIssueTitle(item: any): void {
    if (item && item.selectedItem.title) {
      this.issueTitle = ` ثبت ${item.selectedItem.title} `;
      if (this.customIssueTitle) {
        this.issueTitle = this.customIssueTitle + ' ' + this.issueTitle;
      }
    }
  }
}
