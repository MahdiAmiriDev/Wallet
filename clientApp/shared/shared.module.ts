import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
// COMPONENTS
import {FooterComponent} from './footer/footer.component';
import {NavbarComponent} from './navbar/navbar.component';
import {SidebarComponent} from './sidebar/sidebar.component';
// DIRECTIVES
import {ToggleFullscreenDirective} from './directives/toggle-fullscreen.directive';
import {SidebarDirective} from './directives/sidebar.directive';
import {SidebarLinkDirective} from './directives/sidebarlink.directive';
import {SidebarListDirective} from './directives/sidebarlist.directive';
import {SidebarAnchorToggleDirective} from './directives/sidebaranchortoggle.directive';
import {SidebarToggleDirective} from './directives/sidebartoggle.directive';
import {BaseListFormComponent} from './base/base-list-form/base-list-form.component';
import {BaseEditFormComponent} from './base/base-edit-form/base-edit-form.component';
import {DxDataGridModule} from 'devextreme-angular/ui/data-grid';
import {DxButtonModule} from 'devextreme-angular/ui/button';
import {DxToastModule} from 'devextreme-angular/ui/toast';
import {DxCheckBoxModule} from 'devextreme-angular/ui/check-box';
import {DxFileUploaderModule} from 'devextreme-angular/ui/file-uploader';
import {DxFormModule} from 'devextreme-angular/ui/form';
import {DxLookupModule} from 'devextreme-angular/ui/lookup';
import {DxColorBoxModule} from 'devextreme-angular/ui/color-box';
import {DxNumberBoxModule} from 'devextreme-angular/ui/number-box';
import {DxPopupModule} from 'devextreme-angular/ui/popup';
import {DxScrollViewModule} from 'devextreme-angular/ui/scroll-view';
import {DxSelectBoxModule} from 'devextreme-angular/ui/select-box';
import {DxTagBoxModule} from 'devextreme-angular/ui/tag-box';
import {DxTreeListModule} from 'devextreme-angular/ui/tree-list';
import {DxTextAreaModule} from 'devextreme-angular/ui/text-area';
import {DxTabPanelModule} from 'devextreme-angular/ui/tab-panel';
import {DxListModule} from 'devextreme-angular/ui/list';
import {DxValidatorModule} from 'devextreme-angular/ui/validator';
import {DxValidationSummaryModule} from 'devextreme-angular/ui/validation-summary';
import {DxHtmlEditorModule} from 'devextreme-angular/ui/html-editor';
import {MessagesComponent} from './base/messages/messages.component';
import {SecuredImageComponent} from './base/core/components/secured-image/secured-image.component';
import {Base64imageDirective} from './directives/base64image.directive';
import {DetailComponentDirective} from './base/base-list-form/detail-component.directive';
import {IntToDateTimePipe} from './base/core/pipes/int-to-date-time.pipe';
import {IntToStringDatePipe} from './base/core/pipes/Int-to-string-date.pipe';
import {JalaliPipe} from './base/core/pipes/jalali.pipe ';
import {IntToStringTimePipe} from './base/core/pipes/int-to-string-time.pipe';
import {PersianDayPipe} from './base/core/pipes/persian-day.pipe';
import {FullLayoutComponent} from '../layouts/full/full-layout.component';
import {FullLayoutSplitComponent} from '../layouts/fullsplit/full-layout-split.component';
import {AngularSplitModule} from 'angular-split';
import {MessageService} from './base/messages/message.service';
import {HTTP_INTERCEPTORS} from '@angular/common/http';
import {ErrorInterceptor, JwtInterceptor} from './_helpers';
import {DynamicScriptLoaderService} from './services/dynamic-script-loader.service';
import {CoreModule} from '../core/core.module';
import {AssignableListComponent} from './base/core/components/assignable-list/assignable-list.component';
import {DxButtonGroupModule} from 'devextreme-angular/ui/button-group';
import {PersonListComponent} from './components/person/person-list.component';
import {PersonDetailComponent} from './components/person/person-detail.component';
import {PersonSelectComponent} from './components/person/person-select/person-select.component';
import {DxDropDownBoxModule} from 'devextreme-angular/ui/drop-down-box';
import {DxTextBoxModule} from 'devextreme-angular/ui/text-box';
import {DxTreeViewModule} from 'devextreme-angular/ui/tree-view';
import {ContentEditorComponent} from './components/content-editor/content-editor.component';
import {AddressDetailComponent} from './components/address/address-detail.component';
import {CitySelectComponent} from './components/person/city-select/city-select.component';
import {NgbCollapseModule, NgbDropdownModule} from '@ng-bootstrap/ng-bootstrap';
import {AddressListComponent} from './components/address/address-list.component';
import {DxBoxModule} from 'devextreme-angular/ui/box';
import {DxValidationGroupModule} from 'devextreme-angular/ui/validation-group';
import {DxRadioGroupModule} from 'devextreme-angular/ui/radio-group';
import {JalaliDateWithTimePipe} from './base/core/pipes/jalali-with-time.pipe ';
import {TranslateModule} from '@ngx-translate/core';
import {NgPersianDatepickerModule} from 'ng-persian-datepicker';
import {TimePickerComponent} from "./components/time-picker/time-picker.component";
import {UserSelectComponent} from "./components/user/user-select.component";
import {AuthorizationDirective} from "./directives/authorization.directive";
import {RoleResourceActionDetailComponent} from "./components/role-resource-action/role-resource-action-detail.component";
import {RoleResourceActionListComponent} from "./components/role-resource-action/role-resource-action-list.component";
import {ResourceAppActionDetailComponent} from "./components/resource-app-action/resource-app-action-detail.component";
import {ResourceAppActionListComponent} from "./components/resource-app-action/resource-app-action-list.component";
import {CompanySelectComponent} from "./components/company/company-select.component";
import {IfHasAccessDirective} from "./directives/if-has-access.directive";
import {UserGroupSelectComponent} from './components/user-group/user-group-select.component';
import {HideButtonPipe} from './base/core/pipes/hide-button.pipe';
import {CommonBaseDataDetailComponent} from "./components/common-base-data/common-base-data-detail.component";
import {PersonEconomicProfileListComponent} from './components/person/person-economic-profile/person-economic-profile-list.component';
import {PersonEconomicProfileDetailComponent} from './components/person/person-economic-profile/person-economic-profile-detail.component';
import {PersonSearchComponent} from './components/person/person-search/person-search.component';
import {RepresentationFilterComponent} from "./components/representation-filter/representation-filter.component";


