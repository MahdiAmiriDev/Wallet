import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-content-category-detail-tab-based',
  templateUrl: './content-category-detail-tab-based.component.html'
})
export class ContentCategoryDetailTabBasedComponent extends Editable implements OnInit {
  parentId: number;
  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'contentCategory';
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
        {id: 0, title: 'دسته بندی محصولات', template: 'contentCategory'},
        {id: 1, title: 'فایل', template: 'categoryFile', disabled: !this.allowEditDesc},
      ];
    } else {
      this.tabs[1].disabled = !this.allowEditDesc;
    }

  }


}


