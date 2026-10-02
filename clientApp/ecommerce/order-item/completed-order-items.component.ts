import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';


@Component({
  selector: 'app-completed-order-items',
  templateUrl: './completed-order-items.component.html'
})
export class CompletedOrderItemsComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  detailComponent = CompletedOrderItemsComponent;

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.loadAfterSetFilter = true;

  }

}
