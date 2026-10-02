import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ProductDetailComponent} from './product-detail.component';
import {SimpleProductDetailTabBasedComponent} from './simple-product/simple-product-detail-tab-based.component';
import {CachedKey} from '../../../shared/enums/enum';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {CachedDataService} from '../../../shared/services/cached-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';
import {UserDataService} from '../../../_services/user-data.service';


@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html'
})
export class ProductListComponent extends Selectable implements OnInit {
  @ViewChild('frmProduct', {static: true}) frmProduct: BaseList;

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
  detailComponent = SimpleProductDetailTabBasedComponent;
  private baseListUrl = 'product/GetByCustomFilterForList?';

  constructor(private genericDataService: GenericDataService, private cachedDataService: CachedDataService,private userDataService: UserDataService) {
    super();
    this.contentHeight = window.innerHeight - 110;
    this.columns.push({
      caption: 'کد',
      dataField: 'code'
    },{
      caption: 'عنوان',
      dataField: 'title',
      width: '40%'
    },{
      caption: 'مرکز',
      dataField: 'companyTitle'
    },{
      caption: 'محصول گروهی',
      dataField: 'isGroupProduct',
      with: 50
    },{
      caption: 'انتشار داده شده',
      dataField: 'published',
      with: 50
    });

  }

  _companyId: number;

  get companyId(): number {
    return this._companyId;
  }

  @Input()
  set companyId(val) {
    if (val !== this._companyId) {
      this._companyId = val;
    }
  }

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
    }
    this.companyDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'company');

    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  afterInitialDetailComponent(componentInstance: any) {
    const comp = (<ProductDetailComponent>componentInstance.instance);

    if (this.companyId) {
      comp.companyId = this.companyId;
    }

    if (this.appEntityId) {
      comp.appEntityId = this.appEntityId;
    }

    if (this.productTemplateId) {
      comp.productTemplateId = this.productTemplateId;
    }

    if (this.giftCardTypeId) {
      comp.giftCardTypeId = this.giftCardTypeId;
    }

    if (this.downloadActivationType) {
      comp.downloadActivationType = this.downloadActivationType;
    }

    if (this.recurringCyclePeriodType) {
      comp.recurringCyclePeriodType = this.recurringCyclePeriodType;
    }

    if (this.taxCategoryId) {
      comp.taxCategoryId = this.taxCategoryId;
    }

    if (this.basePriceUnitType) {
      comp.basePriceUnitType = this.basePriceUnitType;
    }

    if (this.basePriceBaseUnitType) {
      comp.basePriceBaseUnitType = this.basePriceBaseUnitType;
    }

  }

  public bindCustomDataSources() {
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
    }
    let res = false;
    if (this.companyId || this.appEntityId || this.productTemplateId ||
      this.giftCardTypeId || this.downloadActivationType || this.recurringCyclePeriodType ||
      this.taxCategoryId || this.basePriceUnitType || this.basePriceBaseUnitType) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }


  selectedCompanyChanged(e): void {
    if (e && e.selectedItem) {
      this.categoryId = null;
    }
  }


  selectedCategoryChanged(item: any): void {
    this.selectedCategory = item;
    if (this.selectedCategory && !this.selectedCategory.isMainCategory) {
      this.categoryId = item.id;
    }
    this.bindDataSources();
  }

  private bindDataSources(): void {
    if (this.selectedCategory) {
      this.frmProduct.customListUrl = `product/GetByCustomFilterForList?categoryId=${this.selectedCategory.id}&companyId=${this.companyId}`;
    }
    this.frmProduct.refreshList();
  }

}
