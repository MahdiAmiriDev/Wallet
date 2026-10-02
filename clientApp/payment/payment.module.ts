import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {COMPONENTS} from './index';
import {ErrorInterceptor, JwtInterceptor} from '../../shared/_helpers';
import {SharedModule} from '../../shared/shared.module';
import {AngularSplitModule} from 'angular-split';
import {ImageCropperModule} from 'ngx-image-cropper';
import {MessageService} from '../../shared/base/messages/message.service';
import {PaymentRoutingModule} from './payment-routing.module';
import {DmsModule} from "../dms/dms.module";
import {NgPersianDatepickerModule} from 'ng-persian-datepicker';
import {YashilLightModule} from "../yashil-light/yashil-light.module";

@NgModule({
    declarations: [COMPONENTS],
  imports: [
    CommonModule,
    PaymentRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    SharedModule,
    AngularSplitModule.forRoot(),
    ImageCropperModule,
    DmsModule,
    NgPersianDatepickerModule,
    YashilLightModule
  ],
    providers: [
        MessageService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: JwtInterceptor, multi: true
        },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
    ],
    exports: [COMPONENTS]
})
export class PaymentModule {
}
