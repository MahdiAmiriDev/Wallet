import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';

@Component({
  selector: 'app-vocher-type-detail',
  templateUrl: './vocher-type-detail.component.html'
})
export class VocherTypeDetailComponent extends BaseEdit implements OnInit {
  @Input()
  companyId: number;
  companyDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'vocherType';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.companyId) {
      this.entity.companyId = this.companyId;
    }

    this.companyDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'company');
    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
  }
}
