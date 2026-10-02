import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {GiftCardTypeDetailComponent} from './gift-card-type-detail.component';


@Component({
    selector: 'app-gift-card-type-list',
    templateUrl: './gift-card-type-list.component.html'
})
export class GiftCardTypeListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'giftCardType';
    detailComponent = GiftCardTypeDetailComponent;
    private baseListUrl = 'giftCardType/GetByCustomFilterForList?';

    constructor() {
        super();
        this.columns.push({
            caption: 'عنوان',
            dataField: 'title'
        });

    }

    ngOnInit(): void {
    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<GiftCardTypeDetailComponent>componentInstance.instance);

    }

}
