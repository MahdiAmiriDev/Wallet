import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-application-customer-club-detail',
  templateUrl: './application-customer-club-detail.component.html'
})
export class ApplicationCustomerClubDetailComponent extends BaseEdit implements OnInit {
  @Input()
  customerClubId: number;
  customerClubDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'applicationCustomerClub';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.customerClubId) {
      this.entity.customerClubId = this.customerClubId;
    }

    this.customerClubDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'customerClub');
  }
}
