import {AfterViewInit, Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';
import {CategoryGroupEnum} from "../../../shared/enums/category-group-enum";
import {ProductTypeEnum} from "../../../shared/enums/product-type-enum";
import {UserDataService} from "../../../_services/user-data.service";


@Component({
  selector: 'app-course-detail',
  templateUrl: './course-detail.component.html'
})
export class CourseDetailComponent extends BaseEdit implements OnInit {
  @ViewChild(DxTreeViewComponent) treeView;
  @Input()
  companyId: number;
  @Input()
  categoryId: number;
  lmsMode: boolean;
  categoryDataSource: any;
  companyDataSource: any;
  skillTypes: any;
  certificateTypes: any;
  evaluationMethods: any;
  accessLevels: any[] = [];
  firstLoad = false;
  taxCategoryDataSource: any;
  taxCategoryId: number;
  productType: number;
  categoryGroupId: number;
  showProductTypeDropDown: boolean;
  productGroupDataSource: any;

  constructor(private genericDataService: GenericDataService, private userDataService: UserDataService) {
    super(genericDataService);
    this.entityName = 'product';
    this.selectedCompanyChanged = this.selectedCompanyChanged.bind(this);
    this.lmsMode = this.userDataService.getSiteConfigurationLmsMode();
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.genericDataService.getCommonBaseDataForSelect('ProductType').subscribe(res => this.productType = res);
    this.taxCategoryDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'taxCategory');
    if (!this.entity.course) {
      this.entity.course = {};
      // this.entity.categories = [];
    }
    if (!this.selectedEntityId) {
      this.getDefaultCourseTaxCategory();
      this.initialEntityDefaultValues();
    }
    if (this.selectedEntityId) {
      this.firstLoad = true;
    } else if (!this.selectedEntityId && this.companyId) {
      this.entity.companyId = this.companyId;
      this.entity.categories = [this.categoryId];
      this.bindDataSources(this.companyId);
      this.firstLoad = true;
    } else {
      this.firstLoad = false;
    }
    this.companyDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'company');
    this._genericDataService.getCommonBaseDataForSelect('SkillType').subscribe(res => this.skillTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('CertificateType').subscribe(res => this.certificateTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('EvaluationMethod').subscribe(res => this.evaluationMethods = res);
    this.productGroupDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'productGroup');
  }

  allowSetCompany(): boolean {
    return this.companyId != null && this.companyId !== undefined;
  }

  selectedCompanyChanged(e): void {
    if (e && e.selectedItem) {
      if (!this.firstLoad) {
        this.entity.categories = {};
        this.bindDataSources(e.selectedItem.id);
        this.companyId = e.selectedItem.id;
      }
      this.firstLoad = false;
    }
  }

  afterLoadData(res: any): any {
    this.bindDataSources(this.entity.companyId);
    return super.afterLoadData(res);
  }

  selectedCategoryChanged(e): void {
    this.entity.categories = [e];

  }

  //زمانی که یک  دسته بندی انتخاب می شود این متد مقدار
  //CategoryGroupId
  //را برای آن دسته بندی ذخیره می کند
  onCategoryGroupIdSelected(categoryGroupId: number): void {
    this.categoryGroupId = categoryGroupId;
    this.entity.categoryGroupId = categoryGroupId;
    this.setProductTypeDropDownShow();
  }

  private bindDataSources(id: any): void {
    /*    this.categoryDataSource =
          this._genericDataService.getCustomEntitiesByUrl(`api/category/GetByCustomForSelect?companyId=${id}`)
            .subscribe((res: any) => {
              this.categoryDataSource = res.data;
    });*/
  }

  private initialEntityDefaultValues(): void {
    this.entity.visibleIndividually = true;
  }

  getDefaultCourseTaxCategory() {
    let key = 'YashilTms.Tax.DefaultCourseTaxCategoryId';
    this.genericDataService.getEntitiesWithAction('AppConfig', `GetAppConfigValue?key=${key}`, null)
      .subscribe((res: any) => {
        if (res) {
          this.entity.taxCategoryId = res.value;
        }
      });
  }

  //این متد تصمیم گیری در رابطه با نمایش یا عدم نمایش
  //دراپ داون مربوط به نوع محصول را انجام می دهد
  setProductTypeDropDownShow(): void {
    switch (this.categoryGroupId) {
      case CategoryGroupEnum.Skill:
        this.showProductTypeDropDown = true;
        break;
      case CategoryGroupEnum.employment:
        this.showProductTypeDropDown = false;
        this.entity.productType = ProductTypeEnum.Employment;
        break;
      default:
        this.showProductTypeDropDown = false;
        this.entity.productType = ProductTypeEnum.Other;
        break;
    }
  }

  //زمانی که کاربر مقدار گروه محصول را انتخاب می کند این متد فراخوانی می شود
  productGroupSelected(selectedProductGroupId: any): void {
    if (selectedProductGroupId){
      this.entity.productGroupId = selectedProductGroupId
    }
  }

  //زمانی که کاربر مقدار گروه محصول شامل مالیات را انتخاب می کند این متد فراخوانی می شود
  productGroupIncludeTaxSelected(selectedProductGroupId: any): void {
    if (selectedProductGroupId){
      this.entity.productGroupIdIncludeTax = selectedProductGroupId
    }
  }

  //دریافت display expression مربوط به گروه فیلد گروه محصول
  getProductGroupDisplayExpr(item): any{
    if(item)
    return `${item.title} - ${item.code} - ${item.isIncludeTax ? 'شامل مالیات':'بدون مالیات'}`
  }
}
