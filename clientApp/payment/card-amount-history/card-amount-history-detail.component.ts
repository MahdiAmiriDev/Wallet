import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
    selector: 'app-card-amount-history-detail',
    templateUrl: './card-amount-history-detail.component.html'
})
export class CardAmountHistoryDetailComponent extends BaseEdit implements OnInit {
    @Input()
    cardInfoId: number;
    accessLevels: any[] = [];
    cardInfoDataSource: any;

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'cardAmountHistory';
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.cardInfoId)
            this.entity.cardInfoId = this.cardInfoId;

        this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
        this.cardInfoDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'cardInfo');
    }
}
