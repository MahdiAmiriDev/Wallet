import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-recipient-detail',
  templateUrl: './recipient-detail.component.html'
})
export class RecipientDetailComponent extends BaseEdit implements OnInit {
  @Input()
  membershipType: number;
  addressDataSource: any;
  membershipTypes: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'recipient';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.membershipType) {
      this.entity.membershipType = this.membershipType;
    }

    this.addressDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'address');
    this._genericDataService.getCommonBaseDataForSelect('MembershipType').subscribe(res => this.membershipTypes = res);
  }
}
