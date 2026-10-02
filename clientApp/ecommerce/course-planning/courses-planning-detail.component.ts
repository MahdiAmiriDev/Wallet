import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {CachedDataService} from '../../../shared/services/cached-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';
import {DxDropDownBoxComponent} from 'devextreme-angular/ui/drop-down-box';
import * as moment from 'jalali-moment';
import {forkJoin, Observable, of} from 'rxjs';
import {TranslateService} from '@ngx-translate/core';
import {FormControl} from '@angular/forms';
import {UserDataService} from '../../../_services/user-data.service';
import {SaleTypeEnum} from '../enums/sales-mode-type-enum';

@Component({
  selector: 'app-courses-planning-detail',
  templateUrl: './courses-planning-detail.component.html',
})
export class CoursePlanningDetailComponent extends BaseEdit implements OnInit {
  startDateDataValue = new FormControl();
  endDateDataValue = new FormControl();
  confirmStates: any[];
  @ViewChild(DxTreeViewComponent) treeView;
  @ViewChild('ddlCourse') courseComponent: DxDropDownBoxComponent;
  @Input()
  showConfirm: boolean;
  userRepresentationId: number;
  @Input()
  showSaleConfigurationDiscountPercentage = false;
  @Input()
  representationId: number;
  @Input()
  companyId: number;
  @Input()
  categoryId: number;
  @Input()
  productSalesPlanningDescription: string;
  representationDataSource: any;
  courseStatus: any;
  representationPersonDataSource: any;
  courseDataSource: any;
  ageCategorys: any;
  implementationTypes: any;
  courseTypes: any;
  runTypes: any;
  customGenders: any;
  accessLevels: any[] = [];
  formLabels: any = {};
  @Input()
  productId: number;
  hasFinalDiscountPercentageChanged = false;
  representationTeacherCustomListUrl = 'RepresentationTeacher/GetByCustomForSelect';
  @Input()
  isCourse: boolean = false;
  canUserSeeOtherCompany: boolean;
  //product
  skillTypes: any;
  certificateTypes: any;
  evaluationMethods: any;
  gridHeight: number;
  productSalePlanningId: number;
  discountPercentageWithDescription: string = ' ';
  classRoomDataSource: any;
  lmsMode: boolean = false;
  isAdminApproval: boolean = false;
  //آیا محصولی انتخاب شده است
  //اگر محصولی انتخاب نشده باشد فیلد های  برنامه ریزی دوره غیرفعال می باشند
  //و پس از انتخاب محصول توسط کاربر فیلد ها را فعال می کنیم
  isProductSelected: boolean;
  //برای فعال یا غیر فعال کردن دراپ داون انتخاب نمایندگی کاربرد دارد
  disableSelectingRepresentationDropDown: boolean = false;
  sumOfPointsIsValid: boolean = true;
  pointsRequirement: boolean = true;

  classPoint: number = 0;
  examPoint: number = 0;
  homeWorkPoint: number = 0;
  usingAbilitySkillLevel: boolean = false;
  disableSelectRepresentation: boolean = false;
  @Input()
  isAdmin:boolean = false;

  constructor(private genericDataService: GenericDataService, private cachedDataService: CachedDataService, private translate: TranslateService, private userDataService: UserDataService) {
    super(genericDataService);
    this.entityName = 'productSalesPlanning';
    this.selectedRepresentationValueChanged = this.selectedRepresentationValueChanged.bind(this);
    this.ddlCourseSelectedChanged = this.ddlCourseSelectedChanged.bind(this);
    this.getRepresentationDataSource = this.getRepresentationDataSource.bind(this);
    this.changeDisableBuyButtonValue = this.changeDisableBuyButtonValue.bind(this);
    this.assignProductSalesPlanningToAllRepresentationChanged = this.assignProductSalesPlanningToAllRepresentationChanged.bind(this);
    this.gridHeight = window.innerHeight - 520;
  }

