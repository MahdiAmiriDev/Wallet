import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {GenericDataService} from '../../base/services/generic-data.service';
import {CachedDataService} from '../../services/cached-data.service';
import {UserDataService} from '../../../_services/user-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';


@Component({
  selector: 'app-representation-filter',
  templateUrl: './representation-filter.component.html'
})
export class RepresentationFilterComponent implements OnInit {

  // tslint:disable-next-line:variable-name
  _selectedRepresentationId: number;
  // tslint:disable-next-line:variable-name
  _selectedCompanyId: number;
  categoryId: number;

  @Input()
  set selectedCompanyId(val: number) {
    if (val && val !== this._selectedCompanyId) {
      this._selectedCompanyId = val;
      this._companyId = val;
    }
  }

  @Input()
  set selectedRepresentationId(val: number) {
    if (val && val !== this._selectedRepresentationId) {
      this._selectedRepresentationId = val;
      this.representationId = val;
      this.firstLoad = true;
    }
  }

  @Input()
  set selectCategoryId(val: number) {
    if (val && val !== this._selectedRepresentationId) {
      this._selectCategoryId = val;
      this.categoryId = val;
      this.firstLoad = true;
    }
  }

  get selectCategoryId(): number {
    return this._selectCategoryId;
  }

  private _selectCategoryId: number;
  @Input()
  showConfirmRepresentation = false;
  @Input()
  showInOneRow = true;
  @Input()
  showCategoryFilter = false;
  @Input()
  isRequired = false;
  @Input()
  postId: number;
  representationId: number;
  @Input()
  representationLabel = 'نمایندگی: ';
  @Input()
  companyLabel = 'مرکز: ';
  @Input()
  categoryLabel = 'دسته بندی: ';
  selectedData: any = {};
  @Output()
  selectedCategoryChanged: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  selectedRepresentationChanged: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  selectedCompanyChanged: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  additionalUserPropHasCompanyId: EventEmitter<any> = new EventEmitter<any>();

  representationDataSource: any;
  additionalUserProp: any;
  contentHeight: number;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'representationPerson';
  companyDataSource: any;
  categoryDataSource: any;
  // tslint:disable-next-line:variable-name
  private _companyId: number;
  firstLoad: boolean;
  @ViewChild(DxTreeViewComponent) treeView;
  @Input()
  showRepresentation: boolean = true;

  @Input()
  set companyId(val: number) {
    if (val && this._companyId !== val) {
      this._companyId = val;
    }
  }

  get companyId(): number {
    return this._companyId;
  }


  constructor(private genericDataService: GenericDataService, private cachedDataService: CachedDataService, private userDataService: UserDataService) {
    this.contentHeight = window.innerHeight - 110;
  }

  ngOnInit(): void {
    if (this.selectedRepresentationId) {
      this.firstLoad = true;
    }
    this.additionalUserProp = this.userDataService.getAdditionalUserProp();
    if (this.additionalUserProp && this.additionalUserProp.companyId) {
      this.companyId = this.additionalUserProp.companyId;
    }
    if (this.additionalUserProp && this.additionalUserProp.representationId) {
      this.representationId = this.additionalUserProp.representationId;
    } else {
      this.representationId = null;
    }
    this.bindCompanyDataSource();
    if (this.companyId) {
      this.bindRepresentationDataSource(this.companyId);
    }
    if (this.companyId != undefined && this.companyId) {
      this.additionalUserPropHasCompanyId.emit(this.companyId);
    }
  }


  onSelectedCompanyChanged(selectedItem: any) {
    if (selectedItem) {
      this.selectedData.company = selectedItem.itemData;
      this.selectedData.representation = null;
      this.selectedData.category = null;
      if (!this.firstLoad) {
        if (this.additionalUserProp && this.additionalUserProp.representationId) {
          this.representationId = this.additionalUserProp.representationId;
        } else {
          this.representationId = null;
        }
        this.categoryId = null;
      } else {
        this.firstLoad = false;
      }
    }
    // this.bindDataToSelectedData();

    this.selectedCompanyChanged.emit(selectedItem!=null ? this.selectedData:null);

    if (selectedItem?.itemData) {
      this.bindRepresentationDataSource(selectedItem.itemData.id);
    } else {
      this.companyId = null;
      this.representationDataSource = null;
      this.categoryDataSource = null;
      this.representationId = null;
    }
  }

  onSelectedRepresentationChanged(item: any): void {
    this.categoryId = null;
    if (item) {
      this.selectedData.representation = item.selectedItem;
      this.selectedData.category = null;
      if (!this.firstLoad) {
        if (this.additionalUserProp && this.additionalUserProp.categoryId) {
          this.categoryId = this.additionalUserProp.categoryId;
        } else {
          this.categoryId = null;
        }
      } else {
        this.firstLoad = false;
      }
    }
    this.selectedRepresentationChanged.emit(this.selectedData);

    if (item.selectedItem) {
      this.representationId = item.selectedItem.id;
      this.bindCategoryDataSources(item.selectedItem.id);
    } else {
      this.categoryDataSource = null;
      this.representationId = null;
    }
  }

  onSelectedCategoryChanged(item: any): void {
    if (item) {
      this.selectedData.category = item.selectedItem;
      this.selectedCategoryChanged.emit(this.selectedData);
    }
    if (item.selectedItem) {
      this.categoryId = item.selectedItem.id;
    } else {
      this.categoryId = null;
    }
  }

  bindCompanyDataSource(): void {
    this.companyDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'company');
  }

  bindRepresentationDataSource(companyId): void {
    if (companyId) {
      this.representationDataSource =
        this.genericDataService.createCustomDataSourceWithAction('id', 'representation', `GetByCompany?companyId=${companyId}&showConfirmRepresentation=${this.showConfirmRepresentation}`);
    } else {
      this.representationDataSource = null;
    }
  }

  bindCategoryDataSources(representationId): void {
    if (representationId && this.showCategoryFilter) {
      let url = 'GetRepresentationCategories?';
      url += `representationId=${representationId}&`;
      this.categoryDataSource = this.genericDataService.createCustomDataSourceWithAction('id', 'RepresentationCategory', url);
    } else {
      this.categoryDataSource = null;
    }
  }

  syncTreeViewSelection(e): void {
    const component = (e && e.component) || (this.treeView && this.treeView.instance);

    if (!component) {
      return;
    }
  }

}
