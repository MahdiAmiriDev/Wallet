import {
  AfterViewInit,
  ComponentFactoryResolver,
  ComponentRef,
  ContentChild,
  Directive,
  EventEmitter,
  Inject,
  Input,
  OnInit,
  Output,
  TemplateRef,
  Type,
  ViewChild
} from '@angular/core';
import {GenericDataService} from '../services/generic-data.service';
import {custom} from 'devextreme/ui/dialog';
import {DetailComponentDirective} from '../base-list-form/detail-component.directive';
import {DxPopupComponent} from 'devextreme-angular/ui/popup';
import {DxDataGridComponent} from 'devextreme-angular/ui/data-grid';
import {DxTreeListComponent} from 'devextreme-angular/ui/tree-list';
import {BaseEditFormComponent} from '../base-edit-form/base-edit-form.component';
import {BaseEdit} from './base-edit';
import {FormEditType} from './edit-type';
import {DialogService} from '../../../_services/dialog.service';
import {Router} from '@angular/router';
import {CustomDevDataSource} from './custom-dev-data-source';
import {HttpClient} from '@angular/common/http';
import {CachedDataService} from '../../services/cached-data.service';
import notify from 'devextreme/ui/notify';
import DataSource from 'devextreme/data/data_source';
import {YashilCustomButtonColumn} from './yashil-cutom-button-column';
import {Workbook} from 'exceljs';
import {exportDataGrid} from 'devextreme/excel_exporter';
import saveAs from 'file-saver';
import {Observable, Subscription} from 'rxjs';
import {Editable} from './editable';
import {InlineEditModeEnum} from './inline-edit-mode-enum';
import {MenuService} from '../../services/menu-service';
import {UserDataService} from '../../../_services/user-data.service';

@Directive()
// tslint:disable-next-line:directive-class-suffix
export class BaseList extends CustomDevDataSource implements OnInit, AfterViewInit {
  private subscription: Subscription;

  resourceAllowEdit: boolean;
  resourceAllowAdd: boolean;
  resourceAllowDelete: boolean;
  resourceAllowArchive: boolean;
  enableRoleResourceActionPopUp: boolean = false;
  enableResourceAppActionPopUp: boolean = false;
  @Input()
  showRoleResourceActionPopUp = true;
  @Input()
  showResourceAppActionPopUp = true;
  @Input()
  selectByButton = false;
  @Input()
  showRowLines = false;
  @Input()
  showExportToExcelButton: boolean;
  @Input()
  showRefreshButton = true;
  @Input()
  showClearFilterButton = true;
  /*
  * this use for custom Mode when u need bind data to custom component without dataSource
  * */
  // @Input()
  // showExportToExcelButton: boolean;
  @Input()
  showCheckBoxesMode: string;
  @Input()
  selectionRecursive: boolean;
  @Input()
  selectAllMode: string;
  @Input()
  selectedMode = 'single';
  @Input()
  customButtons: any[];
  @Input()
  fixHeight = true;
  selectedItemId: number;
  allowEditSelectedRow: boolean;
  allowDeleteSelectedRow: boolean;
  allowArchiveSelectedRow: boolean;
  detailComponentRef: any;
  @Input()
  showFilterButton = true;
  @Input()
  showListHeader = true;
  @Input()
  title: string;
  @Input()
  showCustomCheckBox: boolean = false;
  @Input()
  customButtonCheckBoxTitle: string;
  @Output()
  customCheckBoxEvent = new EventEmitter<boolean>();
  customCheckboxIsChecked = false;
  @Input()
  allowInsertInForm = true;
  @Input()
  showAddButton = true;
  @Input()
  allowEditInForm = true;
  @Input()
  allowArchiveInForm = false;
  @Input()
  allowPaging = true;
  @Input()
  columnAutoWidth = false;
  @Input()
  lazyLoading = true;
  @Input()
  allowColumnResizing: boolean;
  @Input()
  wordWrapEnabled = true;
  @Input()
  loadAfterSetFilter = false;
  @Input()
  allowGrouping = false;
  @Input()
  pagerDisplayMode = 'full';
  @Input()
  columnGrouping = 'id';
  @Input()
  summaryTypeGrouping = 'count';
  @Input()
  displayFormatGrouping = 'تعداد: {0}';
  @Input()
  virtualPaging = false;
  @Input()
  columnHidingEnabled = false;
  @Output()
  selectButtonClicked: EventEmitter<any> = new EventEmitter<any>();

