import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {SubsidyGroupDetailComponent} from './subsidy-group-detail.component';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';


@Component({
    selector: 'app-subsidy-group-list',
    templateUrl: './subsidy-group-list.component.html'
})
export class SubsidyGroupListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'subsidyGroup';
    detailComponent = SubsidyGroupDetailComponent;
    customRowButtons: YashilCustomButtonColumn;
    subsidyGroupId: number;
    popUpTitle: string;
    showPopUp: boolean;

    constructor() {
        super();
        this.showSubsidy = this.showSubsidy.bind(this);
    }

    ngOnInit(): void {
        this.bindColumns();
        this.BindCustomRowButtons();
    }

    private bindColumns(): void {
        this.columns.push(
            {
                caption: 'عنوان',
                dataField: 'title'
            },
        );
    }

    BindCustomRowButtons() {
        this.customRowButtons = {
            width: '150px',
            customButtons: [
                {
                    hint: 'تنظیمات یارانه',
                    text: 'تنظیمات یارانه',
                    icon: 'bulletlist',
                    type: 'default',
                    stylingMode: 'contained',
                    onClick: this.showSubsidy,
                }]
        };
    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as SubsidyGroupDetailComponent);

    }

    showSubsidy(e): void {
        this.subsidyGroupId = e.row.data.id;
        this.popUpTitle = 'ویژگی های تنظیم فروش';
        this.showPopUp = true;
    }
}
