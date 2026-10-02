import {Component, OnInit, ViewChild} from '@angular/core';
import {YashilComponent} from '../../../core/baseClasses/yashilComponent';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {IssueListComponent} from './issue-list.component';
import {IssueLogWorkListComponent} from '../issue-log-work/issue-log-work-list.component';

@Component({
  selector: 'app-issue-search',
  templateUrl: './issue-search.component.html'
})
export class IssueSearchComponent extends YashilComponent implements OnInit {
  @ViewChild('issueList', {static: true}) issueList: IssueListComponent;
  @ViewChild('issueLogWorkList', {static: true}) issueLogWorkList: IssueLogWorkListComponent;
  contentHeight: number;
  mobileNumber: string;
  cardNumber: string;
  searchedMobileNumber: string;
  searchedCardNumber: string;
  issueId: number;
  orderItemId: number;
  issueOwner: number;
  nonRegisteredUser: string;
  userDataSource: any;
  assignee: 0;


  constructor(private genericDataService: GenericDataService) {
    super();

  }

  ngOnInit(): void {
    this.contentHeight = window.innerHeight - 110;
  }

  handleKeyUp(event): void {
    // when pressing Enter key
    if (event.keyCode === 13) {
      this.search();
      // rest of your code
    }
  }

  search(): void {
    this.orderItemId = 0;
    this.issueOwner = 0;
    this.nonRegisteredUser = null;
    this.mobileNumber = this.searchedMobileNumber;
    this.cardNumber = this.searchedCardNumber;
    if (this.searchedMobileNumber && this.searchedMobileNumber.length > 0) {
      this.getUserByUserName();
    }
    this.bindChildComponentDataSource();
  }

  bindChildComponentDataSource(): void {
    this.issueList.issueOwner = this.issueOwner;
    this.issueList.nonRegisteredUser = this.nonRegisteredUser;
    this.issueList.orderItemId = this.orderItemId;
    this.issueList.bindCustomDataSources();
  }

  getUserByUserName(): void {
    this.genericDataService.getEntitiesWithAction('user', 'getByUserName?userName=' + this.searchedMobileNumber, null)
      .subscribe((data: any) => {
          if (data) {
            this.issueOwner = data.id;
          } else {
            this.nonRegisteredUser = this.searchedMobileNumber;
          }
        }, () => {

        },
        () => {
        }
      );
  }

  selectedOrderItemChanged(orderItem: any): void {
    this.orderItemId = orderItem.id;
    this.issueOwner = orderItem.createBy;
    this.bindChildComponentDataSource();
    this.issueId = -1;
    this.issueLogWorkList.bindCustomDataSources();

  }

  selectedIssueChanged(issue: any): void {
    this.issueId = issue.id;
    this.issueLogWorkList.bindCustomDataSources();
  }

  selectIssueAssigneeChange(user: any): void {
    this.assignee = user.id;
    if (this.assignee) {
      this.issueList.assignee = this.assignee;
      this.issueList.bindCustomDataSources();
    }
  }
}
