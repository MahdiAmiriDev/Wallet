import * as moment from 'jalali-moment';
import {Directive, Input, OnInit, ViewChild} from '@angular/core';
import {GenericDataService} from '../services/generic-data.service';
import {DxFormComponent} from 'devextreme-angular/ui/form';
import notify from 'devextreme/ui/notify';
import {catchError} from 'rxjs/operators';
import {BaseEditFormComponent} from '../base-edit-form/base-edit-form.component';
import {Editable} from './editable';
import 'moment/locale/fr';
import {Observable, throwError} from 'rxjs';
import {IActiveDate} from 'ng-persian-datepicker';
import {custom} from 'devextreme/ui/dialog';

@Directive()
export class BaseEdit extends Editable implements OnInit {
  loadForm: boolean;
  // tslint:disable-next-line:variable-name
  _genericDataService: GenericDataService;
  dataObservable: Observable<any>;

  @Input()
  closeAfterSave = true;
  keyExpr: string;
  displayExpr: string;
  parentIdExpr: string;
  /**
   * Set Custom Method Name
   * Sample CustomMethod?id=1&code=9854
   */
  customLoadMethodNameWithParams: string;
  customUpdateMethodName: string;
  customAddMethodName: string;
  buttonOptions: any = {
    text: 'Register',
    type: 'success',
    useSubmitBehavior: true
  };
  public datePickerConfig = {
    format: 'jYYYY/jMM/jDD',
    dir: 'rtl',
    locale: moment.locale('fa')
  };
  public timePickerConfig = {
    format: 'HH:mm',
    dir: 'rtl',
    locale: moment.locale('fa')
  };
  entity: any = {};
  @ViewChild(DxFormComponent, {static: true})
  detailForm: DxFormComponent;
  @ViewChild(BaseEditFormComponent, {static: true})
  baseEditFormComponent: BaseEditFormComponent;
  @Input()
  stayOnPageAfterSave = false;
  private dataFilters: any = [];
  /**
   *
   */
  updateOrInsertAffected: boolean;

  constructor(genericDataService: GenericDataService) {
    super();
    this._genericDataService = genericDataService;
  }

  // tslint:disable-next-line:variable-name
  _entityParentId: number;

  public get entityParentId(): number {
    return this._entityParentId;
  }

  @Input()
  public set entityParentId(value: number) {
    this._entityParentId = value;
    this.entity.parentId = this._entityParentId;
  }

  public get filters(): any[] {
    return this.dataFilters;
  }

  @Input()
  public set filters(val: any[]) {
    this.dataFilters = val;
    if (this.isNew) {
      this.applyFilter();
    }
  }

  public onCloseFormClick(): void {
    this.closeFormClick.next(this.updateOrInsertAffected);
  }

  private closeForm(): void {
    if (this.closeAfterSave) {
      this.selectedEntityId = null;
      this.closeFormClick.emit(this.updateOrInsertAffected);
    }
  }

  public convertMomentDateToInt(p: moment.Moment): number {
    if (p) {
      if (typeof p === 'string') {
        if (String(p).length === 10) {
          const regex = /\//gi;
          const res = +String(p).replace(regex, '');
          if (!isNaN(res)) {
            return res;
          }
          return 14000101;
        }
        return 14000101;
      }
      return +p.format('YYYYMMDD');
    }
    return null;
  }

  public convertDateToString(p: moment.Moment): string {
    if (p) {
      return p.locale('fa').format('YYYY/MM/DD');
    }
    return null;
  }

  doBeforeSubmit(e): Observable<boolean> {
    return new Observable(o => o.next(true));
  }

  // اگر فرم دارای فایل باشد که همراه اطلاعات دیگر ذخیره میشود باید مقدار withFile=true باشد
  // همچنین اطلاعات بصورت formData ارسال میشود و در کنترلر اطلاعات ورودی بصورت [FromForm] دریافت شود
  public formSubmit(e, detailForm: DxFormComponent = null, withFile = false): void {
    if (detailForm) {
      this.detailForm = detailForm;
    }
    this.baseEditFormComponent.allowSave = false;
    if (e) {
      e.preventDefault();
    }
    this.doBeforeSubmit(e).subscribe(res => {
      if (res) {
        if (this.detailForm && this.detailForm.instance) {
          try {
            const result = this.detailForm.instance.validate();
            if (result.status === 'pending') {
              (result.complete as Promise<any>).then(x => {
                if (x.isValid) {
                  this.doSubmitForm(withFile);
                }
                this.baseEditFormComponent.allowSave = true;
              });
            } else {
              if (result.isValid) {
                this.doSubmitForm(withFile);
              } else {
                this.baseEditFormComponent.allowSave = true;
              }
            }
          } catch (e) {
          } finally {
            this.baseEditFormComponent.allowSave = true;
          }
        }
      } else {
        this.baseEditFormComponent.allowSave = true;
      }
    });
  }

