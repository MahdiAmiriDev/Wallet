import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  Input,
  OnInit,
  QueryList,
  ViewChild,
  ViewChildren
} from '@angular/core';
import {BaseList} from '../../../shared/base/classes/base-list';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';
import {CoursePlanningDetailTabBasedComponent} from './courses-planning-detail-tab-based.component';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';
import {CoursePlanningRouteEnum} from "./course-planning-route-enums";
import {UserDataService} from '../../../_services/user-data.service';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {Selectable} from '../../../shared/base/classes/selectable';


@Component({
  selector: 'app-course-planning-custom-list',
  templateUrl: './course-planning-custom-list.component.html'
})
/*
 دسترسی به این کامپوننت از route های مختلفی صورت می پذیرد
 1- مسیر اول customCoursePlannings که مربوط به پروژه lms می باشد
 مسیر دوم coursePlannings که مربوط به استفاده در پروژه زیر ساخت است
 در لحظه ای initial شدن کامپوننت برسی می کنیم که از کدام مسیر به این کامپوننت وارد شده ایم
 اگر مسیر مربوط به پروژه زیر ساخت باشد دکمه ی ثبت تخفیف را نمایش می دهیم
 در غیر این صورت اگر مسیر مربوط به پروژه lms باشد دکمه ی ثبت تخفیف را نمایش نمی دهیم

 2-اگر از مسیر coursePlanning وارد کامپوننت شویم isCourse = true شده در
  این حالت تب های usedFor - hardwareRequirement - softwareRequirement -
   را نمایش نمی دهیم در غیر این صورت اگر مسیر CustomCoursePlannings باشد مقدار isCourse = false بوده
   و تب های نام برده را نمایش می دهیم

  3- عنوان فرم لیست با توجه به مسیر اگر مسیر برابر customCoursePlannings باشد برنامه ریزی دوره بوده
  اگر  مسیر برابر coursePlannings باشد عنوان فرم لیست برابر با تعریف و برنامه ریزی دوره خواهد بود
 */
export class CoursePlanningCustomListComponent extends Selectable implements OnInit, AfterViewInit {

  @ViewChildren("frmCourse") baseList: QueryList<BaseList>;
  @ViewChild('categoryListComponent', {static: true}) categoryListComponent: BaseList;
  @ViewChild(DxTreeViewComponent) treeView;
  @Input() forSelect = false;
  @Input() courseInfo: {};
  @Input() hideCompanyColumn = false;

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
  customRowButtons: YashilCustomButtonColumn;
  popUpTitle: string;
  showPopUp: boolean = false;
  productSalePlanningId: number;
  showSetDiscountButton: boolean = true;
  title:string = 'tms.Course.Planning';
  // جهت نمایش برنامه ریزی فعال
  showConfirm = false;
  private frmCourse: BaseList;
  // tslint:disable-next-line:variable-name
  private _companyId: number;
  customArchiveActionName:string = "archiveProductSalesPlanning";
  customRestoreActionName:string="RestoreProductSalesPlanningFromArchive";
  customButtonCheckBoxTitle:string="نمایش موارد آرشیو شده";
  customCheckboxIsChecked:boolean=false;
  lmsMode : boolean;
  get companyId(): number {
    return this._companyId;
  }


