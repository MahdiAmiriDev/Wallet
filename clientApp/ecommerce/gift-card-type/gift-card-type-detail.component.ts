import {Component, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
    selector: 'app-gift-card-type-detail',
    templateUrl: './gift-card-type-detail.component.html'
})
export class GiftCardTypeDetailComponent extends BaseEdit implements OnInit {
    accessLevels: any[] = [];

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'giftCardType';
    }

    ngOnInit(): void {
        super.ngOnInit();

        this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
    }
}
