import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseList} from '../../../shared/base/classes/base-list';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {CachedDataService} from '../../../shared/services/cached-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';
import {Selectable} from '../../../shared/base/classes/selectable';
import {CourseDetailTabBasedComponent} from './course-detail-tab-based.component';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';
import {UserDataService} from '../../../_services/user-data.service';

@Component({
  selector: 'app-course-custom-list',
  templateUrl: './course-custom-list.component.html'
})
export class CourseCustomListComponent extends Selectable implements OnInit {

  selectedCategory: any;
  @ViewChild(DxTreeViewComponent) treeView;
  categoryId: number;
  contentHeight: number;
  companyDataSource: any;
  additionalUserProp: any;
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'product';
  detailComponent = CourseDetailTabBasedComponent;
  private baseListUrl = 'product/GetByCustomFilterForList?';
  customArchiveAction="ArchiveProduct";
  customRestoreActionName="RestoreFromArchiveProduct";
  customButtonCheckBoxTitle:string="نمایش موارد آرشیو شده"
  customCheckboxIsChecked : boolean = false;

  constructor(private genericDataService: GenericDataService, private cachedDataService: CachedDataService,
              private translate: TranslateService,private userDataService: UserDataService) {
    super();
  }

  // tslint:disable-next-line:variable-name
  _companyId: number;

  @Input()
  set companyId(val) {
    if (val !== this._companyId) {
      this._companyId = val;
    }
  }

  get companyId(): number {
    return this._companyId;
  }


  _representationId: number;

  @Input()
  set representationId(val) {
    if (val !== this._representationId) {
      this._representationId = val;
    }
  }

  get representationId(): number {
    return this._representationId;
  }


  // tslint:disable-next-line:variable-name
  _appEntityId: number;

  get appEntityId(): number {
    return this._appEntityId;
  }

