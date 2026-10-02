import {Component, Input, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';
import {UserDataService} from "../../../_services/user-data.service";


@Component({
  selector: 'app-courses-planning-detail-tab-based',
  templateUrl: './courses-planning-detail-tab-based.component.html'
})
export class CoursePlanningDetailTabBasedComponent extends Editable implements OnInit {
  coursePlanningId: number;
  @Input()
  courseId: number;
  companyId: number;
  representationId: number;
  categoryId: number;
  allowEditDesc: boolean;
  tabs: any[] = [];
  @Input()
  showConfirmOnEditForm: boolean;
  @Input()
  isCourse: boolean = false;
  lmsMode: boolean = false;
  productId: number;
  formLabels: any = {};
  productSalesPlanningDescription: string;
  @Input()
  isAdmin: boolean = false;

  constructor(private genericDataService: GenericDataService, private translate: TranslateService, private userDataService: UserDataService) {
    super();
    this.entityName = 'productSalesPlanning';
  }

  ngOnInit(): void {
    if (this.selectedEntityId && this.selectedEntityId !== 0) {
      this.allowEditDesc = true;
    } else {
      this.allowEditDesc = false;
    }

    this.lmsMode = this.userDataService.getSiteConfigurationLmsMode();

    let result = forkJoin({
      planningTitle: this.translate.get('tms.Course.Planning'),
      filesTitle: this.translate.get('tms.Course.Files'),
      imagesTitle: this.translate.get('shp.ProductSalesPlanning.Images'),
      studentsTitle: this.translate.get('tms.Course.Students'),
      metaDescriptionTitle: this.translate.get('shp.Product.MetaDescription'),
      hardwareRequirementTitle: this.translate.get('shp.ProductSalesPlanning.HardwareRequirement'),
      softwareRequirementTitle: this.translate.get('shp.ProductSalesPlanning.SoftwareRequirement'),
      usedForTitle: this.translate.get('shp.ProductSalesPlanning.UsedFor'),
      productSalesPlanningTeacherTitle: this.translate.get('tms.ProductSalesPlanningTeacher.Title'),
      mediaCategoryTitle: this.translate.get('yl.MediaCategory.MediaCategory'),
      itemGroup: this.translate.get('shp.ProductSalesPlanningItemGroup.ItemGroup')
    });

    result.subscribe(({
                        planningTitle,
                        filesTitle,
                        imagesTitle,
                        studentsTitle,
                        metaDescriptionTitle,
                        hardwareRequirementTitle,
                        softwareRequirementTitle,
                        usedForTitle,
                        productSalesPlanningTeacherTitle,
                        mediaCategoryTitle,
                        itemGroup
                      }) => {

      this.formLabels.planning = planningTitle;
      this.formLabels.files = filesTitle;
      this.formLabels.images = imagesTitle;
      this.formLabels.students = studentsTitle;
      this.formLabels.metaDescription = metaDescriptionTitle;
      this.formLabels.hardwareRequirementTitle = hardwareRequirementTitle;
      this.formLabels.softwareRequirementTitle = softwareRequirementTitle;
      this.formLabels.usedForTitle = usedForTitle;
      this.formLabels.productSalesPlanningTeacherTitle = productSalesPlanningTeacherTitle;
      this.formLabels.mediaCategoryTitle = mediaCategoryTitle;
      this.formLabels.itemGroup = itemGroup;
      this.bindTabs();
    });
  }

  rowInserted(insertedRowId: any): void {
    this.selectedEntityId = insertedRowId;
    this.allowEditDesc = true;
    this.bindTabs();
  }

  bindTabs(): void {
    if (this.tabs.length == 0) {
      this.tabs = [
        {id: 0, title: this.formLabels.planning, template: 'coursePlanning'},
        {id: 1, title: this.formLabels.files, template: 'docs', disabled: !this.allowEditDesc},
        {id: 9, title: this.formLabels.images, template: 'images', disabled: !this.allowEditDesc},
        {id: 2, title: this.formLabels.metaDescription, template: 'description', disabled: !this.allowEditDesc},
        {
          id: 4,
          title: this.formLabels.productSalesPlanningTeacherTitle,
          template: 'productSalesPlanningTeacherTemplate',
          disabled: !this.allowEditDesc
        }
      ];
      if (!this.isCourse) {
        this.tabs.push(
          {
            id: 5,
            title: this.formLabels.softwareRequirementTitle,
            template: 'softwareRequirementTemplate',
            disabled: !this.allowEditDesc
          },
          {
            id: 6,
            title: this.formLabels.hardwareRequirementTitle,
            template: 'hardwareRequirementTemplate',
            disabled: !this.allowEditDesc
          },
          {id: 7, title: this.formLabels.usedForTitle, template: 'usedForTemplate', disabled: !this.allowEditDesc},
        );
      } else {
        this.tabs.push(
          {
            id: 8,
            title: this.formLabels.mediaCategoryTitle,
            template: 'mediaSessionsTemplate',
            disabled: !this.allowEditDesc
          });
      }
      if (!this.lmsMode) {
        this.tabs.push(
          {id: 3, title: 'پیش ثبت نام', template: 'preRegistrationTemplate', disabled: !this.allowEditDesc},
          {id: 10, title: 'سفارش های موفق', template: 'completedOrderItems', disabled: !this.allowEditDesc});
      } else {
        this.tabs.push(
          {id: 11, title: this.formLabels.itemGroup, template: 'itemGroupTemplate', disabled: !this.allowEditDesc});
      }
    } else {

      this.tabs.map(x => x.disabled = !this.allowEditDesc);
    }
  }


  setCoursesPlanningId(item: any): void {
    this.coursePlanningId = item.coursePlanningId;
  }

  /*
  بعد از بارگزاری فایل این متد فراخوانی می شود
   */
  onDocUploaded(appDocumentId: any) {}

  onAfterDescriptionUpdated(entity: any) {
    if(entity != null && entity.id != null && entity.description != null) {
      this.productSalesPlanningDescription = entity.description;
    }
  }
}


