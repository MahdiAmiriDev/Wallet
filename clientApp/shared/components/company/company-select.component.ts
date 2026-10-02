import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {GenericDataService} from "../../../shared/base/services/generic-data.service";
import {DxTreeViewComponent} from "devextreme-angular/ui/tree-view";
import {DxDropDownBoxComponent} from "devextreme-angular/ui/drop-down-box";
import {Selectable} from "../../../shared/base/classes/selectable";
import {UserDataService} from '../../../_services/user-data.service';

@Component({
  selector: 'app-company-select',
  templateUrl: './company-select.component.html'
})

export class CompanySelectComponent extends Selectable implements OnInit {

  @Input()
  representationId: number;
  @Input()
  showCheckBoxesMode = 'none';
  @Input()
  readOnly: boolean = false;
  @Input()
  selectionMode = 'single';
  @ViewChild(DxTreeViewComponent) treeView;
  @Input()
  selectedItemId: number;
  columns: any[] = [];
  @Input() selectedCompanyId: any;
  companyDataSource: any;
  isVisible = false;
  type = '';
  @Input()
  disabled: boolean;
  @Input() isRootNodeSelectable: boolean = false;
  additionalUserProp: any;
  constructor(private genericDataService: GenericDataService, private userDataService: UserDataService) {
    super();
  }

  // tslint:disable-next-line:variable-name
  _companyId: number;
  @Input()
  isRequired: boolean;
  @Input()
  requiredMessage: string;

  get companyId(): number {
    return this._companyId;
  }

  @Input()
  set companyId(val: number) {
    if (this._companyId !== val) {
      this._companyId = val;
      this.bindDataSources();
    }
  }

  ngOnInit(): void {
    this.additionalUserProp = this.userDataService.getAdditionalUserProp();
    if (this.additionalUserProp && this.additionalUserProp.companyId) {
      this.companyId = this.additionalUserProp.companyId;
      this.selectedCompanyId = this.companyId;
    }
    this.bindDataSources();
  }

  treeView_itemSelectionChanged(e): void {
    if (e && e.itemData.id && (this.isRootNodeSelectable || e.itemData.parentId)) {
      this.selectedCompanyId = e.itemData.id;
      this.selectedRowChanged.emit(e);
    } else {
      this.type = 'error';
      this.isVisible = true;
    }
  }
  syncTreeViewSelection(e): void {
    const component = (e && e.component) || (this.treeView && this.treeView.instance);

    if (!component) {
      return;
    }
  }
  closeDDl(ddlCompany: DxDropDownBoxComponent): void {
    if (this.selectionMode === 'single') {
      ddlCompany.instance.close();
    }
  }

  private bindDataSources(): void {
    //this.companyDataSource =  this.genericDataService.getEntitiesWithAction('company', 'GetCompanyWithSubset', null)
    this.companyDataSource = this.genericDataService.createCustomDatasource('id', 'company');
  }

  //زمانی که کاربر ایتم انتخاب شده را حذف کند این متد فراخوانی می شود
  onSelectedItemRemove(selectedItem:any): void {
     if(selectedItem.value == null){
       this.selectedRowChanged.emit(null);
     }
  }

}
