import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ProductCategoryDetailComponent} from './product-category-detail.component';
import {TranslateService} from "@ngx-translate/core";
import {forkJoin} from "rxjs";


@Component({
  selector: 'app-product-category-list',
  templateUrl: './product-category-list.component.html'
})
export class ProductCategoryListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  formLabels: any = {};

  entityName = 'productCategory';
  detailComponent = ProductCategoryDetailComponent;
  private baseListUrl = 'productCategory/GetByCustomFilterForList?';

  constructor(private translate: TranslateService) {
    super();
    this.bindColumns = this.bindColumns.bind(this);

  }

  _productId: number;

  get productId(): number {
    return this._productId;
  }

  @Input()
  set productId(val) {
    if (val !== this._productId) {
      this._productId = val;
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
      productTitle: this.translate.get('shp.ProductCategory.Product'),
      categoryTitle: this.translate.get('shp.ProductCategory.Category'),
    })

    result.subscribe(({
                        productTitle,
                        categoryTitle,
                      }) => {

      this.formLabels.productTitle = productTitle;
      this.formLabels.categoryTitle = categoryTitle;

      this.bindColumns();
      if (this.bindCustomDataSources()) {
        this.loadAfterSetFilter = true;
      }


    });


  }

  afterInitialDetailComponent(componentInstance: any) {
    const comp = (<ProductCategoryDetailComponent>componentInstance.instance);

    if (this.productId) {
      comp.productId = this.productId;
    }

    if (this.categoryId) {
      comp.categoryId = this.categoryId;
    }

  }

  public bindCustomDataSources() {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.productId) {
        customListUrl += `productId=${this.productId}&`;
      }
      if (this.categoryId) {
        customListUrl += `categoryId=${this.categoryId}&`;
      }
    }
    let res = false;
    if (this.productId || this.categoryId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }

  bindColumns(): void {
    this.columns.push({
      caption: this.formLabels.productTitle,
      dataField: 'productTitle'
    }, {
      caption: this.formLabels.categoryTitle,
      dataField: 'categoryTitle'
    });
  }
}
