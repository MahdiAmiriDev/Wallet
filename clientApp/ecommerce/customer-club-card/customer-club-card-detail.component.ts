import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-customer-club-card-detail',
  templateUrl: './customer-club-card-detail.component.html'
})
export class CustomerClubCardDetailComponent extends BaseEdit implements OnInit {
  @Input()
  personId: number;
  @Input()
  customerClubId: number;
  toDateDataValue = new FormControl();
  fromDataValue = new FormControl();
  personDataSource: any;
  customerClubDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'customerClubCard';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.personId) {
      this.entity.personId = this.personId;
    }
    if (this.customerClubId) {
      this.entity.customerClubId = this.customerClubId;
    }

    this.personDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'person');
    this.customerClubDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'customerClub');
  }
}
