import { Selectable } from '../../../shared/base/classes/selectable';
import { BaseList } from '../../../shared/base/classes/base-list';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { CoursePlanningDetailTabBasedComponent } from './courses-planning-detail-tab-based.component';
import { TranslateService } from '@ngx-translate/core';
import { forkJoin } from 'rxjs';


@Component({
  selector: 'app-course-planning-list',
  templateUrl: './courses-planning-list.component.html'
})
export class CoursePlanningListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', { static: true }) listForm: BaseList;
  @Input()
  showConfirmOnEditForm: boolean;
  /// null:در نظر نگیر
  /// 0:false
  /// 1:true
  //  2:آنهایی که خالی است
  @Input()
  showAddButton: true;
  @Input()
  confirmed: number;

  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'productSalesPlanning';
  detailComponent = CoursePlanningDetailTabBasedComponent;
  @Input()
  product: any;
  private baseListUrl = 'productSalesPlanning/GetByCustomFilterForList?';

  constructor(private translate: TranslateService) {
    super();
  }

  // tslint:disable-next-line:variable-name
  _productSalesPlanningId: number;

  get productSalesPlanningId(): number {
    return this._productSalesPlanningId;
  }

  @Input()
  set productSalesPlanningId(val) {
    if (val !== this._productSalesPlanningId) {
      this._productSalesPlanningId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _courseStatus: number;

  get courseStatus(): number {
    return this._courseStatus;
  }

  @Input()
  set courseStatus(val) {
    if (val !== this._courseStatus) {
      this._courseStatus = val;
    }
  }

  @Input()
  representationId: number;
  // tslint:disable-next-line:variable-name
  _productId: number;

  get productId(): number {
    return this._productId;
  }

  @Input()
  set productId(val) {
    if (val !== this._productId) {
      this._productId = val;
      this.bindCustomDataSources();
    }
  }

  // tslint:disable-next-line:variable-name
  _ageCategory: number;

  get ageCategory(): number {
    return this._ageCategory;
  }

  @Input()
  set ageCategory(val) {
    if (val !== this._ageCategory) {
      this._ageCategory = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _implementationType: number;

  get implementationType(): number {
    return this._implementationType;
  }

  @Input()
  set implementationType(val) {
    if (val !== this._implementationType) {
      this._implementationType = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _courseType: number;

  get courseType(): number {
    return this._courseType;
  }

  @Input()
  set courseType(val) {
    if (val !== this._courseType) {
      this._courseType = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _runType: number;

  get runType(): number {
    return this._runType;
  }

  @Input()
  set runType(val) {
    if (val !== this._runType) {
      this._runType = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _customGender: number;

  get customGender(): number {
    return this._customGender;
  }

  @Input()
  set customGender(val) {
    if (val !== this._customGender) {
      this._customGender = val;
    }
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as CoursePlanningDetailTabBasedComponent);
    comp.productId = this.productId;
    comp.isCourse = true;
    comp.courseId = componentInstance.selectedItem.courseId;
    comp.isAdmin = componentInstance.selectedItem.isAdmin;
    comp.showConfirmOnEditForm = this.showConfirmOnEditForm;
    if (this.product && this.product && this.product.categories && this.product.categories.length > 0) {
      comp.categoryId = this.product.categories[0];
      comp.companyId = this.product.companyId;
      comp.representationId = this.representationId;
    }
  }

  private bindColumns(): void {

    let result = forkJoin({
      id: this.translate.get('Id'),
      productSalesPlanningTitle: this.translate.get('shp.ProductSalesPlanning.Title'),
      productTitle: this.translate.get('tms.Course.Title'),
      companyTitle: this.translate.get('shp.Company.Title'),
      representationTitle: this.translate.get('shp.ProductSalesPlanning.RepresentationId'),
      durationTitle: this.translate.get('shp.ProductSalesPlanning.Duration'),
      priceTitle: this.translate.get('shp.ProductSalesPlanning.Price'),
      categoryTitle: this.translate.get('shp.Category.Leaf'),
      telephoneTitle: this.translate.get('um.Address.Telephone'),
      implementationTypeTitle: this.translate.get('tms.Course.ImplementationTypeTitle'),
      publishedTitle: this.translate.get('shp.ProductSalesPlanning.Published'),
      confirmedTitle: this.translate.get('shp.ProductSalesPlanning.Confirmed'),
      startDateTitle: this.translate.get('shp.ProductSalesPlanning.StartDate'),
      endDateTitle: this.translate.get('shp.ProductSalesPlanning.EndDate')
    });

    result.subscribe(({ id, productSalesPlanningTitle,productTitle, companyTitle, representationTitle, durationTitle, priceTitle, categoryTitle, telephoneTitle, implementationTypeTitle, publishedTitle, confirmedTitle, startDateTitle, endDateTitle }) => {

      this.columns.push({
        caption: id,
        dataField: 'id',
        width: '80px'
      });
      this.columns.push({
        caption:productSalesPlanningTitle,
        dataField: 'productSalesPlanningTitle',
        width: '200px'
      })
      if (!this.productId) {
        this.columns.push({
          caption: productTitle,
          dataField: 'productTitle',
          width: '200px'
        });
      }
      this.columns.push({
        caption: companyTitle,
        dataField: 'companyTitle',
        width: '150px'
      });

      this.columns.push({
        caption: representationTitle,
        dataField: 'representationTitle',
        width: '150px'
      });
      this.columns.push({
        caption: durationTitle,
        dataField: 'duration',
        width: '100px',
        allowHeaderFiltering: false
      });
      this.columns.push({
        caption: priceTitle,
        dataField: 'price',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false
      });

      this.columns.push({
        caption: categoryTitle,
        dataField: 'categoryTitle',
        allowHeaderFiltering: false,
        width: '150px'
      });
      this.columns.push({
        caption: telephoneTitle,
        dataField: 'telephone',
        width: '120px',
        allowHeaderFiltering: false
      });
      this.columns.push({
        caption: implementationTypeTitle,
        dataField: 'implementationTypeTitle',
        width: '100px',
      });
      this.columns.push({
        caption: publishedTitle,
        dataField: 'published',
        width: '100px'
      });
      this.columns.push({
        caption: confirmedTitle,
        dataField: 'confirmed',
        width: '100px'
      });
      this.columns.push({
        caption: startDateTitle,
        dataField: 'startDate',
        cellTemplate: 'intDate',
        width: '80px',
        allowHeaderFiltering: false
      });
      this.columns.push({
        caption: endDateTitle,
        dataField: 'endDate',
        cellTemplate: 'intDate',
        width: '80px',
        allowHeaderFiltering: false
      });

    });
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.productSalesPlanningId) {
        customListUrl += `productSalesPlanningId=${this.productSalesPlanningId}&`;
      }
      if (this.courseStatus) {
        customListUrl += `courseStatus=${this.courseStatus}&`;
      }
      if (this.productId) {
        customListUrl += `productId=${this.productId}&`;
      }
      if (this.ageCategory) {
        customListUrl += `ageCategory=${this.ageCategory}&`;
      }
      if (this.implementationType) {
        customListUrl += `implementationType=${this.implementationType}&`;
      }
      if (this.courseType) {
        customListUrl += `courseType=${this.courseType}&`;
      }
      if (this.runType) {
        customListUrl += `runType=${this.runType}&`;
      }
      if (this.customGender) {
        customListUrl += `customGender=${this.customGender}&`;
      }
      if (this.confirmed) {
        customListUrl += `confirmed=${this.confirmed}&`;
      }
    }
    let res = false;
    if (this.productSalesPlanningId || this.courseStatus || this.productId ||
      this.ageCategory || this.implementationType || this.courseType || this.runType || this.customGender || this.confirmed) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }
}
