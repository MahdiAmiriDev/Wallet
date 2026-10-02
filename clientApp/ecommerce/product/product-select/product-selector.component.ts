import {Component, Input, OnInit} from '@angular/core';
import {Selectable} from '../../../../shared/base/classes/selectable';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';

@Component({
    selector: 'app-product-selector',
    templateUrl: './product-selector.component.html'
})
export class ProductSelectorComponent extends Selectable implements OnInit {
    showSelectProduct: boolean;
    @Input()
    companyId: number;
    productDataSource: any;
    @Input()
    selectedItems: any;
    columns: any[] = [];
    entityName = 'product';

    constructor(private genericDataService: GenericDataService) {
        super();
    }

    ngOnInit(): void {
        this.columns.push({
            caption: 'عنوان',
            dataField: 'title',
            width: '70%'
        });
        this.columns.push({
            caption: 'مرکز',
            dataField: 'companyTitle'
        });
        this.columns.push({
            caption: 'انتشار داده شده',
            dataField: 'published',
            with: 50
        });
        this.productDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'product');
    }

    onCloseProductSelectForm() {
        this.showSelectProduct = false;
    }

    selectionChanged(selectedItems: any) {
        this.selectedItems = selectedItems;
        this.showSelectProduct = false;
        this.onSelectionChanged.emit(selectedItems);
    }

    SelectedProductsChanged(selectedItems: any) {
        this.selectedItems = selectedItems;
    }
}
