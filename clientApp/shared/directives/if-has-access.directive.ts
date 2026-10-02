import {Directive, Input, OnInit, TemplateRef, ViewContainerRef} from '@angular/core';
import {UserDataService} from "../../_services/user-data.service";
import {HttpClient} from "@angular/common/http";

@Directive({
  selector: '[ifHasAccess]'
})

/*این دیرکتیو برسی می کند که اگر کانفیگ ال ام اس مود برابر true باشد
* فایلی از جنس json را دریافت می کند  که شامل انتیتی ها و پروپرتی های آن انتیتی است. که باید در
* حالتی که  کانفیگ ال ام اس مود true است: برای ایتم فرمی که این دیرکتیو در آن استفاده شده نمایش داده نشود
* تمامی این فرآیند در حالتی که کانفیگ ال ام اس مود true باشد رخ می دهد در غیر این صورت فرآیندی برسی نمی شود  */

export class IfHasAccessDirective implements OnInit {

  //جهت دریافت نام انتیتی
  @Input() public ifHasAccess: string;
  //جهت دریافت نام پروپرتی مربوط به انتیتی
  @Input() public ifHasAccessPropertyName: string;
  //جهت دریافت فایل جی سون مربوط به انتیتی ها و پروپرتی های آن
  entityIgnoredProps: any[] = [];
  isLmsMode: boolean;


  constructor(private viewContainer: ViewContainerRef,
              private templateRef: TemplateRef<any>,
              private userDataService: UserDataService,
              private http: HttpClient) {

  }

  ngOnInit() {
    //دریافت کانفیگ مربوط به این که آیا سایت در مود ال ام اس است یا نه
    this.isLmsMode = this.userDataService.getSiteConfigurationLmsMode();

    //دریافت فایل جی سون پروپرتی های و انتیتی های آن در صورتی که ال ام اس مود true باشد و سپس برسی این که آیا دسترسی به
    // انتیتی و پروپرتی دریافتی وجود دارد یا نه
    if (this.isLmsMode) {
      this.http.get('../../../assets/data/entityPropsAccess.json')
        .subscribe((res: any) => {
          this.entityIgnoredProps = res.data;
          this.checkEntityPropertiesAccess();
        });
    } else {
      this.viewContainer.createEmbeddedView(this.templateRef);
    }
  }

  //برسی می کند که آیا نام انتیتی ارسال شده در فایل جی سون انتیتی ها و پروپرتی های ایگنور شده وجود دارد یا نه
  //اگر نام انتیتی در لیست وجود نداشته باشد فیلد نمایش داده می شود
  //اگر نام انتیتی در فایل جی سون بود سپس پروپرتی آن انتیتی برسی می شود
  //اگر نام پروپرتی در فایل جی سون وجود داشت فیلد نمایش داده نمی شود در غیر این صورت فیلد را نمایش می دهیم
  checkEntityPropertiesAccess(): void {
    let entity = this.entityIgnoredProps.find((x) => x.entityName === this.ifHasAccess);
    if (entity && entity.ignoreProps) {
      let existsPropertyInIgnoreList = entity.ignoreProps.includes((this.ifHasAccessPropertyName));
      if (existsPropertyInIgnoreList) {
        this.viewContainer.clear();
      } else {
        this.viewContainer.createEmbeddedView(this.templateRef);
      }
    } else {
      this.viewContainer.createEmbeddedView(this.templateRef);
    }
  }

}
