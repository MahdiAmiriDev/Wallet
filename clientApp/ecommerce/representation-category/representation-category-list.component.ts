import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {RepresentationCategoryDetailComponent} from './representation-category-detail.component';
import {TranslateService} from "@ngx-translate/core";
import {forkJoin} from "rxjs";


@Component({
  selector: 'app-representation-category-list',
  templateUrl: './representation-category-list.component.html'
})
export class RepresentationCategoryListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  formLabels: any = {};
  entityName = 'representationCategory';
  detailComponent = RepresentationCategoryDetailComponent;
  private baseListUrl = 'representationCategory/GetByCustomFilterForList?';

  constructor(private translate: TranslateService) {
    super();
    this.bindColumns = this.bindColumns.bind(this);
  }

  // tslint:disable-next-line:variable-name
  _representationId: number;

  get representationId(): number {
    return this._representationId;
  }

  @Input()
  set representationId(val) {
    if (val !== this._representationId) {
      this._representationId = val;
    }
  }

  // tslint:disable-next-line:variable-name
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
      representationTitle: this.translate.get('shp.RepresentationCategory.Representation'),
      categoryTitle: this.translate.get('shp.RepresentationCategory.Category'),
    });
    result.subscribe(({
                        representationTitle,
                        categoryTitle,
                      }) => {

      this.formLabels.representationTitle = representationTitle;
      this.formLabels.categoryTitle = categoryTitle;

      this.bindColumns();
      if (this.bindCustomDataSources()) {
        this.loadAfterSetFilter = true;
      }


    });

  }

  bindColumns(): void {
    if (!this.representationId) {
      this.columns.push({
        caption: this.formLabels.representationTitle,
        dataField: 'representationTitle',
        groupIndex: 1
      });
    }
    this.columns.push({
      caption: this.formLabels.categoryTitle,
      dataField: 'categoryTitle',
      allowGrouping: 0
    });
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as RepresentationCategoryDetailComponent);

    if (this.representationId) {
      comp.representationId = this.representationId;
    }

    if (this.categoryId) {
      comp.categoryId = this.categoryId;
    }

  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.representationId) {
        customListUrl += `representationId=${this.representationId}&`;
      }
      if (this.categoryId) {
        customListUrl += `categoryId=${this.categoryId}&`;
      }
    }
    let res = false;
    if (this.representationId || this.categoryId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }
}
