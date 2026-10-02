import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
    selector: 'app-product-specification-option-detail',
    templateUrl: './product-specification-option-detail.component.html'
})
export class ProductSpecificationOptionDetailComponent extends BaseEdit implements OnInit {
    @Input()
    productId: number;
    @Input()
    specificationOption: number;
    @Input()
    attributeType: number;
    productDataSource: any;
    specificationDataSource: any;
    specificationOptionDataSource: any;
    attributeTypes: any;
    accessLevels: any[] = [];
    selectedAttributeType: any;
    firstLoad = true;

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'productSpecificationOption';
        this.selectedSpecificationValueChanged = this.selectedSpecificationValueChanged.bind(this);
        this.selectedAttributeTypeChanged = this.selectedAttributeTypeChanged.bind(this);
    }

    ngOnInit(): void {
        if (!this.selectedEntityId) {
            this.firstLoad = false;
        }
        this.selectedAttributeType = {};
        super.ngOnInit();
        if (this.productId) {
            this.entity.productId = this.productId;
        }
        if (this.specificationOption) {
            this.entity.specificationOption = this.specificationOption;
        }
        if (this.attributeType) {
            this.entity.attributeType = this.attributeType;
        }
        this.specificationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'specification');
        this.productDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'product');

        this._genericDataService.getCommonBaseDataForSelect('AttributeType').subscribe(res => this.attributeTypes = res);
        this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
    }

    selectedSpecificationValueChanged(e) {
        if (e && e.value) {
            if (!this.firstLoad) {
                this.entity.specificationOptionId = null;
                this.bindCustomDataSource();
            } else {
                this.firstLoad = false;
            }
        }
    }

    selectedAttributeTypeChanged(e) {
        if (e && e.selectedItem) {
            this.selectedAttributeType = e.selectedItem;
            this.entity.attributeType = e.selectedItem.id;
            if (this.selectedAttributeType.code !== '3') {
                this.entity.allowFiltering = false;
            } else {
                this.entity.customValue = null;
            }
        }
    }

    bindCustomDataSource() {
        if (this.entity.specificationId) {
            const customListUrl = `GetByCustomFilterForList?specificationId=${this.entity.specificationId}`;
            if (customListUrl !== '') {
                this.specificationOptionDataSource = this._genericDataService.createCustomDataSourceWithAction('id', 'specificationOption', customListUrl);
            }
        }
    }

    afterLoadData(res: any): any {
        this.bindCustomDataSource();
        return super.afterLoadData(res);
    }

}
