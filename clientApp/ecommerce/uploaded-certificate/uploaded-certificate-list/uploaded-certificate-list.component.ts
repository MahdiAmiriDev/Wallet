import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Selectable} from "../../../../shared/base/classes/selectable";
import {BaseList} from "../../../../shared/base/classes/base-list";
import {GenericDataService} from "../../../../shared/base/services/generic-data.service";

@Component({
  selector: 'app-uploaded-certificate-list',
  templateUrl: './uploaded-certificate-list.component.html'
})

export class UploadedCertificateListComponent extends Selectable implements OnInit {


  @ViewChild('listForm', {static: true}) listForm: BaseList;

  @Output() afterFinalRegistration: EventEmitter<any> = new EventEmitter<any>();

  @Input() invalidItem: boolean;

  baseListUrl: string = 'UploadedCertificate/GetUploadedCertificateItems?';
  columns: any[] = [];
  customButtons: any[] = [];

  constructor(private genericDataService:GenericDataService) {
    super();
  }

  ngOnInit() {
    this.bindColumns();
    this.bindCustomButtons();
    this.bindCustomDataSources();
  }

  bindColumns(): void {
    this.columns.push(
      {
        caption: 'تاریخ صدور',
        dataField: 'issueDateInt',
        width: '120px'
      }, {
        caption: 'شماره پیگیری',
        dataField: 'number',
        width: '120px'
      }, {
        caption: 'نام شهر',
        dataField: 'cityTitle',
        width: '120px'
      }, {
        caption: 'نام نمایندگی',
        dataField: 'representationTitle',
        width: '120px'
      }, {
        caption: 'نام استاد',
        dataField: 'teacherName',
        width: '120px'
      }, {
        caption: 'عنوان دوره',
        dataField: 'courseTitle',
        width: '120px'
      }, {
        caption: 'نوع دوره',
        dataField: 'courseType',
        width: '120px'
      }, {
        caption: 'طول دوره',
        dataField: 'courseDuration',
        width: '120px'
      }, {
        caption: 'جنسیت',
        dataField: 'sexTitle',
        width: '120px'
      }, {
        caption: 'نمره',
        dataField: 'score',
        width: '120px'
      }, {
        caption: 'رشته تحصیلی',
        dataField: 'fieldOfStudy',
        width: '120px'
      }, {
        caption: 'شماره موبایل مربی',
        dataField: 'teacherMobile',
        width: '120px'
      }, {
        caption: 'پیام خطا',
        dataField: 'errorMessage',
        width: '120px'
      })
  }

  bindCustomButtons(): void {
    if (!this.invalidItem) {
      this.customButtons = [{
        icon: 'file',
        text: 'ثبت نهایی',
        type: 'success',
        key: 'finalRegistration',
        float: 'right'
      }];
    }
  }

  bindCustomDataSources(): boolean {
    if (this.listForm) {
      this.baseListUrl += `showInvalidItem=${this.invalidItem}`;
      this.listForm.customListUrl = this.baseListUrl;
      this.listForm.refreshList();
      return true;
    }
  }

  customButtonClicked(key: any): void {
    if (key === 'finalRegistration') {
      this.confirmFinalRegistration();
    }
  }

  confirmFinalRegistration() {
      this.confirmBeforeAction('ثبت نهایی', 'آیا از ثبت نهایی گواهی نامه های بارگذاری شده در سایت اطمینان دارید؟', 'تایید')
        .show().then((dialogResult) => {
        if (dialogResult.res === 'ok') {
          this.finalRegistration();
        }
      });
    }

  public finalRegistration(): void {
    this.setBusy(true);
    this.genericDataService.getEntitiesWithAction(`uploadedCertificate`, 'AddCertificateFromUploadedCertificate',null)
      .subscribe((response:any) => {
        if (!response.isSuccess) {
          this.setBusy(false);
          this.afterFinalRegistration.emit();
          this.showError(response.message);
        } else {
          this.setBusy(false);
          this.afterFinalRegistration.emit();
          this.showMessage(response.message);
        }
      });
  }


  onListRowPrepared(e: any): void {
    if (e && e.rowType === 'data' && e.data.errorMessage) {
      e.rowElement.bgColor = '#ffc107';
      e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
    }
  }
}
