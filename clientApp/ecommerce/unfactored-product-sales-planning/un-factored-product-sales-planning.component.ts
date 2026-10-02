import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, QueryList, ViewChild, ViewChildren} from '@angular/core';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {CachedDataService} from '../../../shared/services/cached-data.service';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';
import {UserDataService} from '../../../_services/user-data.service';
import {DxDataGridComponent} from 'devextreme-angular/ui/data-grid';
import {CoursePlanningDetailTabBasedComponent} from '../course-planning/courses-planning-detail-tab-based.component';


@Component({
    selector: 'app-un-factored-product-sales-planning',
    templateUrl: './un-factored-product-sales-planning.component.html'
})
export class UnFactoredProductSalesPlanningComponent extends Selectable implements OnInit {
    @ViewChild(DxDataGridComponent, {static: false}) dataGrid: DxDataGridComponent;
    @Input()
    lazyLoading = true;
    selectedItemsId: any [] = [];
    productSalesPlanningDataSource: any;
    @Input()
    mode: string ='multiple';
    @Input()
    allowSelectAll: boolean = false;
    @Output()
    onSubmitClick: EventEmitter<any> = new EventEmitter<any>();
    @Output()
    onInitializeComponent: EventEmitter<any> = new EventEmitter<any>();
    @ViewChildren('frmCourse') baseList: QueryList<BaseList>;
    @ViewChild('categoryListComponent', {static: true}) categoryListComponent: BaseList;
    @ViewChild(DxTreeViewComponent) treeView;
    @Input() forSelect = false;
    @Input() courseInfo: {};
    @Input() hideCompanyColumn = false;
    @Output()
    showFilterByProductSalesPlanningBtn: EventEmitter<any> = new EventEmitter<any>();

    @Input()
    set companyId(value: number) {
        if (this._companyId !== value) {
            this._companyId = value;
        }
    }

    additionalUserProp: any;
    categoryId: number;
    contentHeight: number;
    companyDataSource: any;
    selectedCategory: any;
    representationId: number;
    /**
     * Course Category And Education Center Id
     */
    customListUrl: string;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'productSalesPlanning';
    detailComponent = CoursePlanningDetailTabBasedComponent;
    isCourse: boolean = false;
    popUpTitle: string;
    showPopUp: boolean = false;
    productSalePlanningId: number;
    showSetDiscountButton: boolean = true;
    title: string = 'tms.Course.Planning';
    // جهت نمایش برنامه ریزی فعال
    showConfirm = false;
    // tslint:disable-next-line:variable-name
    private _companyId: number;

    get companyId(): number {
        return this._companyId;
    }


