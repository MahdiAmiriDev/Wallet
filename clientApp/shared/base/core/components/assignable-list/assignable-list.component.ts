import {Component, EventEmitter, Inject, Input, OnInit, Output, ViewChild} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {GenericDataService} from '../../../services/generic-data.service';
import {CustomDevDataSource} from '../../../classes/custom-dev-data-source';
import {DxDataGridComponent} from 'devextreme-angular/ui/data-grid';
import notify from 'devextreme/ui/notify';
import {custom} from 'devextreme/ui/dialog';

@Component({
  selector: 'app-assignable-list',
  templateUrl: 'assignable-list.component.html'
})
export class AssignableListComponent implements OnInit {
  @Input()
  notAssignedMethodExtraQueryParam: string;
  @Input()
  assignedMethodExtraQueryParam: string;
  @Input()
  allowEditAssignedItems = false;
  gridHeight = 400;
  @ViewChild('notSelectedItemsGrid') notSelectedItemsDataGrid: DxDataGridComponent;
  @ViewChild('selectedItemsGrid') selectedItemsDataGrid: DxDataGridComponent;
  itemDataSource: any;
  @Input()
  selectedGroupItemId: number;
  @Input()
  selectedGroupItemTitle: string;
  @Input()
  customInlineMethodName: string = null;
  /**
   * @param groupEntityName  The Name Of Entity For Group By.
   */
  @Input()
  groupEntityName: string;
  @Input()
  customActionGroupEntityForSelect: string;
  /**
   * @param assignableListName The Name Of Entity For Grouping.
   */
  @Input()
  assignableListName: string;
  /**
   * @param assignableListLabel  Label For Assignable Entity.
   */
  @Input()
  assignableListLabel: string;
  /**
   * @param groupEntityLabel  Label For Group Entity.
   */
  @Input()
  groupEntityLabel: string;
  @Input()
  columns: any[] = [];
  @Input()
  assignedColumns: any[] = [];
  @Input()
  getAssignedMethodName: string;
  @Input()
  getNotAssignedMethodName: string;
  @Input()
  showGroupItemComponent = true;
  assignedItemDataSource: any = {};
  notAssignedItemDataSource: any = {};
  httpClient: HttpClient;
  @Input()
  assignAffected: boolean;
  @Output()
  assignAffectedChange = new EventEmitter<boolean>();
  @Output()
  closeFormClick = new EventEmitter<boolean>();

  @Input()
  enableAfterAssigning: boolean = false;
  @Output()
  afterAssigning: EventEmitter<any> = new EventEmitter<any>();

  //داده های اضافی که زمانی که قرار است api مربوط به assign فراخوانی شود به مدل که
  //قرار است پست شود اضافه می شود
  @Input()
  assignItemsAdditionalPostData: any = {};

  constructor(@Inject(HttpClient) httpClient: HttpClient, private genericDataService: GenericDataService) {
    this.httpClient = httpClient;
  }

  ngOnInit(): void {
    const defaultColumn = {
      caption: 'عنوان',
      dataField: 'title'
    };
    if (this.columns.length === 0) {
      this.columns.push(defaultColumn);
    }
    if (this.assignedColumns.length === 0) {
      this.assignedColumns.push(defaultColumn);
    }
    this.gridHeight = window.innerHeight - 240;
    if (!this.customActionGroupEntityForSelect) {
      this.itemDataSource = this.genericDataService.createCustomDataSourceForSelect('id', this.groupEntityName);
    } else {
      this.itemDataSource = this.genericDataService.createCustomDataSourceWithAction('id', this.groupEntityName, this.customActionGroupEntityForSelect);

    }
    if (this.selectedGroupItemId) {
      this.bindLists();
    }
  }

  /**
   * Generate Select List Url And Get Custom Dasaources For Selected And Not Selected Items
   */
  bindLists(): void {
    let getAssignedMethod = `${this.assignableListName}/Get${this.assignableListName}sAssignedTo${this.groupEntityName}Async?id=${this.selectedGroupItemId}`;

    let getNotAssignedMethod = `${this.assignableListName}/Get${this.assignableListName}sNotAssignedTo${this.groupEntityName}Async?id=${this.selectedGroupItemId}`;
    if (this.notAssignedMethodExtraQueryParam) {
      getNotAssignedMethod += `&${this.notAssignedMethodExtraQueryParam}`;
    }
    if (this.assignedMethodExtraQueryParam) {
      getAssignedMethod += `&${this.assignedMethodExtraQueryParam}`;
    }
    this.assignedItemDataSource = new CustomDevDataSource(this.httpClient)
      .getCustomDataSource(this.assignableListName, [], getAssignedMethod, this.customInlineMethodName);
    this.notAssignedItemDataSource = new CustomDevDataSource(this.httpClient).getCustomDataSourceWithCustomListUrl(getNotAssignedMethod);
  }