  @Output()
  customButtonClicked: EventEmitter<any> = new EventEmitter<any>();
  @Input()
  offsetHeight: number;
  parentIdExpr: 'parentId';
  @Input()
  isForSelectReadOnly = false;
  @ViewChild(DetailComponentDirective) detailComponentContainer: DetailComponentDirective;
  @Input()
  detailComponent: Type<any>;
  @Input()
  detailComponentEntityName: string;
  loadData: boolean;
  @Input()
  additionalData = {};
  @Output()
  afterInitialDetailComponent: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  beforeAddClicked: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output()
  public selectedRowChanged: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public listRowPrepared: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  recordInitialized: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  rowDeleted: EventEmitter<any> = new EventEmitter<any>();
  @Input()
  allowFilter = false;
  @Input()
  allowLoadData = true;
  @Input()
  dataStructureIsPlain = false;
  @Input()
  rowAlternationEnabled = true;
  entities: any = {};
  genericDataService: GenericDataService;
  dataEntityName: string;
  remoteOperations: any = {};
  @Input()
  customListUrl: string;
  @Input()
  customDeleteActionName: string;
  @Input()
  customArchiveActionName: string;
  @Input()
  customRestoreActionName: string;
  customFilters: any[] = [];
  @Input()
  isForTree: boolean;
// The Root Value In Tree Mode
  // edited
  @Output()
    // edited
  closeActionEmitter = new EventEmitter<boolean>();
  @ViewChild(DxPopupComponent, {static: true}) popupComponent: DxPopupComponent;
  @ViewChild(DxDataGridComponent) listGrid: DxDataGridComponent;
  @ViewChild(DxTreeListComponent) treeList: DxTreeListComponent;
  @ViewChild(BaseEditFormComponent) baseEditFormComponent: BaseEditFormComponent;
  @ContentChild(TemplateRef)
  template: TemplateRef<BaseEdit>;
  @Input()
  gridHeight: number;
  @Input()
  editType: FormEditType = FormEditType.CustomModal;
  @Input()
  inlineEditMode: InlineEditModeEnum = InlineEditModeEnum.Row;
  @Input()
  columns: any[] = [];
  @Input()
  customParameterForPostMethod: any;
  @Input()
  getCustomDataSourceByPostMethod: boolean = false;
  /*
  list of buttons that push in each row
   */
  @Input()
  customRowButtons: YashilCustomButtonColumn;
  isNew = false;
  selectedRowId: number;
  @Input()
  resourceName: string;
  dialogTitle: string;
  @Input()
  allowAdd = true;
  showInlineSaveBtn: boolean = false;
  @Input()
  customInlineUpdateMethodName: string = null;
  @Output()
  public onRowClick: EventEmitter<any> = new EventEmitter<any>();
  @Input() customDeleteMessage: string = "آیا از حذف آیتم انتخاب شده مطمئن هستید؟";

  //زمانی که فعال باشد فرآیند حذف یک ابجکت به صورت غیر هم زمان انجام می شود
  //و تکمیل عملیات حذف وابسته به beforeDeleteAsync می شود
  @Input()
  asyncDeleteMode: boolean = false;
  //در صورتی که asyncDeleteMode فعال باشد قبل از عملیات حذف فراخوانی می شود
  @Output()
  public beforeDeleteAsync = new EventEmitter<{
    data: any,
    process: (canDelete: boolean) => void
  }>();