    constructor(private genericDataService: GenericDataService, private cachedDataService: CachedDataService,
                private translate: TranslateService, private userDataService: UserDataService, private changeDetector: ChangeDetectorRef) {
        super();
        this.gridHeight = window.innerHeight - 160;
    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as CoursePlanningDetailTabBasedComponent);
        comp.companyId = this.companyId;
        comp.categoryId = this.categoryId;
        comp.representationId = this.representationId;
        comp.isCourse = this.isCourse;
    }

    ngOnInit(): void {
        this.additionalUserProp = this.userDataService.getAdditionalUserProp();
        if (this.additionalUserProp) {
            if (this.additionalUserProp.companyId) {
                this.companyId = this.additionalUserProp.companyId;
            }
            if (this.additionalUserProp.representationId) {
                this.representationId = this.additionalUserProp.representationId;
            }
        }

        this.bindDataSources();
        this.bindColumns();
        this.companyDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'company');
    }

   selectedCompanyChanged(e): void {
        if (e) {
            this.categoryId = null;
        }
    }

    onSelectedRepresentationChanged(item: any): void {
        this.selectedCategory = null;
        if (item.representation) {
            this.representationId = item.representation.id;
        } else {
            this.representationId = null;
        }
        this.selectedCompanyChanged(item.company);
        this.bindDataSources();
    }

    setSelectedCompany(item: any): void {
        if (item.company) {
            this.companyId = item.company.id;
            this.representationId = null;
        }
        this.bindDataSources();
    }


    private bindDataSources(): void {
        const baseUrl = 'api/productSalesPlanning/GetUnFactoredProductSalesPlanning?';
        let customListUrl = baseUrl;
        if (this.companyId) {
            customListUrl += `companyId=${this.companyId}&`;
        }
        if (this.representationId) {
            customListUrl += `representationId=${this.representationId}&`;
        }
        if (customListUrl !== baseUrl) {
            this.genericDataService.getCustomEntitiesByUrl(customListUrl)
                .subscribe((res: any) => {
                    this.productSalesPlanningDataSource = res.data;
                    this.showFilterByProductSalesPlanningBtn.emit(res.data[0].showFilterByProductSalesPlanningBtn);
                });
        }
        this.changeDetector.detectChanges();
    }

    private bindColumns(): void {


        let result = forkJoin({
            id: this.translate.get('Id'),
            productTitle: this.translate.get('shp.Product.Title'),
            categoryTitle: this.translate.get('shp.Category.Leaf'),
            courseStatusTitle: this.translate.get('shp.ProductSalesPlanning.CourseStatus'),
            priceTitle: this.translate.get('shp.ProductSalesPlanning.Price'),
            implementationTypeTitle: this.translate.get('shp.ProductSalesPlanning.ImplementationType'),
            confirm: this.translate.get('shp.ProductSalesPlanning.Confirmed'),
            startDateTitle: this.translate.get('shp.ProductSalesPlanning.StartDate'),
            endDateTitle: this.translate.get('shp.ProductSalesPlanning.EndDate'),


        });

        result.subscribe(({
                              id,
                              productTitle,
                              categoryTitle,
                              courseStatusTitle,
                              priceTitle,
                              implementationTypeTitle,
                              confirm,
                              startDateTitle,
                              endDateTitle
                          }) => {

            this.columns.push({
                caption: id,
                dataField: 'id',
                allowHeaderFiltering: false,
                width: '60px',
            }, {
                caption: productTitle,
                dataField: 'productTitle',
                allowHeaderFiltering: false,
                width: '250px'
            }, {
                caption: categoryTitle,
                dataField: 'categoryTitle',
                width: '200px',
                allowHeaderFiltering: false,
            }, {
                caption: courseStatusTitle,
                dataField: 'courseStatusTitle',
                width: '100px',
                allowHeaderFiltering: false,
            }, {
                caption: priceTitle,
                dataField: 'price',
                cellTemplate: 'number',
                width: '100px',
                allowHeaderFiltering: false,

            }, {
                caption: implementationTypeTitle,
                dataField: 'implementationTypeTitle',
                width: '80px'
            }, {
                caption: startDateTitle,
                dataField: 'startDate',
                cellTemplate: 'intDate',
                width: '80px',
                allowHeaderFiltering: false,
            }, {
                caption: endDateTitle,
                dataField: 'endDate',
                cellTemplate: 'intDate',
                width: '80px',
                allowHeaderFiltering: false
            });

        });
    }

    /* هنگامی که کاربر برنامه ریزی را انتخاب می کند این متد فراخوانی می شود */
    selectedUserChanged(e: any): void {
          this.selectedItemsId = e.selectedRowsData.map(i => (i.id));
    }
    /*این متد برسی می کند که آیا آیتمی انتخاب شده است یا نه اگر انتخاب نشده بود نمایش پیام مناسب در غیر این صورت ارسال ایتم های انتخاب شده */
    checkIsItemSelected() {
        if (this.selectedItemsId.length == 0) {
            this.showError('کاربر گرامی ایتم انتخابی نمی تواند خالی باشد');
        } else {
            this.onSubmitClick.emit(this.selectedItemsId);
        }
    }
}
