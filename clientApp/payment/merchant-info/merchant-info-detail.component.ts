import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';

@Component({
  selector: 'app-merchant-info-detail',
  templateUrl: './merchant-info-detail.component.html'
})
export class MerchantInfoDetailComponent extends BaseEdit implements OnInit {
  @Input()
  state: number;
  @Input()
  city: number;
  accessLevels: any[] = [];
  cityDataSource: any;
  usersDataSource: any;
  @ViewChild(DxTreeViewComponent) treeView;
  merchantRoleId: number;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'merchantInfo';

  }

  ngOnInit(): void {
    super.ngOnInit();
    this.cityDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'city');
    this.getMerchantRoleIdAndSetUserDataSource();

  }

  syncTreeViewSelection(e): void {
    const component = (e && e.component) || (this.treeView && this.treeView.instance);
    if (!component) {
      return;
    }
  }

  treeView_itemSelectionChanged(e): void {
    if (e.itemData.parentId) {
      this.entity.cityId = e.itemData.id;
    }
  }

  selectedPersonChanged(selectedPersonId: any): void {
    this.entity.userId = selectedPersonId;
  }

  getMerchantRoleIdAndSetUserDataSource(): void {
    this.setBusy(true);
    this.genericDataService.getEntity(this.entityName, null, 'GetMerchantRoleId').subscribe(res => {
      this.setBusy(false);
      if (res && res.isSuccess) {
        this.merchantRoleId = res.entityId;
        this.usersDataSource = this.genericDataService.createCustomDataSourceWithAction('id', 'user', `GetUserByRoleIdIfAccess?roleId=${this.merchantRoleId}`);
      }
    });
  }
}
