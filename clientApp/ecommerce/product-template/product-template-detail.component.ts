import {Component, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-product-template-detail',
  templateUrl: './product-template-detail.component.html'
})
export class ProductTemplateDetailComponent extends BaseEdit implements OnInit {
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'productTemplate';
  }

  ngOnInit(): void {
    super.ngOnInit();

    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
  }
}
