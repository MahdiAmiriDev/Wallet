import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {COMPONENTS} from './index';
import {ErrorInterceptor, JwtInterceptor} from '../../shared/_helpers';
import {SharedModule} from '../../shared/shared.module';
import {AngularSplitModule} from 'angular-split';
import {ImageCropperModule} from 'ngx-image-cropper';
import {MessageService} from '../../shared/base/messages/message.service';
import {ECommerceRoutingModule} from './ecommerce-routing.module';
import {DmsModule} from '../dms/dms.module';
import {PdfViewerModule} from 'ng2-pdf-viewer';
import {MatchHeightModule} from '../../shared/directives/match-height.directive';
import {JalaliPipe} from '../../shared/base/core/pipes/jalali.pipe ';
import {NgPersianDatepickerModule} from 'ng-persian-datepicker';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {TmsModule} from "../tms/tms.module";
import {BaseInfoModule} from "../base-info/base-info.module";

export function httpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http, '/api/LocaleStringResource/GetAllAsync?lang=', '');
}

@NgModule({
  declarations: [COMPONENTS],
    imports: [
        CommonModule,
        ECommerceRoutingModule,
        ReactiveFormsModule,
        FormsModule,
        HttpClientModule,
        SharedModule,
        AngularSplitModule.forRoot(),
        ImageCropperModule,
        TranslateModule.forChild({
            loader: {
                provide: TranslateLoader,
                useFactory: httpLoaderFactory,
                deps: [HttpClient]
            },
            defaultLanguage: 'fa',
            isolate: true
        }),
        DmsModule,
        PdfViewerModule,
        MatchHeightModule,
        NgPersianDatepickerModule,
        TmsModule,
        BaseInfoModule
    ],
  providers: [
    JalaliPipe,
    MessageService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor, multi: true
    },
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true}
  ],
  exports: [COMPONENTS]
})
export class EcommerceModule {
}
