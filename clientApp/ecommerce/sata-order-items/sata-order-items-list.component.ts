import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {FormControl} from '@angular/forms';
import * as moment from 'jalali-moment';


@Component({
    selector: 'app-sata-order-items-list',
    templateUrl: './sata-order-items-list.component.html'
})
export class SataOrderItemsListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    orderCreationDateFilterFromDateDataValue = new FormControl();
    orderCreationDateFilterToDateDataValue = new FormControl();
    orderCreationDateFilterFromDate: number;
    orderCreationDateFilterToDate: number;
    datePickerConfig = {
        format: 'jYYYY/jMM/jDD',
        dir: 'rtl',
        locale: moment.locale('fa')
    };
    columns: any[] = [];


    constructor() {
        super();
    }

    ngOnInit(): void {
        this.orderCreationDateFilterFromDate = this.convertMomentDateToInt(moment().startOf('year'));
        this.orderCreationDateFilterToDate = this.convertMomentDateToInt(moment());
        this.bindColumns();
        this.bindCustomDataSources();
    }


    private bindColumns(): void {
        this.columns.push({
                caption: 'نام طرح',
                dataField: 'applicationTitle'
            },
            {
                caption: 'زیر مجموعه طرح باقرالعلوم(ع)',
                dataField: 'categoryGroupTitle',
                width: '70px',
            },
            {
                caption: 'استان',
                dataField: 'provinceTitle'
            },
            {
                caption: 'شهر',
                dataField: 'cityTitle'
            },
            {
                caption: 'محصول',
                dataField: 'productTitle'
            },
            {
                caption: 'تاریخ',
                dataField: 'creationDateInt',
                cellTemplate: 'intDate',
                width: '80px',
            },
            {
                caption: 'سهم خانوار',
                dataField: 'total',
                cellTemplate: 'number',
                width: '90px',
            },
            {
                caption: 'وضعیت ساتا',
                dataField: 'subsidyStatusTypeTitle',
                width: '70px',
            },
            {
                caption: 'یارانه ساتا',
                dataField: 'sataSubsidePrice',
                cellTemplate: 'number',
                width: '70px',
            },
            {
                caption: 'تخفیف',
                dataField: 'discountAmountExclTax',
                cellTemplate: 'number',
                width: '90px',
            },
            {
                caption: 'شماره کارت',
                dataField: 'cardNumber',
                width: '150px',
            },
            {
                caption: 'کد ملی',
                dataField: 'nationalCode',
                width: '100px',
            },
            {
                caption: 'تلفن داوطلب',
                dataField: 'userName',
                width: '100px',
            }
        );
    }

    public bindCustomDataSources(): boolean {
        let customListUrl = `orderItem/GetSataSubsidyBasedOrderItemFullInfoView?`;
        if (this.listForm) {

            if (this.orderCreationDateFilterFromDate) {
                customListUrl += `orderCreationDateFromDate=${this.orderCreationDateFilterFromDate}&`;
            }
            if (this.orderCreationDateFilterToDate) {
                customListUrl += `orderCreationDateToDate=${this.orderCreationDateFilterToDate}&`;
            }
        }
        this.listForm.customListUrl = customListUrl;
        this.listForm.refreshList();
        return true;

    }


}
