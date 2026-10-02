import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-app-entity-customer-club-detail',
  templateUrl: './app-entity-customer-club-detail.component.html'
})
export class AppEntityCustomerClubDetailComponent extends BaseEdit implements OnInit {
  @Input()
  appEntityId: number;
  @Input()
  customerclubId: number;
  appEntityDataSource: any;
  customerclubDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'appEntityCustomerClub';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.appEntityId)
      this.entity.appEntityId = this.appEntityId;
    if (this.customerclubId)
      this.entity.customerclubId = this.customerclubId;

    this.appEntityDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'appEntity');
    this.customerclubDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'customerClub');
    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
  }
}
