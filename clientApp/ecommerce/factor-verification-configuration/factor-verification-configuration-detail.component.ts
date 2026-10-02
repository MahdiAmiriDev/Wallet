import {Component, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-factor-verification-configuration-detail',
  templateUrl: './factor-verification-configuration-detail.component.html'
})
export class FactorVerificationConfigurationDetailComponent extends BaseEdit implements OnInit {
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'factorVerificationConfiguration';
  }

  ngOnInit(): void {
    super.ngOnInit();
  }
}
