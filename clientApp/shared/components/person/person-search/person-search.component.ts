import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {BaseList} from '../../../../shared/base/classes/base-list';
import {YashilCustomButtonColumn} from '../../../../shared/base/classes/yashil-cutom-button-column';
import {YashilComponent} from '../../../../core/baseClasses/yashilComponent';
import {GenericDataService} from '../../../base/services/generic-data.service';


@Component({
    selector: 'app-person-search',
    templateUrl: './person-search.component.html'
})
export class PersonSearchComponent extends YashilComponent implements OnInit {
    customButtonType = 'default';
    customButtonText = 'انتخاب شخص';
    customButtonIcon = 'checklist';
    customButtonStyle = 'float: right ;height: 34px;';
    showPopUp: boolean = false;
    entity: any = {};
    @Input()
    showCustomButton = true;

    @Output()
    selectedPerson: EventEmitter<any> = new EventEmitter<any>();


    @ViewChild('listForm', {static: false}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    columns: any[] = [];
    customRowButtons: YashilCustomButtonColumn;
    showSelectedPersonInfo: boolean;
    _personEconomicProfileId: number;
    @Input()
    set personEconomicProfileId(value: number) {
        if (value !== this._personEconomicProfileId) {
            this._personEconomicProfileId = value;
            this.bindPersonData();
        }
    }

    get personEconomicProfileId(): number {
        return this._personEconomicProfileId;
    }

    constructor(private _genericDataService: GenericDataService) {
        super();
        this.bindPersonData = this.bindPersonData.bind(this);
    }

    ngOnInit(): void {

    }

    /*
    در صورتی شناسه شخص مقدار داشته باشد اظلاعات مروبط به شخص را نمایش می دهیم
     */
    private bindPersonData() {
        this._genericDataService.getEntitiesWithAction('PersonEconomicProfile', 'GetById?personEconomicProfileId=' + this._personEconomicProfileId, null)
            .subscribe(res => {
                if (res) {
                    this.entity = res;
                    this.showSelectedPersonInfo = true;
                }
            });
    }

    closePopUpBtn() {
        this.showPopUp = false;
    }

    onCustomButtonClick(): void {
        this.showPopUp = true;
    }

    /*
    وقتی که کاربر شخصی را از لیست انتخاب می کند
     */
    onSelectedPerson(item: any) {
        this.showPopUp = false;
        if (item) {
            this.entity = item;
            this.showSelectedPersonInfo = true;
            this.selectedPerson.emit(item);
        }

    }
}

