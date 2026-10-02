import {Component, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
    selector: 'app-customer-club-detail',
    templateUrl: './customer-club-detail.component.html'
})
export class CustomerClubDetailComponent extends BaseEdit implements OnInit {
    accessLevels: any[] = [];
    registrationModeTypes: any;

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'customerClub';
    }

    ngOnInit(): void {
        super.ngOnInit();
        this._genericDataService.getCommonBaseDataForSelect('RegistrationModeType').subscribe(res => this.registrationModeTypes = res);
    }
}
