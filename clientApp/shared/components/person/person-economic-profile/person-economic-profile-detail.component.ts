import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';

@Component({
    selector: 'app-person-economic-profile-detail',
    templateUrl: './person-economic-profile-detail.component.html'
})
export class PersonEconomicProfileDetailComponent extends BaseEdit implements OnInit {
    @Input()
    personId: number;
    cityDataSource: any;
    @Input()
    personFullName: string = '';
    personEconomicProfileName: string;
    personEconomicProfileEconomicCode:string;
    @ViewChild(DxTreeViewComponent) treeView;

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'personEconomicProfile';
        this.changedIsLegalPerson = this.changedIsLegalPerson.bind(this);
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.personId) {
            this.entity.personId = this.personId;
        }
        if (this.personFullName) {
            this.entity.name = this.personFullName;
        }
        this.cityDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'city');
    }

    afterLoadData(res: any): any {
        this.personEconomicProfileName = res.name;
        this.personEconomicProfileEconomicCode = res.economicCode;
        return super.afterLoadData(res);
    }

    beforeLoadData() {
        this.entity.address = {};
        super.beforeLoadData();
    }

    syncTreeViewSelection(e): void {
        const component = (e && e.component) || (this.treeView && this.treeView.instance);

        if (!component) {
            return;
        }
    }

    treeView_itemSelectionChanged(e): void {
        if (e.itemData.parentId) {
            this.entity.address.cityId = e.itemData.id;
        }
    }

    /*
  وقتی وضعیت شخص حقیقی یا حقوقی تغییر می کند
   */
    changedIsLegalPerson(e) {
        if (e && e.value == false) {
            this.entity.name = this.personFullName;
            this.entity.economicCode = '';
        } else {
            this.entity.name = this.personEconomicProfileName;
            this.entity.economicCode = this.personEconomicProfileEconomicCode;
        }
    }

}
