import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-issue-comment-detail',
  templateUrl: './issue-comment-detail.component.html'
})
export class IssueCommentDetailComponent extends BaseEdit implements OnInit {
  @Input()
  issueId: number;
  issueDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'issueComment';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.issueId) {
      this.entity.issueId = this.issueId;
    }

    this.issueDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'issue');
  }
}
