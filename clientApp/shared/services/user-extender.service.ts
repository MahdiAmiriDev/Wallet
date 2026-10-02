import {Injectable} from '@angular/core';
import {GenericDataService} from '../base/services/generic-data.service';
import {DxFormComponent} from 'devextreme-angular/ui/form';
import {CachedDataService} from './cached-data.service';
import {UserDataService} from '../../_services/user-data.service';
import {forkJoin} from "rxjs";
import {TranslateService} from "@ngx-translate/core";

@Injectable({
  providedIn: 'root'
})
export class UserExtenderService {
  representationDataSource: any;
  companyDataSource: any;
  representationCategoryDataSource: any;
  _detailForm: DxFormComponent;
  _entity: any = {};
  /**
   * set true only on edit form
   */
  firstLoad = false;
  representationCategoryFirstLoad = false;

  constructor(private genericDataService: GenericDataService, private cachedDataService: CachedDataService, private userDataService: UserDataService) {
    this.selectedCompanyChanged = this.selectedCompanyChanged.bind(this);
    this.selectedRepresentationChanged = this.selectedRepresentationChanged.bind(this);
    this.maximumUploadCapacityValueChange = this.maximumUploadCapacityValueChange.bind(this);
  }

  /**
   * Call This Method Only On Edit Form
   * @param entity user Entity Object
   * @param isNew is New Record
   */
  setEntity(entity, isNew = false): void {
    if (isNew) {
      this.setAdditionalUserProp(entity);
    }

    if (entity.additionalInfoList) {
      if (entity.additionalInfoList.companyId) {
        entity.additionalInfoList.companyId = +entity.additionalInfoList.companyId;
      }
      if (entity.additionalInfoList.representationId) {
        entity.additionalInfoList.representationId = +entity.additionalInfoList.representationId;
      }
      if (entity.additionalInfoList.representationCategoryId) {
        entity.additionalInfoList.representationCategoryId = +entity.additionalInfoList.representationCategoryId;
      }
      if (entity.additionalInfoList.maximumUploadCapacity)
        entity.additionalInfoList.maximumUploadCapacity = +entity.additionalInfoList.maximumUploadCapacity;
    }
    this._entity = entity;

    if (this._entity.additionalInfoList && this._entity.additionalInfoList.companyId) {

    }
    this.bindRepresentationDataSource(entity.additionalInfoList.companyId);
    this.bindRepresentationCategoryDataSource(entity.additionalInfoList.representationId);
    if (!isNew) {
      this.firstLoad = true;
      this.representationCategoryFirstLoad = true;
    }
  }


  setAdditionalUserProp(entity: any) {
    const additionalUserProp = this.userDataService.getAdditionalUserProp();
    if (additionalUserProp.companyId) {
      entity.additionalInfoList.companyId = +additionalUserProp.companyId;
    }
    if (additionalUserProp.representationId) {
      entity.additionalInfoList.representationId = +additionalUserProp.representationId;
    }
    if (additionalUserProp.representationCategoryId) {
      entity.additionalInfoList.representationCategoryId = +additionalUserProp.representationCategoryId;
    }
  }

  extend(detailForm: DxFormComponent, userEditFormItems: any[], isNew = false, extendedFormItemsTranslate: any): any[] {
    this._detailForm = detailForm;
    this.companyDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'company');

    const additionalUserProp = this.userDataService.getAdditionalUserProp();
    if (additionalUserProp.companyId) {
      this.firstLoad = true;
      this.representationCategoryFirstLoad = true;
      this.bindRepresentationDataSource(additionalUserProp.companyId);
    }
    if (additionalUserProp.representationId) {
      this.bindRepresentationCategoryDataSource(additionalUserProp.representationId);
    }

    const lmsMode = this.userDataService.getSiteConfigurationLmsMode();
    if (lmsMode) {
      userEditFormItems[1].items.splice(1, 0,
        {
          name: 'maximumUploadCapacity',
          label: {
            text: 'ظرفیت باگذاری کاربر فایل به مگابایت'
          }, editorOptions: {
            onValueChanged: this.maximumUploadCapacityValueChange,
          },
          dataField: 'additionalInfoList.maximumUploadCapacity',
          editorType: 'dxNumberBox',
        });
    }

