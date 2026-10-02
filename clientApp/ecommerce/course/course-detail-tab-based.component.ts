import {Component, Input, OnInit} from '@angular/core';
import {Editable} from '../../../shared/base/classes/editable';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';
import {UserDataService} from "../../../_services/user-data.service";


@Component({
  selector: 'app-course-detail-tab-based',
  templateUrl: './course-detail-tab-based.component.html'
})
export class CourseDetailTabBasedComponent extends Editable implements OnInit {
  courseId: number;
  @Input()
  representationId: number;
  allowEditDesc: boolean;
  tabs: any[] = [];
  companyId: number;
  categoryId: number;
  product: any;
  formLabels: any = {};
  lmsMode: boolean;


  constructor(private translate: TranslateService , private userDataService: UserDataService) {
    super();
    this.entityName = 'product';
    this.lmsMode = this.userDataService.getSiteConfigurationLmsMode();
  }

  ngOnInit(): void {
    if (this.selectedEntityId && this.selectedEntityId !== 0) {
      this.allowEditDesc = true;
    } else {
      this.allowEditDesc = false;
    }

    let result = forkJoin({
      courseTitle: this.translate.get('tms.Course.Title'),
      descriptionTitle: this.translate.get('tms.Course.Description'),
      topicTitle: this.translate.get('tms.Course.Topic'),
      prerequisiteTitle: this.translate.get('tms.Course.Prerequisite'),
      targetTitle: this.translate.get('tms.Course.Target'),
      requirementsTitle: this.translate.get('tms.Course.Requirements'),
      skillTitle: this.translate.get('tms.Course.Skill'),
      audienceTitle: this.translate.get('tms.Course.Audience'),
      iconTitle: this.translate.get('Icon'),
      filmTitle: this.translate.get('Film'),
      planningTitle: this.translate.get('tms.Course.Planning'),
      productTag : this.translate.get('shp.Product.ProductTag')
    });

    result.subscribe(({courseTitle, descriptionTitle, topicTitle, prerequisiteTitle, targetTitle, requirementsTitle, skillTitle, audienceTitle, iconTitle, filmTitle, planningTitle,productTag}) => {

      this.formLabels.title = courseTitle;
      this.formLabels.description = descriptionTitle;
      this.formLabels.topic = topicTitle;
      this.formLabels.prerequisite = prerequisiteTitle;
      this.formLabels.target = targetTitle;
      this.formLabels.requirements = requirementsTitle;
      this.formLabels.skill = skillTitle;
      this.formLabels.audience = audienceTitle;
      this.formLabels.icon = iconTitle;
      this.formLabels.film = filmTitle;
      this.formLabels.planning = planningTitle;
      this.formLabels.productTag=productTag;

      this.bindTabs();
    });


  }

  rowInserted(insertedRowId: any): void {
    this.selectedEntityId = insertedRowId;
    this.allowEditDesc = true;
    this.bindTabs();
  }

  bindTabs(): void {
    if (this.tabs.length === 0) {

      this.tabs = [
        { id: 0, title: this.formLabels.title, template: 'course' },
        { id: 1, title: this.formLabels.description, template: 'description', disabled: !this.allowEditDesc },
        { id: 2, title: this.formLabels.topic, template: 'topic', disabled: !this.allowEditDesc },
        { id: 3, title: this.formLabels.prerequisite, template: 'prerequisite', disabled: !this.allowEditDesc },
        { id: 4, title: this.formLabels.target, template: 'target', disabled: !this.allowEditDesc },
        { id: 5, title: this.formLabels.requirements, template: 'requirements', disabled: !this.allowEditDesc },
        { id: 6, title: this.formLabels.skill, template: 'skill', disabled: !this.allowEditDesc },
        { id: 7, title: this.formLabels.audience, template: 'audience', disabled: !this.allowEditDesc },
        { id: 8, title: this.formLabels.icon, template: 'icon', disabled: !this.allowEditDesc },
        { id: 9, title: this.formLabels.film, template: 'film', disabled: !this.allowEditDesc },
        { id: 10, title: this.formLabels.planning, template: 'coursePlanning', disabled: !this.allowEditDesc }
      ];

      if (!this.lmsMode) {
        this.tabs.push({
          id: 11,
          title: this.formLabels.productTag,
          template: 'productTag',
          disabled: !this.allowEditDesc
        });
      }

    } else {
      this.tabs.forEach(tab => {
        if (tab.id !== 0) {
          tab.disabled = !this.allowEditDesc;
        }
      });
    }
  }


  setProductData(item: any): void {
    this.product = item;
    this.courseId = item.courseId;
  }


}


