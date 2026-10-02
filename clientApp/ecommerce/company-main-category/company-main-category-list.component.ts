import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {CompanyMainCategoryDetailComponent} from './company-main-category-detail.component';
import {TranslateService} from "@ngx-translate/core";
import {forkJoin} from "rxjs";


@Component({
  selector: 'app-company-main-category-list',
  templateUrl: './company-main-category-list.component.html'
})
export class CompanyMainCategoryListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  formLabels: any = {};
  columns: any[] = [];
  entityName = 'companyMainCategory';
  detailComponent = CompanyMainCategoryDetailComponent;
  private baseListUrl = 'companyMainCategory/GetByCustomFilterForList?';

  constructor(private translate: TranslateService) {
    super();
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

  _categoryId: number;

  get categoryId(): number {
    return this._categoryId;
  }

  @Input()
  set categoryId(val) {
    if (val !== this._categoryId) {
      this._categoryId = val;
    }
  }

  ngOnInit(): void {
    let result = forkJoin({
      companyTitle: this.translate.get('shp.CompanyMainCategory.Company'),
      mainCategoryTitle: this.translate.get('shp.CompanyMainCategory.Category'),
    });
    result.subscribe(({
                        companyTitle,
                        mainCategoryTitle,
                      }) => {
      this.formLabels.companyTitle = companyTitle;
      this.formLabels.mainCategoryTitle = mainCategoryTitle;

      this.bindColumns()
      if (this.bindCustomDataSources()) {
        this.loadAfterSetFilter = true;
      }
    });
  }

  afterInitialDetailComponent(componentInstance: any) {
    const comp = (<CompanyMainCategoryDetailComponent>componentInstance.instance);

    if (this.companyId) {
      comp.companyId = this.companyId;
    }

    if (this.categoryId) {
      comp.categoryId = this.categoryId;
    }

  }

  public bindCustomDataSources() {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.companyId) {
        customListUrl += `companyId=${this.companyId}&`;
      }
      if (this.categoryId) {
        customListUrl += `categoryId=${this.categoryId}&`;
      }
    }
    let res = false;
    if (this.companyId || this.categoryId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }

  bindColumns(): void {
    if (!this.companyId) {
      this.columns.push({
        caption: this.formLabels.companyTitle,
        dataField: 'companyTitle',
        groupIndex: 1
      });
    }
    this.columns.push({
      caption: this.formLabels.mainCategoryTitle,
      dataField: 'mainCategoryTitle',
      allowGrouping: 0
    });
  }
}
