import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-merchant-info-detail-tab-based',
  templateUrl: './merchant-info-detail-tab-based.component.html'
})
export class MerchantInfoDetailTabBasedComponent extends Editable implements OnInit {
  state: number;
  city: number;

  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'merchantInfo';
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
      {id: 1, title: 'اطلاعات پذیرنده', template: 'merchantInfo'},
      {id: 2, title: 'ترمینال', template: 'terminal', disabled: !this.allowEditDesc},
    ];

  }


}