  constructor(private componentFactoryResolver: ComponentFactoryResolver,
              private dialogService: DialogService, genericDataService: GenericDataService,
              private cachedDataService: CachedDataService, @Inject(HttpClient) httpClient: HttpClient,
              public router: Router,
              public menuService: MenuService, private userDataService: UserDataService
  ) {
    super(httpClient);
    this.genericDataService = genericDataService;
    this.bindDataSource = this.bindDataSource.bind(this);
    this.allowEditRecord = this.allowEditRecord.bind(this);
    this.allowDeleteRecord = this.allowDeleteRecord.bind(this);
    this.allowArchiveRecord = this.allowArchiveRecord.bind(this);
    this.selectedCustomButtonChanged = this.selectedCustomButtonChanged.bind(this);
  }

  // tslint:disable-next-line:variable-name
  _rootValue: any;

  get rootValue(): any {
    return this._rootValue;
  }

  // @Input()
  // public set filters(val: any[]) {
  //     if (val) {
  //         this.customFilters = val;
  //         this.bindDataSource();
  //     }
  // }

  @Input()
  set rootValue(val) {
    this._rootValue = val;
  }

  // tslint:disable-next-line:variable-name
  _gridDataSource: any = {};

  get gridDataSource(): DataSource {
    return this._gridDataSource;
  }

  set gridDataSource(val) {
    if (val !== this._gridDataSource) {
      this._gridDataSource = val;
    }
  }

  public get entityName(): string {
    return this.dataEntityName;
  }

  @Input()
  public set entityName(value: string) {
    this.dataEntityName = value;
  }

  // tslint:disable-next-line:variable-name
  private _showEditForm: boolean;

  public get showEditForm(): boolean {
    return this._showEditForm;
  }

  @Input()
  public set showEditForm(val: boolean) {
    this._showEditForm = val;
  }

  public get filters(): any[] {
    return this.customFilters;
  }

  public get isCustomEditForm(): boolean {
    return this.editType === FormEditType.CustomModal;
  }

  ngAfterViewInit(): void {
    this.setRemoteOperations();
    if (this.lazyLoading) {
      this.remoteOperations = {paging: true, sorting: true, filtering: false};
    }
    this.closeActionEmitter.subscribe(res => {
      this.showEditForm = false;
    });
  }

  ngOnInit(): void {
    if (!this.rootValue) {
      this.rootValue = null;
    }
    this.setAccess();
    /* this.declareDeleteConfirm();*/
    this.setListTitle();
    this.setEditColumn();
    this.setCustomRowButtons();
    let heightD = 150;
    if (this.offsetHeight) {
      heightD = this.offsetHeight;
    }
    if (this.allowGrouping && this.allowGrouping === true && heightD < 170) {
      heightD = 170;
    }
    if (!this.gridHeight && this.fixHeight) {
      this.gridHeight = window.innerHeight - heightD;
    }
    /*
  * If Is For Tree bind To Data Source After Root Value Set
  * */
    if (!this.loadAfterSetFilter) {
      this.bindDataSource();
    }
  }

  selectedSelectButtonChanged(data: any): void {
    this.selectButtonClicked.emit(data);
  }

  selectedCustomButtonChanged(e: any): void {
    this.customButtonClicked.emit(e);
  }

  toggleFilter(): void {
    this.allowFilter = !this.allowFilter;
    //  this.remoteOperations = {paging: true, sorting: true, filtering: this.allowFilter, groupPaging: true};
    this.setRemoteOperations();
  }

  setRemoteOperations(): void {
    if (this.lazyLoading) {
      this.remoteOperations.paging = true;
    } else {
      this.remoteOperations.paging = false;
    }
    this.remoteOperations.sorting = true;
    this.remoteOperations.filtering = this.allowFilter;
    if (this.allowGrouping) {
      this.remoteOperations.groupPaging = true;
    } else {
      this.remoteOperations.groupPaging = false;
    }
  }

