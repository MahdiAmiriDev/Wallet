import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../base/classes/base-edit';
import {GenericDataService} from '../../base/services/generic-data.service';
import {Entity} from '../../base/base-data/entity.enum';


@Component({
  selector: 'app-address-detail',
  templateUrl: './address-detail.component.html'
})
export class AddressDetailComponent extends BaseEdit implements OnInit {
  @Input()
  accessLevelId: number;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'address';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.accessLevelId) {
      this.entity.accessLevelId = this.accessLevelId;
    }

    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
  }
}
