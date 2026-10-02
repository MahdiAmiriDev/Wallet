import {Component, OnInit} from '@angular/core';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-category-group-detail-tab-based',
  templateUrl: './category-group-detail-tab-base.html'
})
export class CategoryGroupDetailTabBased extends Editable implements OnInit {

  tabs: any[] = [];
  allowEditImages: boolean;

  constructor() {
    super();
    this.entityName = 'categoryGroup';
  }

  ngOnInit(): void {
    if (this.selectedEntityId && this.selectedEntityId !== 0) {
      this.allowEditImages = true;
    } else {
      this.allowEditImages = false;
    }
    this.bindTabs();

  }

  //بعد از این که یک categoryGroup را کاربر insert کرد
  //کاربر امکان امکان افزودن تصویر برای گروه دسته بندی را خواهد داشت
  rowInserted(insertedRowId: any): void {
    this.selectedEntityId = insertedRowId;
    this.allowEditImages = true;
    this.bindTabs();
  }

  bindTabs(): void {
    this.tabs = [
      {id: 0, title: 'گروه دسته بندی', template: 'categoryGroup'},
      {id: 0, title: 'تصویر گروه دسته بندی', template: 'images', disabled: !this.allowEditImages}
    ]
  }


}