@NgModule({
  exports: [
    TranslateModule,
    AddressListComponent,
    PersonListComponent,
    PersonDetailComponent,
    CoreModule,
    FullLayoutComponent,
    FullLayoutSplitComponent,
    IntToDateTimePipe,
    IntToStringDatePipe,
    IntToStringTimePipe,
    PersianDayPipe,
    JalaliPipe,
    JalaliDateWithTimePipe,
    HideButtonPipe,
    CommonModule,
    FooterComponent,
    NavbarComponent,
    DxToastModule,
    DxDropDownBoxModule,
    DxTextBoxModule,
    DxTreeViewModule,
    SidebarComponent,
    ToggleFullscreenDirective,
    SidebarDirective,
    BaseListFormComponent,
    BaseEditFormComponent,
    AssignableListComponent,
    SecuredImageComponent,
    DxLookupModule,
    DxColorBoxModule,
    DxScrollViewModule,
    DxDataGridModule,
    DxCheckBoxModule,
    DxSelectBoxModule,
    DxNumberBoxModule,
    DxFormModule,
    DxButtonModule,
    DxPopupModule,
    DxTagBoxModule,
    DxRadioGroupModule,
    DxFileUploaderModule,
    DxTreeListModule,
    DxTextAreaModule,
    DxTabPanelModule,
    DxValidatorModule,
    DxBoxModule,
    DxValidationSummaryModule,
    Base64imageDirective,
    DetailComponentDirective,
    DxListModule,
    DxHtmlEditorModule,
    PersonSelectComponent,
    CitySelectComponent,
    ContentEditorComponent,
    AddressDetailComponent,
    TimePickerComponent,
    UserSelectComponent,
    AuthorizationDirective,
    CompanySelectComponent,
    IfHasAccessDirective,
    UserGroupSelectComponent,
    PersonEconomicProfileListComponent ,
    PersonEconomicProfileDetailComponent,
    PersonSearchComponent,
    RepresentationFilterComponent
  ],
    imports: [
        TranslateModule.forRoot(),
        DxToastModule,
        DxHtmlEditorModule,
        DxDropDownBoxModule,
        DxTextBoxModule,
        DxTreeViewModule,
        DxLookupModule,
        DxScrollViewModule,
        DxDataGridModule,
        DxColorBoxModule,
        DxCheckBoxModule,
        DxSelectBoxModule,
        DxNumberBoxModule,
        DxFormModule,
        DxButtonModule,
        DxPopupModule,
        DxRadioGroupModule,
        NgPersianDatepickerModule,
        DxFileUploaderModule,
        DxTreeListModule,
        DxTagBoxModule,
        DxTextAreaModule,
        DxTabPanelModule,
        DxListModule,
        DxValidatorModule,
        DxValidationSummaryModule,
        AngularSplitModule,
        CoreModule,
        DxButtonGroupModule,
        NgbDropdownModule,
        NgbCollapseModule,
        DxValidationGroupModule,

    ],
  declarations: [
    AddressListComponent,
    PersonListComponent,
    PersonDetailComponent,
    ContentEditorComponent,
    AssignableListComponent,
    PersonDetailComponent,
    PersonSelectComponent,
    CitySelectComponent,
    FooterComponent,
    NavbarComponent,
    SidebarComponent,
    ToggleFullscreenDirective,
    SidebarDirective,
    SidebarLinkDirective,
    SidebarListDirective,
    SidebarAnchorToggleDirective,
    SidebarToggleDirective,
    Base64imageDirective,
    BaseListFormComponent,
    BaseEditFormComponent,
    MessagesComponent,
    SecuredImageComponent,
    Base64imageDirective,
    DetailComponentDirective,
    IntToDateTimePipe,
    IntToStringDatePipe,
    JalaliPipe,
    JalaliDateWithTimePipe,
    IntToStringTimePipe,
    PersianDayPipe,
    HideButtonPipe,
    FullLayoutComponent,
    FullLayoutSplitComponent,
    AddressDetailComponent,
    TimePickerComponent,
    UserSelectComponent,
    AuthorizationDirective,
    RoleResourceActionDetailComponent,
    RoleResourceActionListComponent,
    ResourceAppActionDetailComponent,
    ResourceAppActionListComponent,
    CompanySelectComponent,
    IfHasAccessDirective,
    UserGroupSelectComponent,
    CommonBaseDataDetailComponent,
    PersonEconomicProfileListComponent,
    PersonEconomicProfileDetailComponent,
    PersonSearchComponent,
    RepresentationFilterComponent
  ],
  providers: [
    DynamicScriptLoaderService,
    MessageService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor, multi: true
    },
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true}
  ]
})
export class SharedModule {
}
