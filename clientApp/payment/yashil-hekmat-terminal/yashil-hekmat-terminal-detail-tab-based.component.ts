import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-yashil-hekmat-terminal-detail-tab-based',
  templateUrl: './yashil-hekmat-terminal-detail-tab-based.component.html'
})
export class YashilHekmatTerminalDetailTabBasedComponent extends Editable implements OnInit {
  hekmatTerminalId: number;
  merchantId: number;
  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'yashilHekmatTerminal';
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
    if (this.tabs.length == 0) {
      this.tabs = [
        {id: 0, title: 'ترمینال کاربر', template: 'yashilHekmatTerminal'},
        {id: 1, title: 'آیکون', template: 'icon', disabled: !this.allowEditDesc},
      ];
    } else {
      this.tabs[1].disabled = !this.allowEditDesc;
    }

  }
}


