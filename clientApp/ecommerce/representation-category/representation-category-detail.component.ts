import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';
import {TranslateService} from "@ngx-translate/core";


@Component({
  selector: 'app-representation-category-detail',
  templateUrl: './representation-category-detail.component.html'
})
export class RepresentationCategoryDetailComponent extends BaseEdit implements OnInit {
  @ViewChild(DxTreeViewComponent) treeView;
  @Input()
  representationId: number;
  @Input()
  categoryId: number;
  @Input()
  accessLevelId: number;
  representationDataSource: any;
  categoryDataSource: any;
  accessLevels: any[] = [];
  rootNoteSelectErrorMessage: string;

  constructor(private genericDataService: GenericDataService,private translate: TranslateService) {
    super(genericDataService);
    this.entityName = 'representationCategory';
    this.onRepresentationChange = this.onRepresentationChange.bind(this);
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.translate.get('shp.Company.Validation.SubCompanyShouldBeSelectedFromTheLowestSubCompany').subscribe(res => {
      this.rootNoteSelectErrorMessage = res;
    });
    if (this.representationId) {
      this.entity.representationId = this.representationId;
    }
    if (this.categoryId) {
      this.entity.categoryId = this.categoryId;
    }
    if (this.accessLevelId) {
      this.entity.accessLevelId = this.accessLevelId;
    }

    this.representationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'representation');
    this.bindCategoryDataSources();
  }

  syncTreeViewSelection(e): void {
    const component = (e && e.component) || (this.treeView && this.treeView.instance);

    if (!component) {
      return;
    }
  }

  treeView_itemSelectionChanged(e): void {
    if (e.itemData.parentId) {
      this.categoryId = e.itemData.id;
      this.entity.categoryId = this.categoryId;
    } else {
      this.showError(this.rootNoteSelectErrorMessage);
    }
  }

  private bindCategoryDataSources(): void {
    if (this.entity.representationId) {
      this.categoryDataSource = this._genericDataService.createCustomDataSourceWithAction('id', 'category', 'GetMainCategoriesByRepresentationId?repId=' + this.entity.representationId);
    }
  }

  onRepresentationChange(item: any): void {
    if (item && item.value) {
      this.entity.categoryId = null;
      this.bindCategoryDataSources();
    }
  }

}
