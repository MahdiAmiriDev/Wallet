import {Component, OnInit} from '@angular/core';
import {Selectable} from '../../../shared/base/classes/selectable';

@Component({
  selector: 'app-modified-order-item-to-itself-list',
  templateUrl: './modified-order-item-to-itself.component.html'
})
export class ModifiedOrderItemToItselfListComponent extends Selectable implements OnInit {
  orderItemListHeight: number;
  constructor() {
    super();
    this.orderItemListHeight = window.innerHeight - 210;
  }

  ngOnInit(): void {

  }
}