  loadPlainData(): void {
    if (this.customListUrl) {
      this.genericDataService.getCustomEntitiesByUrl(`/api/${this.customListUrl}`).subscribe((res: any) => {
        this.gridDataSource = res;
      });
    } else {
      this.genericDataService.getEntities(this.entityName).subscribe((res: any) => {
        this.gridDataSource = res;
      });
    }
  }

  public refreshList(): void {
    this.bindDataSource();
  }

  public afterSave(): void {
    this.bindDataSource();
  }

  public bindDataSource(): void {
    if (!this.allowLoadData) {
      return;
    }
    const pageFilters = this.customFilters;
    if (this.dataStructureIsPlain) {
      this.loadPlainData();
    } else {
      this.setBusy(true);
      this.gridDataSource = this.getCustomDataSource(this.entityName, pageFilters, this.customListUrl, this.customInlineUpdateMethodName, false,
        this.customParameterForPostMethod, this.getCustomDataSourceByPostMethod);
    }
  }

  delete(data: any): void {
    this.declareDeleteConfirm().show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        let id: number;
        if (data && data.id) {
          id = data.id;
        } else if (typeof data == 'number') {
          id = data;
        } else {
          throw 'id not found in selected row for delete entity';
        }
        this.setBusy(true);
        if (this.asyncDeleteMode) {
          this.beforeDeleteAsync.emit({
            data: data,
            process: (canDelete: boolean) => {
              if (canDelete) {
                this.performDelete(id);
              } else {
                this.setBusy(false);
                this.bindDataSource();
              }
            }
          });
        } else {
          this.performDelete(id);
        }
      }
    });
    // prompt('آیا مایل به حذف رکورد انتخاب شده می یاشید؟');
  }

  private performDelete(id: number): void {
    this.genericDataService.deleteEntity(this.entityName, id, this.customDeleteActionName).subscribe((res: any) => {
      if (res && res.isSuccess) {
        notify({
          message: res.message,
          position: {
            my: 'center top',
            at: 'center top'
          }
        }, 'success', 3000);
        this.rowDeleted.emit(id);
        /*
        با توجه به اینکه دکمه دیلیت که
         زده میشود هم onSelectionChange نیز emit میشود
        هنگامی که رکورد مورد نظر نیز دیلیت میشود باید به بقیه نیز اعلام کنیم که رکوردی انتخاب نشده است زیرا
         رکورد انتخاب شده نظر حذف گردیده است
         */
        this.selectedRowChanged.emit(null);
      } else {
        notify({
          message: res.message,
          position: {
            my: 'center top',
            at: 'center top'
          }
        }, 'error', 3000);
      }
      this.setBusy(false);
      this.bindDataSource();
    });
  }

  archive(data: any): void {
    this.declareArchiveConfirm().show().then((dialogResult) => {

      if (dialogResult.res === 'ok') {
        let id: number;
        if (data && data.id) {
          id = data.id;
        } else if (typeof data == 'number') {
          id = data;
        } else {
          throw 'id not found in selected row for archive entity';
        }
        this.setBusy(true);
        this.genericDataService.postEntityByUrl(`/api/${this.entityName}/${this.customArchiveActionName}/`, {id: id})
          .subscribe((res: any) => {
            if (res && res.isSuccess) {
              this.showMessage('آیتم با موفقیت آرشیو شد');
              this.refreshList();
            } else {
              this.showError(res.message);
            }
          });
      }
    });
  }

  restore(data: any) {
    this.declareRestoreConfirm().show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        let id: number;
        if (data && data.id) {
          id = data.id;
        } else if (typeof data == 'number') {
          id = data;
        } else {
          throw 'id not found in selected row for delete entity';
        }
        this.setBusy(true);
        this.genericDataService.postEntityByUrl(`/api/${this.entityName}/${this.customRestoreActionName}/`, {id: id})
          .subscribe((res: any) => {
            this.setBusy(false);
            if (res && res.isSuccess) {
              this.showMessage('آیتم با موفقیت از آرشیو خارج شد');
              this.refreshList()
            } else {
              this.showError(res.message);
            }
          });
      }
    })
  }


  onSelectionChanged(e): void {
    if (e && e.selectedRowsData && e.selectedRowsData.length > 0) {
      this.setSelectedRow(e.selectedRowsData[0][this.rowKey], e.selectedRowsData[0]);
      this.selectedRowKeys = e.selectedRowKeys;
      if (this.selectedMode !== 'single') {
        // this.selectedRowChanged.emit(e.selectedRowKeys);
        const selectedData = e.component.getSelectedRowsData(this.selectedMode);
        this.selectedRowChanged.emit(this.getEmployeeNames(selectedData));

      } else {
        this.selectedRowChanged.emit(e.selectedRowsData[0]);
      }
    } else {
      this.selectedRowChanged.emit(null);
    }
  }

  getEmployeeNames = (data: any): any[] => (data.length > 0
    ? data.map((x) => x.id)
    : 0);

  onListRowPrepared(e: any): void {
    if (e.rowType === 'data' && e.data.deleted === true) {
      e.rowElement.style.color = ('#4882e1');
    }
    this.listRowPrepared.emit(e);
  }

  dialogClosed(): void {
    if (this.detailComponentRef && this.detailComponentRef.instance instanceof BaseEdit) {
      (this.detailComponentRef.instance as BaseEdit).onCloseFormClick();
    } else if (this.detailComponentRef && this.detailComponentRef.instance instanceof Editable) {
      const mainBaseEdit = (this.detailComponentRef.instance as Editable).mainBaseEdit;
      if (mainBaseEdit) {
        mainBaseEdit.onCloseFormClick();
      }
    }
  }

  clearList(): void {
    if (this.listGrid) {
      this.listGrid.dataSource = null;
    }
  }

  addEntity(): void {
    // t his.loadData = !this.loadData;
    if (!this.isCustomEditForm) {
      this.listGrid.instance.addRow();
      return;
    }
    this.beforeAddClicked.emit(true);
    if (this.allowAdd) {
      this.isNew = true;
      this.selectedItemId = null;
      this.showEditForm = true;
      this.dialogService.dialogOpend.emit(false);

      this.setDialogContent(true, null, null);
    }
  }

  setDialogContent(isNew: boolean, selectedEntityId, selectedItem: any, title = ''): ComponentRef<any> {
    if (title !== '') {
      this.dialogTitle = `ویرایش -> ${title}`;
    }

    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.detailComponent);
    const viewContainerRef = this.detailComponentContainer.viewContainerRef;
    viewContainerRef.clear();
    this.detailComponentRef = viewContainerRef.createComponent(componentFactory);
    if (this.detailComponentRef.instance instanceof Editable) {
      const editComponent = (this.detailComponentRef.instance as Editable);
      editComponent.selectedEntityId = selectedEntityId;
      editComponent.isNew = isNew;
      editComponent.closeFormClick = this.closeActionEmitter;
      editComponent.additionalData = this.additionalData;
      editComponent.showCloseButton = true;
      if (this.detailComponentEntityName) {
        editComponent.entityName = this.detailComponentEntityName;
      }
      this.subscription = editComponent.closeFormClick.subscribe((x: boolean) => {
        if (x) {
          this.refreshList();
          this.detailComponentRef.destroy();
        }
      });
    }
    this.afterSetDialogContent(this.detailComponentRef.instance, selectedItem, isNew);

    this.detailComponentRef.onDestroy(x => {
      if (this.subscription) {
        this.subscription.unsubscribe();
      }
    });

    return this.detailComponentRef;
  }

  editEntity(id): void {
    this.loadData = !this.loadData;
    if (!this.isCustomEditForm) {
      const selectedRowIndex = this.listGrid.instance.getRowIndexByKey(this.listGrid.instance.getSelectedRowKeys()[0]);
      this.showInlineSaveBtn = true;
      this.listGrid.instance.editRow(selectedRowIndex);
      return;
    }
    let selectedItem = null;
    const selectedRowTitle = '';
    if (this.listGrid) {
      const keys = [id];
      this.listGrid.instance.deselectAll();
      this.listGrid.instance.selectRows(keys, true);

      this.listGrid.instance.byKey(id).then((selectedRowData) => {
        if (selectedRowData) {
          selectedItem = selectedRowData;
          this.setDialogContentWithParameter(id, selectedItem, selectedRowTitle);
        } else {
          this.showError('بروز خطا در نمایش فرم ویرایش ، لطفا با مدیر سیستم تماس بگیرید');
        }
      });
    } else {
      this.setDialogContentWithParameter(id, selectedItem, selectedRowTitle);
    }
  }

  setDialogContentWithParameter(id, selectedItem, selectedRowTitle) {
    this.selectedRowId = id;
    this.isNew = false;
    this.selectedItemId = this.selectedRowId;
    this.showEditForm = true;
    this.dialogService.dialogOpend.emit(false);
    this.setDialogContent(false, this.selectedItemId, selectedItem, selectedRowTitle);
  }

  afterLoadData(result): void {
    this.setBusy(false);
  }

  allowEditRecord(data: any): boolean {
    if (data.hasOwnProperty('allowEdit')) {
      if (data.allowEdit === true) {
        return true;
      }
      return false;
    }
    return true;
  }

  allowDeleteRecord(data: any): boolean {
    if (!this.allowDeleteInForm) {
      return false;
    }
    if (data.hasOwnProperty('allowDelete')) {
      if (data.allowDelete === true) {
        return true;
      }
      return false;
    }
    return true;
  }

  allowArchiveRecord(data: any): boolean {
    if (!this.allowArchiveInForm) {
      return false;
    }
    if (data.hasOwnProperty('allowArchive')) {
      if (data.allowArchive === true) {
        return true;
      }
      return false;
    }
    return true;
  }

  protected setSelectedRow(selectedRowId, selectedRow): void {
    this.selectedItemId = selectedRowId;
    this.allowEditSelectedRow = this.allowEditRecord(selectedRow);
    this.allowDeleteSelectedRow = this.allowDeleteRecord(selectedRow);
    this.allowArchiveSelectedRow = this.allowArchiveRecord(selectedRow);
  }

  private declareDeleteConfirm(): any {
    return custom({
      title: 'حذف',
      messageHtml: this.customDeleteMessage,
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

  private declareArchiveConfirm(): any {
    return custom({
      title: 'آرشیو',
      messageHtml: 'آیا از آرشیو آیتم انتخاب شده مطمئن هستید؟',
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

  private declareRestoreConfirm(): any {
    return custom({
      title: 'آرشیو',
      messageHtml: 'آیا مایلید آیتم انتخاب‌ شده از آرشیو خارج شود؟',
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

  private afterSetDialogContent(instance: any, selectedItem: any, isNew: boolean): void {
    this.afterInitialDetailComponent.emit({instance, selectedItem, isNew});
  }

  private setAccess(): void {
    const menuItems = this.userDataService.getUserResourceAction();
    if (!this.resourceName) {
      this.resourceName = this.entityName;
    }
    const resourceName = this.resourceName.charAt(0).toUpperCase() + this.resourceName.slice(1);
    if (menuItems) {
      const resource = menuItems.filter(x => x.rk === resourceName);
      this.resourceAllowAdd = resource.filter(x => x.aId === 4).length >= 1;
      this.resourceAllowDelete = resource.filter(x => x.aId === 3).length >= 1;
      this.resourceAllowArchive = resource.filter(x => x.aId === 3).length >= 1;
      this.resourceAllowEdit = resource.filter(x => x.aId === 13).length >= 1;
    }
  }

  private setListTitle(): void {
    this.menuService.getMenuList().subscribe((menuItems: any) => {
      if (!this.title) {
        const moduleKy = this.router.url.split('/')[1];
        // const menuItems = this.cachedDataService.getData('menuItems');
        if (menuItems.data) {
          const rootMenu = menuItems.data.filter(x => x.moduleKey === moduleKy);
          if (rootMenu && rootMenu.length === 1) {
            const resourceName = this.entityName.charAt(0).toUpperCase() + this.entityName.slice(1);
            const menu = rootMenu[0].submenu.filter(x => x.resourceTitle === resourceName);
            if (menu.length === 1) {
              this.title = menu[0].title;
            }
          }
        }
      }
    });
  }

  exportGridToExcel(e): void {
    const gridInstance = this.listGrid.instance;
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('main sheet');
    try {
      gridInstance.beginUpdate();
      exportDataGrid({
        component: gridInstance,
        worksheet,
      }).then(() => {
        workbook.xlsx.writeBuffer()
          .then((buffer: BlobPart) => {
            saveAs(new Blob([buffer], {type: 'application/octet-stream'}), 'file.xlsx');
          }).then(() => {
          gridInstance.endUpdate();
        });
      });
    } catch (e) {
      console.log(e);
      gridInstance.endUpdate();
    }
    e.cancel = true;
  }

  // جهت ست کردن ستون مربوط به دکمه ویرایش و حذف
  setEditColumn(): void {
    if (this.isCustomEditForm && !this.isForSelectReadOnly) {
      const editColumn = { // Pushes the "Contacts" band column into the "columns" array
        caption: '',
        width: '80px',
        cellTemplate: 'cellTemplate',
        allowEditing: false,
        formItem: {
          visible: false
        }
      };
      if (this.allowEditInForm || this.allowDeleteInForm) {
        if (this.isForTree) {
          this.columns.push(editColumn);
        } else {
          this.columns.unshift(editColumn);
        }
      }
    }

    if (this.isForSelectReadOnly && this.selectByButton) {
      const selectRowColumn = { // Pushes the "Contacts" band column into the "columns" array
        caption: '',
        width: '100px',
        cellTemplate: 'selectRowButtonTemplate',
        allowEditing: false,
        formItem: {
          visible: false
        }
      };
      this.columns.push(selectRowColumn);
    }
  }

  setCustomRowButtons(): void {
    if (this.customRowButtons) {
      const buttonColumn = {
        caption: this.customRowButtons.caption,
        width: this.customRowButtons.width,
        allowExporting: this.customRowButtons.allowExporting,
        cellTemplate: 'rowButtonTemplate',
        allowEditing: false,
        formItem: {
          visible: false
        }
      };
      if (this.customRowButtons.setAtFirstColumn) {
        this.columns.unshift(buttonColumn);

      } else {
        this.columns.push(buttonColumn);
      }
    }
  }

  inlineBatchSaving(items: any) {
    if (this.editType == FormEditType.Inline && this.inlineEditMode == InlineEditModeEnum.Batch) {
      items.cancel = true;
      if (items.changes.length) {
        items.promise = this.processBatchRequest(
          this.customInlineUpdateMethodName,
          items.changes,
          items.component
        );
      }
    }
  }

  async processBatchRequest(
    customInlineUpdateMethodName: string,
    changes: any,
    component: any
  ): Promise<any> {
    const updateMethodName = customInlineUpdateMethodName ? customInlineUpdateMethodName : 'InlineEditBatchEntity';
    let items = [];
    for (let i = 0; i < changes.length; i++) {
      let item = changes[i].data;
      item.id = changes[i].key;
      items.push(item);
    }
    changes = await this.genericDataService.postEntityByUrl(`/api/${this.entityName}/${updateMethodName}`, JSON.stringify(items), false).toPromise()
      .then(res => {
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
          this.showMessage(message);
        } else {
          this.showError(message);
        }
      });
    ;
    component.getDataSource().store().push(changes);
    await component.refresh(true);
    component.cancelEditData();
  }


  onCustomCheckBoxClicked() {
    this.customCheckBoxEvent.emit(this.customCheckboxIsChecked);
  }
}