  ngOnInit(): void {
    this.loadForm = !this.selectedEntityId;
    this.lmsMode = this.userDataService.getSiteConfigurationLmsMode();
    this.getUsingAbilityToDetermineSkillLevelOfStudentsCourses();
    this.getAdminApprovalRequiredForProductSalesPlanning();

    if (this.companyId) {
      this.getCanUserSeeOtherCompanies();
    }
    super.ngOnInit();
    let result = forkJoin({
      initialRegistrationTitle: this.translate.get('InitialRegistration'),
      confirmTitle: this.translate.get('Confirm'),
      refuseTitle: this.translate.get('Refuse'),
      finalRegistrationTitle: this.translate.get('FinalRegistration'),
      confirmDiscountPercentageTitle: this.translate.get('tms.Course.Validation.ConfirmDiscountPercentage'),
      dissuasionTitle: this.translate.get('Dissuasion'),
      discountPercentageWithDescription: this.translate.get('tms.CoursePlaning.DiscountPercentageWithDescription')
    });

    result.subscribe(({
                        initialRegistrationTitle,
                        confirmTitle,
                        refuseTitle,
                        finalRegistrationTitle,
                        confirmDiscountPercentageTitle,
                        dissuasionTitle,
                        discountPercentageWithDescription
                      }) => {

      this.formLabels.initialRegistration = initialRegistrationTitle;
      this.formLabels.description = confirmTitle;
      this.formLabels.refuse = refuseTitle;
      this.formLabels.finalRegistration = finalRegistrationTitle;
      this.formLabels.confirmDiscountPercentage = confirmDiscountPercentageTitle;
      this.formLabels.dissuasion = dissuasionTitle;
      this.formLabels.discountPercentageWithDescription = discountPercentageWithDescription;

      this.confirmStates = [
        {id: null, title: this.formLabels.initialRegistration},
        {id: true, title: this.formLabels.description},
        {id: false, title: this.formLabels.refuse}];
    });
    /*
    اگر درصد تخفیف تغییر کرده باشد نمایش پیام درصد تخفیف به کاربر برابر با True می شود
     */

    const data = this.userDataService.getAdditionalUserProp();
    if (data && data.representationId) {
      this.userRepresentationId = data.representationId;
      if (!this.selectedEntityId) {
        this.entity.representationId = data.representationId;
        this.representationId = data.representationId;
      }
    }
    if (!this.selectedEntityId) {

      this.initialEntityDefaultValues();
      //
      if (this.companyId) {
        this.entity.companyId = this.companyId;
      }
      if (this.categoryId) {
        this.entity.categoryId = this.categoryId;
      }
      if (this.productId) {
        this.entity.productId = this.productId;
      }
      //this.bindCourseDataSources();
    } else {
      this.hasFinalDiscountPercentageChanged = false;
      this.productSalePlanningId = this.selectedEntityId;
    }
    this.representationPersonDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'representationPerson');
    if ( (this.companyId === undefined || this.companyId == null || !this.companyId) && this.selectedEntityId ) {
      this._genericDataService.getEntitiesWithAction('representation', 'GetByProductSalesPlanningCompany?productSalesPlanningId=' + this.selectedEntityId, null)
        .subscribe((res: any) => {
          this.representationDataSource = res.data;
        });

    } else {
      this._genericDataService.getEntitiesWithAction('representation', 'GetByCompany?companyId=' + this.companyId, null)
        .subscribe((res: any) => {
          this.representationDataSource = res.data;
        });
    }
    this._genericDataService.getCommonBaseDataForSelect('CourseStatus').subscribe(res => this.courseStatus = res);
    this._genericDataService.getCommonBaseDataForSelect('AgeCategory').subscribe(res => this.ageCategorys = res);
    this._genericDataService.getCommonBaseDataForSelect('ImplementationType').subscribe(res => this.implementationTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('CourseType').subscribe(res => this.courseTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('RunType').subscribe(res => this.runTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('CustomGender').subscribe(res => this.customGenders = res);

    this._genericDataService.getCommonBaseDataForSelect('SkillType').subscribe(res => this.skillTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('CertificateType').subscribe(res => this.certificateTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('EvaluationMethod').subscribe(res => this.evaluationMethods = res);
    this.classRoomDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'classRoom');


    this.setCustomAddAndUpdateApiActionName();

  }

  //این متد جهت ست کردن مقدار آدرس
  //APi Action Method
  //برای عملیات های درج و بروزرسانی برنامه ریزی دوره کاربرد دارد
  setCustomAddAndUpdateApiActionName() {
    this.customAddMethodName = 'AddProductSalesPlanning';
    this.customUpdateMethodName = 'UpdateProductSalesPlanning';
  }

  /*دریافت استفاده از توانایی تعیین سطح مهارت دروس دانشجویان*/
  getUsingAbilityToDetermineSkillLevelOfStudentsCourses() {
    this.genericDataService.getEntitiesWithAction(
      'YashilLightAppConfig',
      'GetUsingAbilityToDetermineSkillLevelOfStudentsCourses',
      null
    ).subscribe(res => {
      // More robust boolean conversion
      this.usingAbilitySkillLevel = !!res;
    }, error => {
      this.usingAbilitySkillLevel = false;
    });
  }

  /*دریافت کانفیگ مربوط به نیاز به تایید ادمین یا بالادستی برای ایجاد یا ویرایش برنامه ریزی دوره*/
  getAdminApprovalRequiredForProductSalesPlanning() {
    this.genericDataService.getEntitiesWithAction(
      'YashilLightAppConfig',
      'GetAdminApprovalRequiredForProductSalesPlanning',
      null
    ).subscribe(res => {
      this.isAdminApproval = !!res;
    }, error => {
      this.isAdminApproval = false;
    });
  }


  getRepresentationDataSource(options): void {
    return this.representationDataSource;
  }

  selectedRepresentationValueChanged(e): void {
    if (e.previousValue != null && e.previousValue) {
      this.entity.categoryId = null;
      this.representationId = e.value;
      //  this.bindCourseDataSources();
    }
  }

  afterLoadData(res: any): any {
    // this.bindCourseDataSources();
    this.hasFinalDiscountPercentageChanged = res.hasFinalDiscountPercentageChanged;
    this.loadForm = true;
    return super.afterLoadData(res);
  }


  onCourseSelectionChanged(e): void {
    if (e && e.data) {
      this.entity.productId = e.data.id;
      this.entity.duration = e.data.duration;
    }
  }


  ddlCourseSelectedChanged(item: any): void {
    this.entity.productU = null;
  }

  doBeforeSubmit(e): Observable<boolean> {


    if (!this.checkPointsRequirement()) {
      this.pointsRequirement = false;
      return of(false);
    } else {
      this.pointsRequirement = true;
    }

    if (!this.validatePointsSum()) {
      this.sumOfPointsIsValid = false;
      return of(false);
    } else {
      this.sumOfPointsIsValid = true;
    }

    if (!this.showConfirm) {
      this.entity.confirmed = null;
    }
    if (!this.isCourse) {
      this.entity.confirmed = true;
    }
    if (this.entity.confirmed) {
      this.entity.rejectReason = null;
    }
    this.entity.oldPrice = this.entity.price;


    if (this.productSalesPlanningDescription != null && this.productSalesPlanningDescription.length > 0)
      this.entity.description = this.productSalesPlanningDescription;

    if (this.disableSelectRepresentation)
      this.entity.representationId = null;

    return super.doBeforeSubmit(e);
  }

  doSubmitForm(): void {
    if (this.entity.confirmed && this.hasFinalDiscountPercentageChanged) {
      this.confirmBeforeAction(this.formLabels.finalRegistration, this.formLabels.confirmDiscountPercentage, this.formLabels.description)
        .show().then((dialogResult) => {
        if (dialogResult.res === 'ok') {
          super.doSubmitForm();
        }
      });
    } else {
      super.doSubmitForm();
    }

  }

  getSaleConfiguration(): void {
    this.genericDataService.getEntitiesWithAction('SaleConfiguration', `GetActiveOldSaleConfigurations?companyId=${this.companyId}&representationId=${this.representationId}`, null)
      .subscribe((data: any) => {
          if (data) {
            // this.entity.saleConfigurationDiscountPercentage = data.discountPercentage;
            this.entity.saleConfigurationId = data.id;
          }
        }, () => {

        },
        () => {
          // this.entity.finalDiscountPercentage =
          //   this.getFinalDiscountPercentage(this.entity.discountPercentage, this.entity.saleConfigurationDiscountPercentage);
        }
      );
  }

  // private bindCourseDataSources(): void {
  //     let customListUrl = `GetRepresentationProductsByCategoryId?`;
  //     if (this.companyId) {
  //         customListUrl = `${customListUrl}companyId=${this.companyId}&`;
  //
  //     }
  //     if (this.representationId) {
  //         customListUrl = `${customListUrl}representationId=${this.representationId}&`;
  //         this.representationTeacherCustomListUrl += `?representationId=${this.representationId}&`;
  //     }
  //     if (this.categoryId && this.categoryId !== 0) {
  //         customListUrl = `${customListUrl}categoryId=${this.categoryId}`;
  //     }
  //     if (customListUrl !== '') {
  //         this.courseDataSource = this._genericDataService.createCustomDataSourceWithAction('id', 'product', customListUrl);
  //     }
  // }

  private initialEntityDefaultValues(): void {
    if (this.productId) {
      this.entity.productId = this.productId;
    } else {
      this.entity.product = {};
      this.entity.product.course = {};
      this.entity.product.companyId = this.companyId;
      if (this.categoryId) {
        this.entity.product.categories = [this.categoryId];
      }
    }
    this.entity.product.visibleIndividually = true;
    this.entity.courseStatus = 55;
    this.entity.maxCapacity = 50;
    this.entity.ageCategory = 17;
    this.entity.implementationType = 12;
    this.entity.courseType = 5;
    this.entity.runType = 34;
    this.entity.customGender = 4;
    this.entity.categoryId = this.categoryId;
    this.entity.companyId = this.companyId;
    this.entity.representationId = this.representationId;
    //this.entity.discountPercentage = null;
    this.entity.startDate = +moment().locale('fa').format('YYYYMMDD');
  }

  setProduct(item: any) {
    if (item) {
      this.isProductSelected = true;
      this.productId = item.id;
      this.entity.productId = this.productId;
      this.entity.product = item;
      if (!this.selectedEntityId) {
        if (item.duration) {
          this.entity.duration = item.duration;
        } else {
          this.entity.duration = null;
        }
      }
      this.entity.product.course = {};
      this.entity.product.categories = [item.categories];
      this.entity.product.course.salesModelType = item.salesModelType;
      this.entity.product.course.skillType = item.skillType;
      this.entity.product.course.evaluationMethod = item.evaluationMethod;
      //اگر دوره قابلیت فروش در تمامی نمایندگی ها را داشته باشد
      //دراپ داون انتخاب نمایندگی را غیر فعال می کنیم
      if (item.salesModelType == SaleTypeEnum.SalesInAppRepresentation) {
        this.disableSelectingRepresentationDropDown = true;
        this.entity.representationId = null;
        this.representationId = null;
      }
      this.getSaleConfiguration();
    }
  }

  selectedCategoryChanged(e): void {
    this.entity.product.categories = [e];
  }

  /*
  غیر فعال کردن دکمه خرید
   */
  changeDisableBuyButtonValue(item: any) {
    this.entity.disableBuyButton = !!(item && item.value);
  }

  getCanUserSeeOtherCompanies(): void {
    this.genericDataService.getEntitiesWithAction(`company`, `GetCanUserSeeOtherCompany?companyId=${this.companyId}`, null)
      .subscribe((res: any) => {
        this.canUserSeeOtherCompany = res;
      });
  }

  checkPointsRequirement = (): boolean => {
    if (!this.lmsMode) return true;
    if (!this.usingAbilitySkillLevel) return true;
    if ((this.entity.classPoint === undefined || this.entity.classPoint === null || this.entity.classPoint <= 0) &&
      (this.entity.examPoint === undefined || this.entity.examPoint === null || this.entity.examPoint <= 0) &&
      (this.entity.homeWorkPoint === undefined || this.entity.homeWorkPoint === null || this.entity.homeWorkPoint <= 0)) return false;
    return true;
  };


  validatePointsSum = (): boolean => {
    if (!this.lmsMode) return true;
    if (!this.usingAbilitySkillLevel) return true;
    const sum = (this.entity.classPoint || 0) +
      (this.entity.examPoint || 0) +
      (this.entity.homeWorkPoint || 0);
    return sum <= 100;
  };

  //زمانی که کاربر گزینه اعمال برناهه ریزی دوره برای تمامی نمایدگی ها را تغییر می دهد
  //این متد فراخوانی می شود
  assignProductSalesPlanningToAllRepresentationChanged(selectedOption: any): void {
    this.disableSelectRepresentation = selectedOption.value;
  }

}
