import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {VocherTypeDetailComponent} from './vocher-type-detail.component';
import {YashilCustomButtonColumn} from "../../../shared/base/classes/yashil-cutom-button-column";
import {custom} from "devextreme/ui/dialog";
import {GenericDataService} from "../../../shared/base/services/generic-data.service";

@Component({
  selector: 'app-vocher-type-list',
  templateUrl: './vocher-type-list.component.html'
})
export class VocherTypeListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  customRowButton: YashilCustomButtonColumn;
  showVochersList : boolean;
  vocherTypeId: number;

  entityName = 'vocherType';
  private baseListUrl = 'vocherType/GetByCustomFilterForList?';


  detailComponent = VocherTypeDetailComponent;
  _companyId: number;
  @Input()
  set companyId(val) {
    if (val !== this._companyId) {
      this._companyId = val;
    }
  }

  get companyId(): number {
    return this._companyId;
  }


  constructor(private genericDataService: GenericDataService) {
    super();
    this.onShowVochersList = this.onShowVochersList.bind(this);
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'شناسه',
      dataField: 'vocherTypeId',
      width: '5%',
    });
    this.columns.push({
      caption: 'عنوان',
      dataField: 'title',
      width: '30%',
    });

    this.columns.push({
      caption: 'نام مرکز',
      dataField: 'companyTitle',
      width: '30%',
    });
    this.createCustomRowButton();
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.companyId) {
        customListUrl += `companyId=${this.companyId}&`;
      }
    }
    let res = false;
    if (this.companyId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as VocherTypeDetailComponent);

    if (this.companyId) {
      comp.companyId = this.companyId;
    }

  }

  //این متد دکمه ی های مربوط به نمایش جزییات
  //و حذف کوپن هایی که به محصول تخصیص نشده را نمایش می دهد
  createCustomRowButton(): void {
    this.customRowButton = {
      width: '150px',
      customButtons: [{
        text: 'مشاهده جزئیات',
        onClick: this.onShowVochersList,
        hint: 'مشاهده جزئیات',
        type: 'default'
      }
        , {
          text: 'حذف کوپن های تخصیص نیافته',
          onClick: this.deleteNotAssignVochers,
          hint: 'حذف کوپن های تخصیص نیافته',
          type: 'default'
        },
      ]
    };
  }
// لیست تمامی ووچرهای مربوط به این ووچرتایپ
  onShowVochersList():void{
    this.showVochersList = true;
  }

//vocherType حذف کوپن های تخصیص نیافته یک
  deleteNotAssignVochers = (): void => {
    this.declareDeleteAllNotAssignedConfirm().show().then((dialogResult) => {

      if (dialogResult.res === 'ok') {
        this.setBusy(true);
        this.genericDataService.postEntityByUrl(`/api/${this.entityName}/DeleteNotAssignedVochers/`, {id: this.vocherTypeId})
          .subscribe((res: any) => {
            this.setBusy(false);
            if (res && res.isSuccess) {
              this.showMessage('تمامی آیتم ها با موفقیت حذف شد');
            } else {
              this.showError(res.message);
            }
          });
      }
      this.createCustomRowButton();;
    });
  }

  //نمایش پیغام قبل از حذف
  private declareDeleteAllNotAssignedConfirm(): any {
    return custom({
      title: 'حذف',
      messageHtml: 'آیا از حذف تمامی کوپن های تخصیص نیافته مطمئن هستید؟',
      buttons: [{
        text: 'تایید',
        onClick: (e) => {
          return {res: 'ok'};
        }
      },
        {
          text: 'انصراف',
          onClick: (e) => {
            return {res: 'cancel'};
          }
        }
      ]
    });
  }
  // دریافت آیدی ردیف انتخاب شده
  onSelectedRowChanged(selectedItem: any) {
    if (selectedItem && selectedItem.vocherTypeId)
      this.vocherTypeId = selectedItem.vocherTypeId;
  }
}
