import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
  selector: 'app-product-tags-detail-tab-based',
  templateUrl: './product-tags-detail-tab-based.component.html'
})
export class ProductTagsDetailTabBasedComponent extends Editable implements OnInit {

  parentId: number;

  productId: number;

  tagType: number;


  allowEditDesc: boolean;
  tabs: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'productTags';
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
      {id: 1, title: 'برچسب های محصول', template: 'productTags'},
      {id: 2, title: 'توضیحات', template: 'description', disabled: !this.allowEditDesc},];

  }


}