  constructor(private genericDataService: GenericDataService,
              private translate: TranslateService, private userDataService: UserDataService, private changeDetector: ChangeDetectorRef) {
    super();
    this.lmsMode = this.userDataService.getSiteConfigurationLmsMode();
    this.gridHeight = window.innerHeight - 180;
    this.showProductSalePlanningDiscount = this.showProductSalePlanningDiscount.bind(this);
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as CoursePlanningDetailTabBasedComponent);
    comp.companyId = this.companyId;
    comp.categoryId = this.categoryId;
    comp.representationId = this.representationId;
    comp.isCourse = this.isCourse;
    comp.isAdmin = componentInstance.selectedItem.isAdmin;
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
    // بررسی آدرسی که در آن هستیم و تصمیم گیری راجع به اینکه چه ستون هایی را نمایش دهیم
    // این کار برای این است که بتوانیم تمایز ستون ها در پروژه زیر ساخت و lms را اعمال کنیم
    let currentRoute = window.location.pathname.split("/")[2];
    switch (currentRoute) {
      case CoursePlanningRouteEnum.CoursePlannings: {
        this.isCourse = true;
        break;
      }
      case CoursePlanningRouteEnum.CustomCoursePlannings: {
        this.showSetDiscountButton = false;
        this.title = 'shp.ProductSalesPlanning.CustomCoursePlannings.Title';
        break;
      }
    }
    this.companyDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'company');
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.bindColumns();
    }, 0)
    this.baseList.changes.subscribe((component: QueryList<BaseList>) => {
      this.frmCourse = component.first;
      this.bindDataSources();
    });
  }

  syncTreeViewSelection(e): void {
    const component = (e && e.component) || (this.treeView && this.treeView.instance);

    if (!component) {
      return;
    }
  }

  treeView_itemSelectionChanged(e): void {
    if (!e.itemData.isMainCategory) {
      this.categoryId = e.itemData.id;
    }
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

  selectedCategoryChanged(item: any): void {
    this.selectedCategory = item;
    if (this.selectedCategory) {
        this.categoryId = item.id;
    }
    this.bindDataSources();
  }


  setSelectedCompany(item: any): void {
    if (item.company) {
      this.companyId = item.company.id;
      this.representationId = null;
    }
    this.bindDataSources();
  }

  showProductSalePlanningDiscount(e): void {
    this.productSalePlanningId = e.row.data.id;
    this.popUpTitle = 'تخفیف برنامه ریزی دوره';
    this.showPopUp = true;
  }

  private bindDataSources(): void {
    const baseUrl = 'productSalesPlanning/GetByCategoryIdAndCompanyId?';
    let customListUrl = baseUrl;
    if (this.companyId) {
      customListUrl += `companyId=${this.companyId}&`;
    }
    if (this.selectedCategory) {
      customListUrl += `categoryId=${this.selectedCategory.id}&`;
    }
    if (this.representationId) {
      customListUrl += `representationId=${this.representationId}&`;
    }

    customListUrl += `showArchives=${this.customCheckboxIsChecked}&`;

    if (customListUrl !== baseUrl) {
      this.frmCourse.customListUrl = customListUrl;
      this.frmCourse.refreshList();
    }
    this.changeDetector.detectChanges();
  }

  private bindColumns(): void {
    let result = forkJoin({
      id: this.translate.get('Id'),
      productSalesPlanningTitle: this.translate.get('shp.ProductSalesPlanning.Title'),
      productTitle: this.translate.get('shp.Product.Title'),
      companyTitle : this.translate.get('shp.Company.Company'),
      categoryTitle: this.translate.get('shp.Category.Leaf'),
      courseStatusTitle: this.translate.get('shp.ProductSalesPlanning.CourseStatus'),
      priceTitle: this.translate.get('shp.ProductSalesPlanning.Price'),
      implementationTypeTitle: this.translate.get('shp.ProductSalesPlanning.ImplementationType'),
      confirm: this.translate.get('shp.ProductSalesPlanning.Confirmed'),
      startDateTitle: this.translate.get('shp.ProductSalesPlanning.StartDate'),
      endDateTitle: this.translate.get('shp.ProductSalesPlanning.EndDate'),
      representationTitle: this.translate.get('shp.Representation.Title')
    });

    result.subscribe(({
                        id,
                        productSalesPlanningTitle,
                        productTitle,
                        companyTitle,
                        categoryTitle,
                        courseStatusTitle,
                        priceTitle,
                        implementationTypeTitle,
                        confirm,
                        startDateTitle,
                        endDateTitle,
                        representationTitle
                      }) => {

      this.columns = [
        { caption: id, dataField: 'id', width: '60px' },
        {caption: productSalesPlanningTitle, dataField: 'productSalesPlanningTitle',width: '200px'},
        { caption: productTitle, dataField: 'productTitle', width: '150px' },
        { caption: companyTitle, dataField: 'companyTitle', width: '100px' },
        { caption: categoryTitle, dataField: 'categoryTitle', width: '100px' },
        { caption: courseStatusTitle, dataField: 'courseStatusTitle', width: '100px' },
        { caption: implementationTypeTitle, dataField: 'implementationTypeTitle', width: '80px' },
        { caption: representationTitle, dataField: 'representationTitle', width: '80px' }
      ];

      // فقط وقتی lmsMode = false باشد ستون confirmed نمایش داده شود
      if (!this.lmsMode) {
        this.columns.push({
          caption: confirm,
          dataField: 'confirmed',
          width: '50px'
        },
        { caption: priceTitle,
          dataField: 'price',
          cellTemplate: 'number',
          width: '100px' },
        );
      }

      this.columns.push(
        {
          caption: startDateTitle,
          dataField: 'startDate',
          cellTemplate: 'intDate',
          width: '80px'
        },
        {
          caption: endDateTitle,
          dataField: 'endDate',
          cellTemplate: 'intDate',
          width: '80px'
        }
      );

      this.bindCustomRowButtons();
    });
  }

  bindCustomRowButtons() {
    if (this.showSetDiscountButton) {
      this.customRowButtons = {
        width: '120px',
        customButtons: [
          {
            hint: 'تخفیف برنامه ریزی دوره',
            text: 'ثبت تخفیف',
            icon: 'bulletlist',
            type: 'default',
            stylingMode: 'contained',
            onClick: this.showProductSalePlanningDiscount,
          }]
      };
    } else {
      this.customRowButtons = {
        customButtons: []
      };
    }
  }

  onCustomCheckBoxChange(value: boolean) {
    this.customCheckboxIsChecked = value;
    this.bindDataSources();
  }
}
