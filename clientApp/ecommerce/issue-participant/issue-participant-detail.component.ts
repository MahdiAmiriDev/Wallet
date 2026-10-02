import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-issue-participant-detail',
  templateUrl: './issue-participant-detail.component.html'
})
export class IssueParticipantDetailComponent extends BaseEdit implements OnInit {
  @Input()
  issueId: number;
  @Input()
  userId: number;
  issueDataSource: any;
  userDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'issueParticipant';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.issueId) {
      this.entity.issueId = this.issueId;
    }
    if (this.userId) {
      this.entity.userId = this.userId;
    }

    this.issueDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'issue');
    this.userDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'user');
  }
}