    userEditFormItems[1].items.splice(1, 0, {
      name: 'representationCategoryId',
      label: {
        text: extendedFormItemsTranslate.categoryTitle
      },
      dataField: 'additionalInfoList.representationCategoryId',
      editorType: 'dxLookup',
      editorOptions: {
        rtlEnabled: true,
        dropDownOptions: {closeOnOutsideClick: true},
        showTitle: false,
        dataSource: this.representationCategoryDataSource,
        displayExpr: 'categoryTitle',
        valueExpr: 'id',
        showClearButton: true,
        readOnly: additionalUserProp && additionalUserProp.representationCategoryId
      }
    });
    userEditFormItems[1].items.splice(1, 0, {
      name: 'representationId',
      label: {
        text: extendedFormItemsTranslate.representationTitle
      },
      dataField: 'additionalInfoList.representationId',
      editorType: 'dxLookup',
      editorOptions: {
        rtlEnabled: true,
        dropDownOptions: {closeOnOutsideClick: true},
        showTitle: false,
        dataSource: this.representationDataSource,
        onSelectionChanged: this.selectedRepresentationChanged,
        displayExpr: 'title',
        valueExpr: 'id',
        showClearButton: true,
        readOnly: additionalUserProp && additionalUserProp.representationId
      }
    });
    userEditFormItems[1].items.splice(1, 0,
      {
        name: 'companyId',
        label: {
          text: extendedFormItemsTranslate.companyTitle
        },
        template: 'categoryTree',
      });


    userEditFormItems = userEditFormItems.filter(x => x.name !== 'organizationId');
    return userEditFormItems;
  }

  private selectedRepresentationChanged(e: any): void {
    if (e && e.selectedItem) {
      this._detailForm.instance.getEditor('representationCategoryId').option('disabled', false);
      if (!this.representationCategoryFirstLoad && this._entity) {
        this._detailForm.instance.getEditor('representationCategoryId').reset();
      }
      if (e.selectedItem.id) {
        this.bindRepresentationCategoryDataSource(e.selectedItem.id);
      }
      this.representationCategoryFirstLoad = false;
    }
  }

  public selectedCompanyChanged(e: any): void {

    if (e && e.itemData) {
      this._detailForm.instance.getEditor('representationId').option('disabled', false);
      if (!this.firstLoad && this._entity) {
        this._entity.additionalInfoList.companyId = e.itemData.id;
        this._detailForm.instance.getEditor('representationId').reset();
        this._detailForm.instance.getEditor('representationCategoryId').reset();
        this.bindRepresentationDataSource(e.itemData.id);
        this.bindRepresentationCategoryDataSource(null);
        this._entity.additionalInfoList.representationCategoryId = null;
      }
      this.firstLoad = false;
    }
    if (e == null) {
      this._entity.additionalInfoList.companyId = null;
    }
  }

  private bindRepresentationCategoryDataSource(representationId): any {
    if (representationId) {
      this.representationCategoryDataSource =
        this.genericDataService.createCustomDataSourceWithAction('id', 'representationCategory',
          `GetByRepresentationId?representationId=${representationId}`);
    } else {
      this.representationCategoryDataSource = null;
    }
    if (this._detailForm && this._detailForm.instance) {
      this._detailForm.instance.getEditor('representationCategoryId').option('dataSource', this.representationCategoryDataSource);
    }
    return this.representationCategoryDataSource;
  }

  private bindRepresentationDataSource(companyId): any {
    if (companyId) {
      this.representationDataSource = this.genericDataService.createCustomDataSourceWithAction('id', 'representation', `GetByCompany?companyId=${companyId}`);
    } else {
      this.representationDataSource = null;
    }
    if (this._detailForm && this._detailForm.instance) {
      this._detailForm.instance.getEditor('representationId').option('dataSource', this.representationDataSource);
    }
    return this.representationDataSource;
  }


  //زمانی که مقدار حداکثر فضای اختصاص داده شده به کاربر برای بارگذاری فایل تغییر کند
  //این متد فراخوانی شده و مقدار جدید را به مدل بایند می کند.
  maximumUploadCapacityValueChange(itemData: any): void {
    this._entity.additionalInfoList.maximumUploadCapacity = itemData.value;
  }


  doBeforeSubmit(): void {
    this._entity.additionalInfoList.representationCategoryId = String(this._entity.additionalInfoList.representationCategoryId);
    this._entity.additionalInfoList.representationId = String(this._entity.additionalInfoList.representationId);
    this._entity.additionalInfoList.companyId = String(this._entity.additionalInfoList.companyId);

    this._entity.additionalInfoList.maximumUploadCapacity = this._entity.additionalInfoList.maximumUploadCapacity == undefined ?
      String(null) :
      String(this._entity.additionalInfoList.maximumUploadCapacity);
  }
}
