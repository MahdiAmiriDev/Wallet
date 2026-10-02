import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-app-entity-customer-club-detail-tab-based',
  templateUrl: './app-entity-customer-club-detail-tab-based.component.html'
})
export class AppEntityCustomerClubDetailTabBasedComponent extends Editable implements OnInit {
  appEntityId: number;
  customerclubId: number;

  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'appEntityCustomerClub';
  }

  ngOnInit() {
    if (this.selectedEntityId && this.selectedEntityId !== 0) {
      this.allowEditDesc = true;
    } else {
      this.allowEditDesc = false;
    }
    this.bindTabs();
  }

  rowInserted(insertedRowId: any) {
    this.selectedEntityId = insertedRowId;
    this.allowEditDesc = true;
    this.bindTabs();
  }

  bindTabs() {

    this.tabs = [
      {id: 1, title: 'باشگاه مشتریان موجودیت', template: 'appEntityCustomerClub'},
      {id: 2, title: 'توضیحات', template: 'description', disabled: !this.allowEditDesc},
    ];

  }


}


