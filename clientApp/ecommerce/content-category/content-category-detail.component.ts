import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-content-category-detail',
  templateUrl: './content-category-detail.component.html'
})
export class ContentCategoryDetailComponent extends BaseEdit implements OnInit {
  _parentId: number;
  parentDataSource: any;
  accessLevels: any[] = [];

  @Input()
  set parentId(val) {
    if (val !== this._parentId) {
      this._parentId = val;
    }
  }

  get parentId(): number {
    return this._parentId;
  }

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'contentCategory';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.parentId) {
      this.entity.parentId = this.parentId;
    }

    this.parentDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'contentCategory');
  }
}
