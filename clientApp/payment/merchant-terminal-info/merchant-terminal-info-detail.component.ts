import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
    selector: 'app-merchant-terminal-info-detail',
    templateUrl: './merchant-terminal-info-detail.component.html'
})
export class MerchantTerminalInfoDetailComponent extends BaseEdit implements OnInit {
    @Input()
    merchantId: number;
    accessLevels: any[] = [];
    merchantDataSource: any;

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'merchantTerminalInfo';
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.merchantId)
            this.entity.merchantId = this.merchantId;

        this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
        this.merchantDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'merchantInfo');
    }
}
