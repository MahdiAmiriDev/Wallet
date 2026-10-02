import {Directive, Input, OnInit, TemplateRef, ViewContainerRef} from "@angular/core";
import {GenericDataService} from "../base/services/generic-data.service";
import {UserDataService} from "../../_services/user-data.service";

/*
این دیرکتیو جهت برسی این که آیا کاربر دسترسی موردنیاز را دارد یا نه استفاده می شود
*/
@Directive({
  selector: '[authorization]'
})

export class AuthorizationDirective implements OnInit {

  @Input()
  public authorization: string;
  @Input()
  public authorizationAppAction: string;
  @Input()
  public authorizationEnable: boolean = true;
  userHasAccessToResource: any;

  constructor(private genericDataService: GenericDataService,
              private templateRef: TemplateRef<any>,
              private viewContainer: ViewContainerRef,
              private userDataService: UserDataService) {
  }

  ngOnInit() {
    this.userHasAccessToResource = this.userDataService.getUserHasAccessToResource();
    this.checkUserHasAccessToResource();
  }

  /*  این متد برسی می کند که آیا کاربر دسترسی موردنیاز را دارد یا نه در صورت دارا بودن دسترسی مقدار true و در غیر این صورت مقدار false در sessionStorage ذخیره می شود */
  checkUserHasAccessToResource(): any {
    if (this.authorizationEnable) {
      if (this.authorization && this.authorizationAppAction) {
        //   زمانی که برای اولین بار متد checkUserHasAccessToResource کال می شود
        //   به دلیل این که در sessionStorage مقداری ذخیره نشده است بنابراین
        //مقدار userHasAccessToResource برابر null است
        if (this.userHasAccessToResource == null || this.userHasAccessToResource) {
          this.genericDataService.getEntitiesWithAction('User', `CheckUserHasPermission?resource=${this.authorization}&appAction=${this.authorizationAppAction}`, null)
            .subscribe((res) => {
              if (res) {
                this.viewContainer.createEmbeddedView(this.templateRef);
                this.userHasAccessToResource = this.userDataService.setUserHasAccessToResource(res);
              } else {
                this.viewContainer.clear();
                this.userHasAccessToResource = this.userDataService.setUserHasAccessToResource(res);
              }
            });
        }
      } else {
        console.log('کاربر گرامی مقادیر resource و appAction ست نشده است');
      }
    } else {
      this.viewContainer.clear();
    }
  }
}
