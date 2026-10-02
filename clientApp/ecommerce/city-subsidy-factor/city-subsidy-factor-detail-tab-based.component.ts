import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-city-subsidy-factor-detail-tab-based',
  templateUrl: './city-subsidy-factor-detail-tab-based.component.html'
})
export class CitySubsidyFactorDetailTabBasedComponent extends Editable implements OnInit {
  cityId: number;

  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'citySubsidyFactor';
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
      {id: 1, title: 'ضریب یارانه شهر ', template: 'citySubsidyFactor'},
      {id: 2, title: 'توضیحات', template: 'description', disabled: !this.allowEditDesc},
    ];

  }


}


