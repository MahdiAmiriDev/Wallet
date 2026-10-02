import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';


import {ProductGroupDetailComponent} from './product-group-detail.component';
import {YashilCustomButtonColumn} from "../../../shared/base/classes/yashil-cutom-button-column";


@Component({
  selector: 'app-product-group-list',
  templateUrl: './product-group-list.component.html'
})
export class ProductGroupListComponent extends Selectable implements OnInit {

  @ViewChild('listForm', {static: true}) listForm: BaseList;

  @Output() onUserSelectRow:EventEmitter<any> = new EventEmitter();

  @Input() showAddButton: boolean = true;
  @Input() allowEditInForm: boolean = true;
  @Input() allowDeleteInForm: boolean = true;
  @Input() showChoseRowButton: boolean = false;


  @Input() set isIncludeTax(value:boolean){
    this.includeTax = value;
    this.bindCustomDataSources();
  }

  includeTax:boolean | null = null;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'productGroup';
  detailComponent = ProductGroupDetailComponent;
  customRowButton: YashilCustomButtonColumn;

  _parentId: number;
  @Input()
  set parentId(val) {
    if (val !== this._parentId) {
      this._parentId = val;
    }
  }

  get parentId(): number {
    return this._parentId;
  }

  _categoryGroupId: number;
  @Input()
  set categoryGroupId(val) {
    if (val !== this._categoryGroupId) {
      this._categoryGroupId = val;
    }
  }

  get categoryGroupId(): number {
    return this._categoryGroupId;
  }

  private baseListUrl = 'productGroup/GetByCustomFilterForList?';


  constructor() {
    super();
    this.userSelectRow = this.userSelectRow.bind(this);
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push(
      {
        caption: 'عنوان',
        dataField: 'title'
      },
      {
        caption: 'عنوان دسته بندی',
        dataField: 'categoryGroupTitle'
      },
      {
        caption: 'آیا شامل بیمه می شود',
        dataField: 'isIncludeInsurance'
      },
      {
        caption: 'کد اصلی',
        dataField: 'mainCode'
      },
      {
        caption: 'کد ',
        dataField: 'code'
      },
      {
        caption: 'شامل مالیات می شود',
        dataField: 'isIncludeTax'
      }
    );

    if (this.showChoseRowButton)
      this.bindCustomRowButton();

  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {

      //اگر در حالتی بودیم که دکمه انتخاب ردیف ها فعال باشد
      //باید فیلتری برای حالت های شامل مالیات و بدون مالیات
      //درنظر گرفت
      if(this.showChoseRowButton && (this.includeTax != undefined && (this.includeTax == true || this.includeTax == false))) {
        console.log(this.isIncludeTax);
        customListUrl +=`isIncludeTax=${this.includeTax}&`
      }


      if (this.parentId) {
        customListUrl += `parentId=${this.parentId}&`;
      }

      if (this.categoryGroupId) {
        customListUrl += `categoryGroupId=${this.categoryGroupId}&`;
      }


    }

    let res = false;
    if (this.parentId || this.categoryGroupId || (this.showChoseRowButton && (this.includeTax != undefined && (this.includeTax == true || this.includeTax == false)))) {


      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();

      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (<ProductGroupDetailComponent>componentInstance.instance);

    if (this.parentId) {
      comp.parentId = this.parentId;
    }

    if (this.categoryGroupId) {
      comp.categoryGroupId = this.categoryGroupId;
    }

  }

  //بایند کردن دکمه ی مربوط به ردیف های داده
  bindCustomRowButton() {
    this.customRowButton = {
      width: '120px',
      customButtons: [
        {
          hint: 'انتخاب',
          text: 'انتخاب',
          icon: 'bulletlist',
          type: 'default',
          stylingMode: 'contained',
          onClick: this.userSelectRow,
        }]
    };
  }

  //زمانی که کاربر یک ردیف را انتخاب می کند این متد فراخوانی می شود
  userSelectRow(selectedItem: any): void{
    if(selectedItem.data.id)
      this.onUserSelectRow.emit(selectedItem.data.id);
  }

}

