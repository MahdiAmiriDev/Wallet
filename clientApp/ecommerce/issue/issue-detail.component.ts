import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Observable} from 'rxjs';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-issue-detail',
  templateUrl: './issue-detail.component.html'
})
export class IssueDetailComponent extends BaseEdit implements OnInit {
  dueDateDataValue = new FormControl();
  resolvedDateDataValue = new FormControl();
  @Input()
  orderItemId: number;
  @Input()
  issueType: number;
  @Input()
  resolutionType: number;
  @Input()
  priorityType: number;
  @Input()
  issueStateType: number;
  @Input()
  assignee: number;
  orderItemDataSource: any;
  issueTypes: any;
  resolutionTypes: any;
  priorityTypes: any;
  issueStateTypes: any;
  assigneeDataSource: any;
  accessLevels: any[] = [];
  issueOwner: number;
  nonRegisteredUser: string;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'issue';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.orderItemId) {
      this.entity.orderItemId = this.orderItemId;
    }
    if (this.issueType) {
      this.entity.issueType = this.issueType;
    }
    if (this.resolutionType) {
      this.entity.resolutionType = this.resolutionType;
    }
    if (this.priorityType) {
      this.entity.priorityType = this.priorityType;
    }
    if (this.issueStateType) {
      this.entity.issueStateType = this.issueStateType;
    }
    if (this.assignee) {
      this.entity.assignee = this.assignee;
    }
    if (this.issueOwner) {
      this.entity.issueOwner = this.issueOwner;
    }
    if (this.nonRegisteredUser) {
      this.entity.nonRegisteredUser = this.nonRegisteredUser;
    }


    this._genericDataService.getCommonBaseDataForSelect('IssueType').subscribe(res => this.issueTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('ResolutionType').subscribe(res => this.resolutionTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('PriorityType').subscribe(res => this.priorityTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('IssueStateType').subscribe(res => this.issueStateTypes = res);
    this.orderItemDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'orderItem');
    this.assigneeDataSource = this._genericDataService.createCustomDataSourceWithAction('id', 'user', 'getCrmUser');


  }

  // tslint:disable-next-line:typedef
  doBeforeSubmit(e): Observable<boolean> {
    // tslint:disable-next-line:triple-equals
    if (this.entity.id != undefined && this.issueOwner) {
      this.entity.issueOwner = this.issueOwner;
    }
    return super.doBeforeSubmit(e);
  }
}
