import {Component, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-tax-category-detail',
  templateUrl: './tax-category-detail.component.html'
})
export class TaxCategoryDetailComponent extends BaseEdit implements OnInit {
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'taxCategory';
  }

  ngOnInit(): void {
    super.ngOnInit();
  }
}
