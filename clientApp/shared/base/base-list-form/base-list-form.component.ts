import {AfterViewInit, Component, Input, OnInit} from '@angular/core';
import {BaseList} from '../classes/base-list';
import {value} from "devexpress-dashboard/model/items/map/metadata/_geo-point-map-item";
import {backColor} from "devexpress-dashboard/model/format-rules/style-settings/metadata/_appearance-settings";


@Component({
    selector: 'app-base-list-form',
    templateUrl: './base-list-form.component.html',
    styleUrls: ['./base-list-form.component.css']
})

export class BaseListFormComponent extends BaseList implements AfterViewInit, OnInit {
    displayButton: boolean = true;

    customizeText(e): string {
        const d = e;
        return '';
    }

    refreshData(): void {
        if (this.isForTree) {
            this.treeList.instance.refresh();
        } else {
            this.listGrid.instance.refresh();
        }
    }

    clearFilter(): void {
        if (this.isForTree) {
            this.treeList.instance.clearFilter();
        } else {
            this.listGrid.instance.clearFilter();
        }
    }

    protected readonly value = value;
    protected readonly backColor = backColor;
}
