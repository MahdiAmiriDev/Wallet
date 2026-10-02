import { Component, Input, ViewChild } from '@angular/core';
import { CourseDetailTabBasedComponent } from './course-detail-tab-based.component';
import { BaseList } from '../../../shared/base/classes/base-list';
import { TranslateService } from '@ngx-translate/core';
import { forkJoin } from 'rxjs';


@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html'
})
export class CourseListComponent {
  @ViewChild('frmCourse', { static: true }) frmCourse: BaseList;
  /**
   * Course Category And Education Center Id
   */
  @Input()
  courseInfo: {};
  @Input()
  hideCompanyColumn = false;
  customListUrl: string;
  baseListUrl = 'course/GetByCompanyMainCategoryForList?companyId=';
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'course';
  detailComponent = CourseDetailTabBasedComponent;

  constructor(private translate: TranslateService) {
  }

  // tslint:disable-next-line:variable-name
  _companyId: number;

  get companyId(): number {
    return this._companyId;
  }

  @Input()
  set companyId(value: number) {
    if (this._companyId !== value) {
      this._companyId = value;
      this.frmCourse.customListUrl = `${this.baseListUrl}${this._companyId}`;
      this.frmCourse.refreshList();
    }
  }

  ngOnInit(): void {

    let result = forkJoin({
      title: this.translate.get('tms.Course.Title'),
      categoryLeafTitle: this.translate.get('shp.Category.Leaf'),
      companyTitle: this.translate.get('shp.Company.Title'),
      skillTypeTitle: this.translate.get('tms.Course.SkillType'),
      certificateTypeTitle: this.translate.get('tms.Course.CertificateType'),
      evaluationMethodTitle: this.translate.get('tms.Course.EvaluationMethod'),
      drationTitle: this.translate.get('shp.Product.Duration')

    });

    result.subscribe(({ title, categoryLeafTitle, companyTitle, skillTypeTitle, certificateTypeTitle, evaluationMethodTitle, drationTitle }) => {
      this.columns.push({
        caption: title,
        dataField: 'productTitle'
      });
      this.columns.push({
        caption: categoryLeafTitle,
        dataField: 'categoryTitle'
      });
      if (!this.companyId) {
        this.columns.push({
          caption: companyTitle,
          dataField: 'product.title'
        });
      }
      this.columns.push({
        caption: skillTypeTitle,
        dataField: 'skillTypeTitle'
      });
      this.columns.push({
        caption: certificateTypeTitle,
        dataField: 'certificateTypeTitle'
      });
      this.columns.push({
        caption: evaluationMethodTitle,
        dataField: 'evaluationMethodTitle'
      });
      this.columns.push({
        caption: drationTitle,
        dataField: 'duration'
      });
    })




  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = componentInstance.instance as CourseDetailTabBasedComponent;
    comp.companyId = this.companyId;
  }
}
