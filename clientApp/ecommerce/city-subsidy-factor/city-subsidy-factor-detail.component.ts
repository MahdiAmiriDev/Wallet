import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';


@Component({
  selector: 'app-city-subsidy-factor-detail',
  templateUrl: './city-subsidy-factor-detail.component.html'
})
export class CitySubsidyFactorDetailComponent extends BaseEdit implements OnInit {
  @Input()
  cityId: number;
  cityDataSource: any;
  accessLevels: any[] = [];
  @ViewChild(DxTreeViewComponent) treeView;
  instance: any;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'citySubsidyFactor';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.cityId)
      this.entity.cityId = this.cityId;

    this.cityDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'city');
    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
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
}