  /**
   * Raise When Selected Group Changed
   * @param data
   */
  selectedGroupItemChanged(data): void {
    this.selectedGroupItemId = data.value;
    this.selectedGroupItemTitle = data.text;
    this.bindLists();
  }

  /**
   * Assign Items To Selected Group
   */
  assignItemsToGroup(): void {
    const selectedRowKeys = this.notSelectedItemsDataGrid.instance.getSelectedRowKeys();
    this.assignItems(selectedRowKeys, true);
  }

  /**
   * Not Assign Items To Selected Group
   */
  removeAssignItemsFromGroup(): void {
    const selectedRowKeys = this.selectedItemsDataGrid.instance.getSelectedRowKeys();
    this.assignItems(selectedRowKeys, false);
  }

  /**
   * Assign Or Not Assign Items To Selected Group
   * @param selectedRowKeys Selected Items For Assign Or Not Assign
   * @param assign True For Assign And False For Not Assign
   */
  assignItems(selectedRowKeys: any, assign): void {
    if (selectedRowKeys && selectedRowKeys.length > 0) {
      this.confirmBeforeAction(assign ? 'تخصیص آیتم های انتخاب شده' : 'لغو تخصیص آیتم های انتخاب شده', 'تایید')
        .show().then((dialogResult) => {
        if (dialogResult.res === 'ok') {
          let postData: any;
          if (this.assignItemsAdditionalPostData) {
            postData = {
              selectedItems: selectedRowKeys,
              groupId: this.selectedGroupItemId,
              assign: assign,
              ...this.assignItemsAdditionalPostData
            };
          } else {
            postData = {
              selectedItems: selectedRowKeys,
              groupId: this.selectedGroupItemId,
              assign
            }
          }
          this.genericDataService.postEntityByUrl(`api/${this.assignableListName}/AssignSelectedItemsTo${this.groupEntityName}`, postData).subscribe(res => {
              if (this.enableAfterAssigning && assign) {
                this.afterAssigning.emit({
                  selectedItems: selectedRowKeys,
                  groupId: this.selectedGroupItemId
                });
              } else {
                let isSuccess = false;
                let message = 'بروز خطا در عملیات ذخیره سازی';
                // اگر مقدار بازگشتی resultStatus باشد
                if (typeof res !== 'boolean') {
                  if (res) {
                    if (res.isSuccess) {

                      isSuccess = true;
                      message = 'اطلاعات با موفقیت ذخیره شد';
                    } else {
                      if (res.message) {
                        message = res.message;
                      }
                    }
                  }
                  // اگر مقدار بازگشتی boolean باشد
                } else {
                  if (res) {


                    isSuccess = true;
                    message = 'اطلاعات با موفقیت ذخیره شد';


                  }
                }
                if (isSuccess) {
                  this.assignAffected = true;
                  this.assignAffectedChange.emit(this.assignAffected);
                  this.showMessage(message);
                } else {
                  this.showError(message);
                }
              }
              this.bindLists();
            }
          );
        }
      });
    } else {
      notify({
        message: ` هیچ ${this.assignableListLabel}ی انتخاب نشده است! `,
        position: {
          my: 'center top',
          at: 'center top'
        }
      }, 'warning', 3000);
    }
  }

  confirmBeforeAction(title: string, confirmText: string): any {
    return custom({
      title,
      messageHtml: `آیا مطمئن به ${title} می باشید؟ `,
      buttons: [{
        text: confirmText,
        onClick: (e) => {

          return {res: 'ok'};
        }
      }, {
        text: 'انصراف',
        onClick: (e) => {
          return {res: 'cancel'};
        }
      }]
    });
  }


  public showError(message: string): void {
    notify({
      message,
      position: {
        my: 'center top',
        at: 'center top'
      },
      rtlEnabled: true
    }, 'error', 5000);
  }

  public showMessage(message: string): void {
    notify({
      message,
      position: {
        my: 'center top',
        at: 'center top'
      },
      rtlEnabled: true
    }, 'success', 3000);
  }

  public onCloseFormClick(): void {
    this.closeFormClick.next(this.assignAffected);
  }
}