  @Input()
  set appEntityId(val) {
    if (val !== this._appEntityId) {
      this._appEntityId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _productTemplateId: number;

  get productTemplateId(): number {
    return this._productTemplateId;
  }

  @Input()
  set productTemplateId(val) {
    if (val !== this._productTemplateId) {
      this._productTemplateId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _giftCardTypeId: number;

  get giftCardTypeId(): number {
    return this._giftCardTypeId;
  }

  @Input()
  set giftCardTypeId(val) {
    if (val !== this._giftCardTypeId) {
      this._giftCardTypeId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _downloadActivationType: number;

  get downloadActivationType(): number {
    return this._downloadActivationType;
  }

  @Input()
  set downloadActivationType(val) {
    if (val !== this._downloadActivationType) {
      this._downloadActivationType = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _recurringCyclePeriodType: number;

  get recurringCyclePeriodType(): number {
    return this._recurringCyclePeriodType;
  }

  @Input()
  set recurringCyclePeriodType(val) {
    if (val !== this._recurringCyclePeriodType) {
      this._recurringCyclePeriodType = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _taxCategoryId: number;

  get taxCategoryId(): number {
    return this._taxCategoryId;
  }

  @Input()
  set taxCategoryId(val) {
    if (val !== this._taxCategoryId) {
      this._taxCategoryId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _basePriceUnitType: number;

  get basePriceUnitType(): number {
    return this._basePriceUnitType;
  }

  @Input()
  set basePriceUnitType(val) {
    if (val !== this._basePriceUnitType) {
      this._basePriceUnitType = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _basePriceBaseUnitType: number;

  get basePriceBaseUnitType(): number {
    return this._basePriceBaseUnitType;
  }

  @Input()
  set basePriceBaseUnitType(val) {
    if (val !== this._basePriceBaseUnitType) {
      this._basePriceBaseUnitType = val;
    }
  }

  ngOnInit(): void {
    this.additionalUserProp = this.userDataService.getAdditionalUserProp();
    if (this.additionalUserProp && this.additionalUserProp.companyId) {
      this.companyId = this.additionalUserProp.companyId;
      this.categoryId = null;
    } else {
      this.companyDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'company');
    }
    if (this.additionalUserProp && this.additionalUserProp.representationId) {
      this.representationId = this.additionalUserProp.representationId;
    }

    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }

    this.contentHeight = window.innerHeight - 110;

    let result = forkJoin({
      codeTitle: this.translate.get('Code'),
      courseTitle: this.translate.get('tms.Course.Title'),
      categoryLeafTitle: this.translate.get('shp.Category.Leaf'),
      productTypeTitle: this.translate.get('shp.Product.productTypeTitle'),
      companyTitle:this.translate.get('shp.Company.Company'),
      productDurationTitle: this.translate.get('shp.Product.Duration')
    });

    result.subscribe(({codeTitle, courseTitle, categoryLeafTitle,companyTitle,productTypeTitle, productDurationTitle}) => {
      this.columns.push({
        caption: codeTitle,
        dataField: 'code',
        allowHeaderFiltering: false,
        width: '15%',
      });
      this.columns.push({
        caption: 'شناسه دوره',
        dataField: 'id',
        width: '15%',
        allowHeaderFiltering: false,
      });
      this.columns.push({
        caption: courseTitle,
        dataField: 'title',
        width: '30%',
        allowHeaderFiltering: false,
      });
      this.columns.push({
        caption: categoryLeafTitle,
        dataField: 'categoryTitle',
        width: '15%',
        allowHeaderFiltering: false,
      });
      this.columns.push({
        caption: companyTitle,
        dataField: 'companyTitle',
        width: '15%',
        allowHeaderFiltering: false,
      });
      this.columns.push({
        caption: productTypeTitle,
        dataField: 'productTypeTitle',
        width: '15%',
        allowHeaderFiltering: false,
      });
      this.columns.push({
        caption: productDurationTitle,
        dataField: 'duration',
        width: '15%',
        allowHeaderFiltering: false,
      });
    })
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as CourseDetailTabBasedComponent);

    if (this.companyId) {
      comp.companyId = this.companyId;
    }
    if (componentInstance.selectedItem && componentInstance.selectedItem.courseId) {
      comp.courseId = componentInstance.selectedItem.courseId;
    }
  }

  public bindCustomDataSources(): boolean {
    let res = false;
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.companyId) {
        customListUrl += `companyId=${this.companyId}&`;
      }
      if (this.appEntityId) {
        customListUrl += `appEntityId=${this.appEntityId}&`;
      }
      if (this.productTemplateId) {
        customListUrl += `productTemplateId=${this.productTemplateId}&`;
      }
      if (this.giftCardTypeId) {
        customListUrl += `giftCardTypeId=${this.giftCardTypeId}&`;
      }
      if (this.downloadActivationType) {
        customListUrl += `downloadActivationType=${this.downloadActivationType}&`;
      }
      if (this.recurringCyclePeriodType) {
        customListUrl += `recurringCyclePeriodType=${this.recurringCyclePeriodType}&`;
      }
      if (this.taxCategoryId) {
        customListUrl += `taxCategoryId=${this.taxCategoryId}&`;
      }
      if (this.basePriceUnitType) {
        customListUrl += `basePriceUnitType=${this.basePriceUnitType}&`;
      }
      if (this.basePriceBaseUnitType) {
        customListUrl += `basePriceBaseUnitType=${this.basePriceBaseUnitType}&`;
      }

      customListUrl += `showArchives=${this.customCheckboxIsChecked}&`;

    }
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    res = true;
    return res;
  }


  selectedCompanyChanged(selectedItem:any){
    if (selectedItem && selectedItem.itemData) {
      this.companyId = selectedItem.itemData.id;
      this.categoryId = null;
      this.bindCustomDataSources();
    }
  }


  selectedCategoryChanged(item: any): void {
    this.selectedCategory = item;
    if (this.selectedCategory && !this.selectedCategory.isMainCategory) {
      this.categoryId = item.id;
    }
    if (this.companyId) {
      this.bindDataSources();
    }
  }

  private bindDataSources(): void {
    if (this.selectedCategory) {
     let customListUrl = `product/GetByCustomFilterForList?categoryId=${this.selectedCategory.id}&companyId=${this.companyId}&`;
      customListUrl += `showArchives=${this.customCheckboxIsChecked}&`;
      this.listForm.customListUrl = customListUrl;
    }
    this.listForm.refreshList();
  }


  onCustomCheckBoxChange(value: boolean) {
    this.customCheckboxIsChecked = value;
    this.bindCustomDataSources();
  }

}
