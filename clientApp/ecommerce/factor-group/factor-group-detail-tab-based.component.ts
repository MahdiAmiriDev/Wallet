import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';
import {AssignableListComponent} from '../../../shared/base/core/components/assignable-list/assignable-list.component';


@Component({
  selector: 'app-factor-group-detail-tab-based',
  templateUrl: './factor-group-detail-tab-based.component.html'
})
export class FactorGroupDetailTabBasedComponent extends Editable implements OnInit {
  parentId: number;
  factorGroupStatus: number;
  factorGroupItem = AssignableListComponent;
  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'factorGroup';
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
    this.tabs = [
      {id: 1, title: 'گروه بندی فاکتور', template: 'factorGroup'},
      // {id: 2, title: 'آیتم های گروه', template: 'factorGroupItems', disabled: !this.allowEditDesc},
    ];
  }




}


