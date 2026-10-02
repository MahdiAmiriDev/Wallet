import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-factor-detail-tab-based',
  templateUrl: './factor-detail-tab-based.component.html'
})
export class FactorDetailTabBasedComponent extends Editable implements OnInit {
  taxCategoryId: number;
  companyId: number;
  representationId: number;
  factorReviewStatusType: number;

  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'factor';
  }

  ngOnInit(): void {
    if (this.selectedEntityId && this.selectedEntityId !== 0) {
      this.allowEditDesc = true;
    } else {
      this.allowEditDesc = false;
    }
    this.bindTabs();
  }

  rowInserted(insertedRowId: any): void {
    this.selectedEntityId = insertedRowId;
    this.bindTabs();
  }

  bindTabs(): void {

    if (this.tabs.length == 0) {
      this.tabs = [
        {id: 1, title: 'فاکتور', template: 'factor'}
      ];
    }
    if (this.selectedEntityId && this.selectedEntityId !== 0) {
      this.tabs.push({id: 3, title: 'آیتم های فاکتور', template: 'factorItems', disabled: !this.allowEditDesc});
      this.tabs.push({id: 4, title: 'فایل فاکتور', template: 'factorDocs', disabled: !this.allowEditDesc});
      this.tabs.push({id: 5, title: 'دلایل عدم تاید', template: 'factorVerifications', disabled: !this.allowEditDesc});
    }

  }


}


