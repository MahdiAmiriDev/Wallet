import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-issue-type-item-detail',
  templateUrl: './issue-type-item-detail.component.html'
})
export class IssueTypeItemDetailComponent extends BaseEdit implements OnInit {
  @Input()
  issueId: number;
  @Input()
  issueType: number;
  issueDataSource: any;
  issueTypes: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'issueTypeItem';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.issueId) {
      this.entity.issueId = this.issueId;
    }
    if (this.issueType) {
      this.entity.issueType = this.issueType;
    }

    this.issueDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'issue');
    this._genericDataService.getCommonBaseDataForSelect('IssueType').subscribe(res => this.issueTypes = res);
  }
}
