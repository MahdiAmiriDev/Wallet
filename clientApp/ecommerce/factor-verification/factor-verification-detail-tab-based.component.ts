import {Component, OnInit} from '@angular/core';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-factor-verification-detail-tab-based',
  templateUrl: './factor-verification-detail-tab-based.component.html'
})
export class FactorVerificationDetailTabBasedComponent extends Editable implements OnInit {
  factorId: number;
  factorConfirmationType: number;
  factorVerificationConfigurationRoleId: number;
  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor() {
    super();
    this.entityName = 'factorVerification';
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
    this.allowEditDesc = true;
    this.bindTabs();
  }

  bindTabs(): void {

    if (this.tabs.length == 0) {
      this.tabs = [
        {id: 0, title: 'بررسی فاکتور', template: 'factorVerification'},
        {id: 1, title: 'وضعیت فاکتور', template: 'factorVerifications', disabled: !this.allowEditDesc},
        {id: 2, title: 'آیتم های فاکتور', template: 'factorItems', disabled: !this.allowEditDesc},
      ];
    } else {
      this.tabs[1].disabled=!this.allowEditDesc;
      this.tabs[2].disabled=!this.allowEditDesc;
    }

  }

  getFactorId(data: any): void {
    this.factorId = data.factorId;
  }
}


