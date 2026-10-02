import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {GenericDataService} from "../../../../shared/base/services/generic-data.service";

@Component({
  selector: 'app-product-group-search',
  templateUrl: './product-group-search.component.html'
})

export class ProductGroupSearchComponent implements OnInit {

  @Input() showIncludeTaxFilter: boolean;
  @Input() isIncludeTax: boolean;

  @Output() onProductGroupSelected: EventEmitter<any> = new EventEmitter();

  showPopup: boolean;
  radioButtonItems: any[];

  constructor(private genericDataService: GenericDataService) {
  }

  ngOnInit() {
    this.radioButtonItems = [
      {
        text: 'بامالیات',
        value: true
      },
      {
        text: 'بدون مالیات',
        value: false
      }
    ];
  }


  //زمانی کاربر گروه محصول را انتخاب می کند این متد فراخوانی می شود
  selectedProductGroupItemChanged(selectedItemId: any) {
    if(selectedItemId){
      this.onProductGroupSelected.emit(selectedItemId);
      this.showPopup = false;
    }
  }



  //زمانی که کاربر گزینه شامل مالیات را تغییر می دهد این متد فراخوانی می شود

  isIncludeTaxChanged(item: any) {
    this.isIncludeTax = item.value;
  }

}
