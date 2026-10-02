import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Selectable} from "../../../../shared/base/classes/selectable";
import {BaseList} from "../../../../shared/base/classes/base-list";
import {YashilCustomButtonColumn} from "../../../../shared/base/classes/yashil-cutom-button-column";
import {CachedDataService} from "../../../../shared/services/cached-data.service";
import {GenericDataService} from "../../../../shared/base/services/generic-data.service";
import {UserDataService} from '../../../../_services/user-data.service';


@Component({
  selector: 'app-product-search',
  templateUrl: './product-search.component.html',
  styleUrls: ['./product-search.component.css']
})
export class ProductSearchComponent extends Selectable implements OnInit {


  showPopUp: boolean = false;
  @Input()
  productId: number;
  @Input()
  companyId: number;
  @Input()
  showCustomButton = true;
  @Input()
  categoryId: number;
  @Output()
  selectedProductChanged: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild('listForm', {static: false}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  columns: any[] = [];
  customRowButtons: YashilCustomButtonColumn;
  entityName = 'product';
  private baseListUrl = 'product/GetByCustomFilterForList?';
  private additionalUserProp: any;

  constructor(private genericDataService: GenericDataService, private cachedDataService: CachedDataService, private userDataService: UserDataService) {
    super();
    this.selectProduct = this.selectProduct.bind(this);
  }

  ngOnInit(): void {
    this.bindColumns();
    this.bindCustomRowButtons();
    this.additionalUserProp = this.userDataService.getAdditionalUserProp();
    if (this.additionalUserProp && this.additionalUserProp.companyId) {
      this.companyId = this.additionalUserProp.companyId;
    }
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  public bindCustomDataSources() {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.companyId) {
        customListUrl += `companyId=${this.companyId}&`;
      }
      if (this.categoryId) {
        customListUrl += `categoryId=${this.categoryId}&`;
      }
    }
    let res = false;
    if (this.companyId || this.categoryId) {
      if (this.listForm) {
        this.listForm.customListUrl = customListUrl;
        this.listForm.refreshList();
      }
      res = true;
    }
    return res;
  }

  bindCustomRowButtons(): void {
    this.customRowButtons = {
      width: '150px',
      customButtons: [
        {
          hint: 'انتخاب ',
          text: 'انتخاب ',
          icon: '../../assets/images/select-hand.svg',
          width: '120px',
          type: 'default',
          onClick: this.selectProduct
        }
      ]
    };
  }

  bindColumns() {
    this.columns.push({
      caption: 'کد',
      dataField: 'code'
    }, {
      caption: 'عنوان',
      dataField: 'title',
      width: '40%'
    }, {
      caption: 'مرکز',
      dataField: 'companyTitle'
    }, {
      caption: 'محصول گروهی',
      dataField: 'isGroupProduct',
      with: 50
    }, {
      caption: 'انتشار داده شده',
      dataField: 'published',
      with: 50
    });
  }


  selectProduct(item: any): void {
    if (item && item.data && item.data.id) {
      this.productId = item.data.id;
      this.selectedRowChanged.emit(item.data.id);
      this.showPopUp = false;
    }
  }


  closePopUpBtn() {
    this.showPopUp = false;
  }

  showSelectProductList() {
    this.showPopUp = true;
    this.bindCustomDataSources();
  }

  setProductData(item: any) {
    if (item) {
      this.selectedProductChanged.emit(item);
    }
  }
}

