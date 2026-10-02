import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-gift-card-type-detail-tab-based',
  templateUrl: './gift-card-type-detail-tab-based.component.html'
})
export class GiftCardTypeDetailTabBasedComponent extends Editable implements OnInit {

  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'giftCardType';
  }

  ngOnInit(): void {
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
        {id: 0, title: 'نوع کارت هدیه', template: 'giftCardType'},
        {id: 1, title: 'توضیحات', template: 'description', disabled: !this.allowEditDesc},
      ];
    } else {
      this.tabs[1].disabled = !this.allowEditDesc;
    }

  }


}


