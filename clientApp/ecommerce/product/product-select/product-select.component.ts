import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Selectable} from '../../../../shared/base/classes/selectable';
import {BaseList} from '../../../../shared/base/classes/base-list';
import {SimpleProductDetailTabBasedComponent} from '../simple-product/simple-product-detail-tab-based.component';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';

@Component({
    selector: 'app-product-select',
    templateUrl: './product-select.component.html'
})
export class ProductSelectComponent extends Selectable implements OnInit {
    @Output()
    closeFormClick = new EventEmitter<any>();
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'product';
    detailComponent = SimpleProductDetailTabBasedComponent;
    categoryId: number;
    contentHeight: number;
    searchEntity: any = {};
    companyDataSource: any;
    taxCategoryDataSource: any;
    @Input()
    selectedItems: any;
    private baseListUrl = 'product/GetByCustomFilterForList?';

    constructor(private genericDataService: GenericDataService) {
        super();
        this.columns.push({
            caption: 'عنوان',
            dataField: 'title',
            width: '70%'
        });
        this.columns.push({
            caption: 'مرکز',
            dataField: 'companyTitle'
        });
        this.columns.push({
            caption: 'انتشار داده شده',
            dataField: 'published',
            with: 50
        });

    }

    ngOnInit(): void {
        this.contentHeight = window.innerHeight - 80;
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
        this.companyDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'company');
        this.taxCategoryDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'taxCategory');
    }

    selectedCategoryChanged(item: any) {
        this.searchEntity.categoryId = item;
    }

    selectedSpecificationCategoryChanged($event: any) {
    }

    onFilterForm(e: Event) {
        this.bindCustomDataSources();
    }

    onCloseForm($event: any) {
        this.closeFormClick.emit();
    }

    selectBtnClicked(e: any) {
        this.onSelectionChanged.emit(this.selectedItems);
        this.closeFormClick.emit();
    }

    SelectedProductsChanged(items: any) {
        this.selectedItems = items;
        // this.selectedRowChanged.emit(items);
    }

    public bindCustomDataSources() {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.searchEntity.code) {
                customListUrl += `code=${this.searchEntity.code}&`;
            }
            if (this.searchEntity.title) {
                customListUrl += `title=${this.searchEntity.title}&`;
            }
            if (this.searchEntity.categoryId) {
                customListUrl += `categoryId=${this.searchEntity.categoryId}&`;
            }
            if (this.searchEntity.companyId) {
                customListUrl += `companyId=${this.searchEntity.companyId}&`;
            }
            if (this.searchEntity.appEntityId) {
                customListUrl += `appEntityId=${this.searchEntity.appEntityId}&`;
            }
            if (this.searchEntity.productTemplateId) {
                customListUrl += `productTemplateId=${this.searchEntity.productTemplateId}&`;
            }
            if (this.searchEntity.giftCardTypeId) {
                customListUrl += `giftCardTypeId=${this.searchEntity.giftCardTypeId}&`;
            }
            if (this.searchEntity.downloadActivationType) {
                customListUrl += `downloadActivationType=${this.searchEntity.downloadActivationType}&`;
            }
            if (this.searchEntity.recurringCyclePeriodType) {
                customListUrl += `recurringCyclePeriodType=${this.searchEntity.recurringCyclePeriodType}&`;
            }
            if (this.searchEntity.taxCategoryId) {
                customListUrl += `taxCategoryId=${this.searchEntity.taxCategoryId}&`;
            }
            if (this.searchEntity.basePriceUnitType) {
                customListUrl += `basePriceUnitType=${this.searchEntity.basePriceUnitType}&`;
            }
            if (this.searchEntity.basePriceBaseUnitType) {
                customListUrl += `basePriceBaseUnitType=${this.searchEntity.basePriceBaseUnitType}&`;
            }
        }
        this.listForm.customListUrl = customListUrl;
        this.listForm.refreshList();

        return true;
    }
}