  afterSave(): void {
  }

  ngOnInit(): void {
    this.loadEntityData();
  }

  public afterLoadData(res: any): any {
    this.afterDataLoaded.emit(res);
  }

  public beforeLoadData(): void {
  }

  public loadEntityData(): void {
    this.beforeLoadData();
    if ((this.selectedEntityId && !this.isNew)) {
      this.setBusy(true);
      this.dataObservable = this._genericDataService.getEntity(this.entityName, this.selectedEntityId, this.customLoadMethodNameWithParams);
      this.dataObservable.subscribe(
        res => {
          this.entity = res;
          this.afterLoadData(res);
          this.setBusy(false);
        }
      );
    } else {
      if (this.entityParentId) {
        this.entity[this.parentIdExpr] = this.entityParentId;
      }
      this.applyFilter();
    }
  }

  applyFilter(): void {
    let i = 0;
    for (const item of this.filters) {
      this.entity[item[0]] = item[1];
      i++;
    }
  }

  // اگر فرم دارای فایل باشد که همراه اطلاعات دیگر ذخیره میشود باید مقدار withFile=true باشد
  doSubmitForm(withFile = false): void {
    this.setBusy(true);
    if (this.entity.id) {
      this._genericDataService.updateEntity(this.entityName, this.entity, this.customUpdateMethodName, withFile)
        .pipe(catchError(err => {
          this.showErrorMessage(err);
          this.baseEditFormComponent.allowSave = true;
          this.setBusy(false);
          return throwError('بروز خطا');
        })).subscribe(
        res => {
          this.showResponseMessage(res);
          this.baseEditFormComponent.allowSave = true;
          this.setBusy(false);
          if (res && res.isSuccess) {
            this.updateOrInsertAffected = true;
            if (this.emitEntityAfterUpdatedRow) {
              this.afterUpdateRow.emit(this.entity);
            } else {
              this.afterUpdateRow.emit(res);
            }
            this.afterSave();
            this.closeForm();
          } else {
            this.afterUpdateFailed.emit(res);
          }
        });
    } else {
      this._genericDataService.addEntity(this.entityName, this.entity, this.customAddMethodName, withFile).pipe(catchError(err => {
        this.showErrorMessage(err);
        this.baseEditFormComponent.allowSave = true;
        this.setBusy(false);
        return throwError('بروز خطا');
      })).subscribe(
        res => {
          this.showResponseMessage(res);
          this.baseEditFormComponent.allowSave = true;
          this.setBusy(false);
          if (res && res.isSuccess) {
            if (res.entityId) {
              this.entity.id = res.entityId;
              this.selectedEntityId = res.entityId;
            }
            this.updateOrInsertAffected = true;
            if (this.emitEntityAfterInsertedRow) {
              this.afterInsertRow.emit(this.entity);
            } else {
              this.afterInsertRow.emit(res.entityId);
            }
            this.afterSave();
            if (this.stayOnPageAfterSave) {
              this.loadEntityData();
            } else {
              this.closeForm();
            }
          } else {
            this.afterInsertFailed.emit(res);
          }
        }
      );
    }
  }


  private showErrorMessage(err: any): void {
    let errorMsg = '';
    if (err && err.error !== '') {
      if (typeof (err.error) === 'object') {
        if (err.status === 400) {
          errorMsg = 'لطفا ورودی ها را چک کنید';
        } else {
          errorMsg = 'بروز خطا در عملیات ذخیره سازی';
        }
      } else {
        errorMsg = err.error;
      }
    }
    notify({
      message: errorMsg,
      position: {
        my: 'center top',
        at: 'center top'
      },
      rtlEnabled: true
    }, 'error', 3000);
  }

  public convertDateToInt(activeDate: IActiveDate): number {
    if (activeDate) {
      const persianDate = activeDate.shamsi;
      if (String(persianDate).length === 10) {
        const regex = /\//gi;
        const res = +String(persianDate).replace(regex, '');
        if (!isNaN(res)) {
          return res;
        }
        return 14000101;
      }
      return 14000101;
    }
  }

  public confirmBeforeAction(title: string, messageHtml: string, confirmText: string): any {
    return custom({
      title,
      messageHtml,
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
}
