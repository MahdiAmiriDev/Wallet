import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {forkJoin} from 'rxjs';
import {TranslateService} from '@ngx-translate/core';
import {Selectable} from '../../base/classes/selectable';
import {GenericDataService} from "../../base/services/generic-data.service";
import {Entity} from "../../base/base-data/entity.enum";
import {DxDataGridComponent} from "devextreme-angular/ui/data-grid";


@Component({
  selector: 'app-user-select-modal',
  templateUrl: 'user-select-modal.component.html'
})
export class UserSelectModalComponent extends Selectable implements OnInit {


  columns: any[] = [];
  userDataSource: any;
  selectedItemsId: any [] = [];

  @ViewChild(DxDataGridComponent, {static: false}) dataGrid: DxDataGridComponent;
  @Input()
  isRequired: boolean;
  @Input()
  requiredMessage: string;
  @Input()
  isReadOnly: boolean;
  @Input()
  customListUrl: string;
  @Input()
  lazyLoading = true;
  @Input()
  showOnlyWithNationalCode = false;
  @Input()
  mode: string;
  @Input()
  allowSelectAll: boolean = false;
  @Output()
  onSubmitClick: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  onInitializeComponent: EventEmitter<any> = new EventEmitter<any>();


  constructor(private genericDataService: GenericDataService, private translate: TranslateService) {
    super();
  }

  /*
فراخوانی متد setUserDatasource
دریافت ترجمه ای مربوط به فیلد ها و پوش کردن به column
*/
  ngOnInit(): void {

    this.onInitializeComponent.emit({openOnFieldClick:false , searchEnable:false});

    this.setUserDataSource();

    let result = forkJoin({
      userNameTitle: this.translate.get('um.User.UserName'),
      nameAndLastNameTitle: this.translate.get('NameAndLastName'),
    });

    result.subscribe(({userNameTitle, nameAndLastNameTitle}) => {
      this.columns.push(
        {
          caption: nameAndLastNameTitle,
          dataField: 'fullName',
        },
        {
          caption: userNameTitle,
          dataField: 'userName'
        }
      );
    });
  }



  /* هنگامی که کاربر یوزری را انتخاب می کند این متد فراخوانی می شود */
  selectedUserChanged(e: any): void {
    if (e && e.selectedRowsData && e.selectedRowsData.length > 0) {
      if(e.selectedRowsData.length >= 50)
      {
        this.showError('کاربر گرامی تعداد ایتم انتخابی نمی تواند بیش از 50 مورد باشد');
        this.dataGrid.instance.deselectRows(e.selectedRowsData[50].id);
        return;
      }
    }
    this.selectedItemsId = e.selectedRowsData.map(i => (i.id));
  }



  /* این متد کار ست کردن URl مربوط به api دریافت user ها را انجام می دهد */
  setUserDataSource() {
    this.userDataSource = this.genericDataService.createCustomDataSourceWithAction('id', `${Entity.User}`, 'GetUsersForSendConversationForSelect');
  }



  /*این متد برسی می کند که آیا آیتمی انتخاب شده است یا نه اگر انتخاب نشده بود نمایش پیام مناسب در غیر این صورت ارسال ایتم های انتخاب شده */
  checkIsItemSelected() {
    if (this.selectedItemsId.length == 0) {
      this.showError('کاربر گرامی ایتم انتخابی نمی تواند خالی باشد');
    } else {
      this.onSubmitClick.emit(this.selectedItemsId);
    }
  }

}
