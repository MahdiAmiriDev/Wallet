import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {CachedDataService} from '../../../shared/services/cached-data.service';
import {CompanyDetailTabBasedComponent} from './company-detail-tab-based.component';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';
import {UserDataService} from '../../../_services/user-data.service';
import {
  CanUserSeeOtherCompaniesDetailComponent
} from './can-user-see-other-companies/can-user-see-other-companies-detail.component';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {YashilCustomButtonColumn} from "../../../shared/base/classes/yashil-cutom-button-column";
import {CompanyInformationModel} from "../_models/company-information.model";


@Component({
  selector: 'app-company-list',
  templateUrl: './company-list.component.html'
})
export class CompanyListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'company';
  detailComponent: any;
  companyId: number;
  representationId: number;
  canUserSeeOtherCompany: boolean = false;
  showCompanyCategory: boolean;
  private baseListUrl = 'company/GetCurrentUserCompanyWithChildrenForList?';
  selectedEntity: any;
  customRowButton: YashilCustomButtonColumn;
  showPopUp: boolean;
  companyInformation: CompanyInformationModel;
  lmsMode: boolean;

  constructor(private cachedDataService: CachedDataService,
              private genericDataService: GenericDataService,
              private translate: TranslateService, private userDataService: UserDataService) {
    super();
    this.lmsMode = this.userDataService.getSiteConfigurationLmsMode();
    this.bindCustomRowButton = this.bindCustomRowButton.bind(this);
    this.showCompanyInformationPopUp = this.showCompanyInformationPopUp.bind(this);
  }

  headId: number;

  _parentId: number;
  get parentId(): number {
    return this._parentId;
  }

  @Input()
  set parentId(val) {
    if (val !== this._parentId) {
      this._parentId = val;
    }
  }

  _establishedLicenseType: number;
  get establishedLicenseType(): number {
    return this._establishedLicenseType;
  }

  @Input()
  set establishedLicenseType(val) {
    if (val !== this._establishedLicenseType) {
      this._establishedLicenseType = val;
    }
  }

  ngOnInit(): void {
    this.getDisplayCompanyCategory();
    /*
    بر اساس مسیر موجود در url تصمیم می گیریم که کدام یک را به عنوان کامپوننت دیتا در نظر بگیریم
    اگر از مسیر canUserSeeOtherCompanies وارد شده باشد به این معنی است که می خواهد فقط این فیلد مربوط به موجودیت company را
    ویرایش کند بنا بر این کامپوننت دیتا را به CanUserSeeOtherCompaniesDetailComponent تغییر می دهیم
    */
    let currentRoute = window.location.pathname.split('/')[2];
    switch (currentRoute) {
      case 'canUserSeeOtherCompanies': {
        this.detailComponent = CanUserSeeOtherCompaniesDetailComponent;
        this.canUserSeeOtherCompany = true;
        break;
      }
      case 'companys': {
        this.detailComponent = CompanyDetailTabBasedComponent;
        break;
      }
    }

    const data = this.userDataService.getAdditionalUserProp();
    if (data) {
      if (data.companyId) {
        this.companyId = data.companyId;
      }
      if (data.representationId) {
        this.representationId = data.representationId;
      }
    }

    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }

    let result = forkJoin({
      title: this.translate.get('Title'),
      establishedLicenseTypeTitle: this.translate.get('shp.Company.EstablishedLicenseType'),
      displayOrder: this.translate.get('shp.Company.DisplayOrder')
    });

    result.subscribe(({title, establishedLicenseTypeTitle, displayOrder}) => {
        this.columns.push({
          caption: title,
          dataField: 'title',
          visibleIndex: 0,
          alignment: 'right'
        });
    });
    //اگر lms mode در  local storage  برابر با true  باشد دکمه را نمایش نمی دهد
    if (!this.lmsMode) {
      this.bindCustomRowButton();
    }
  }


  afterInitialDetailComponent(componentInstance: any) {
    if (!this.canUserSeeOtherCompany) {
      const comp = (<CompanyDetailTabBasedComponent>componentInstance.instance);

      if (this.parentId) {
        comp.parentId = this.parentId;
      }

      if (this.establishedLicenseType) {
        comp.establishedLicenseType = this.establishedLicenseType;
      }
      if (this.selectedEntity) {
        comp.parentId = this.selectedEntity.parentId;
      }
      if (this.showCompanyCategory) {
        comp.showCompanyCategory = this.showCompanyCategory;
      }
    }
  }

  public bindCustomDataSources() {
    let customListUrl = `${this.baseListUrl}`;
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    return true;
  }

  onRowSelect(item: any): void {
    this.selectedEntity = item;
    this.selectedRowChanged.emit(item);
  }

  getDisplayCompanyCategory() {
    let key = 'Site.Configuration.AssignCategoryForCompanyAndRepresentationAfterInsert';
    this.genericDataService.getEntitiesWithAction('AppConfig', `GetAppConfigValue?key=${key}`, null)
      .subscribe((res: any) => {
        if (res) {
          this.showCompanyCategory = !JSON.parse(res.value.toLowerCase());
        }
      });
  }

  bindCustomRowButton(): void {
      this.customRowButton = {
        width: '130px',
        setAtFirstColumn: false,
        customButtons: [
          {
            hint: 'اطلاعات مرکز',
            text: 'اطلاعات مرکز',
            icon: 'columnchooser',
            type: 'default',
            onClick: this.showCompanyInformationPopUp,
            visibleProp:'showCompanyInformationButton'
          }
        ]
      }
  }

  //زمانی که کاربر بر روی دکمه ی نمایش اطلاعات مرکز کلیک می کند این متد فراخوانی می شود
  showCompanyInformationPopUp(selectedCompany: any): void {
    if (selectedCompany.data.id) {
      this.genericDataService.getEntitiesWithAction('company', `GetCompanyInformation?companyId=${selectedCompany.data.id}`, null)
        .subscribe((res: any) => {
          if (res) {
            this.companyInformation = {
              showInMainPage: res.showInMainPage ? 'فعال است': 'فعال نیست',
              hasActiveContract: res.hasActiveContract ?  'دارد':'ندارد',
              hasDesktopAndMobileModeImage: res.hasDesktopAndMobileModeImage ? 'دارد':'ندارد',
              hasActiveProductSalesPlanning: res.hasActiveProductSalesPlanning ? 'دارد':'ندارد'
            }
            this.showPopUp = true;
          }
        });
    }
  }
}
