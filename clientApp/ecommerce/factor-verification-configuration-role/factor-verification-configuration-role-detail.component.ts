import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
    selector: 'app-factor-verification-configuration-role-detail',
    templateUrl: './factor-verification-configuration-role-detail.component.html'
})
export class FactorVerificationConfigurationRoleDetailComponent extends BaseEdit implements OnInit {
    @Input()
    factorVerificationConfigurationId: number;
    @Input()
    roleId: number;
    factorVerificationConfigurationDataSource: any;
    roleDataSource: any;
    accessLevels: any[] = [];

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'factorVerificationConfigurationRole';
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.factorVerificationConfigurationId) {
            this.entity.factorVerificationConfigurationId = this.factorVerificationConfigurationId;
        }
        if (this.roleId) {
            this.entity.roleId = this.roleId;
        }

        this.factorVerificationConfigurationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'factorVerificationConfiguration');
      this.roleDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'role');
    }
}
