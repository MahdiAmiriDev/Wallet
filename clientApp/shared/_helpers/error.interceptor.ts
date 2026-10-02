import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {catchError} from 'rxjs/operators';

import {AuthenticationService} from '../../_services';
import {Router, RouterStateSnapshot} from '@angular/router';
import {YashilStatusCode} from "./status-code-enum";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService,
              private router: Router) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(catchError(err => {
      if (err.status === YashilStatusCode.UnAuthorized) {
        // auto logout if 401 response returned from api
        this.authenticationService.logout();
        // location.reload(true);
      }
      // در صورتی کاربر لاک شده است و باید به صفحه ورود کلمه عبور مجدد برود
      if (err.status === YashilStatusCode.InactiveLockUser) {
        this.router.navigate([`pages/checkPassword`], {queryParams: {referrerUrl: this.router.url.toString()}}).then();
      }
      // اگر نیاز به لاگین مجدد باشد به صفحه لاگین میرود
      // این قسمت زمانی انجام میشود که بخواهیم وارد صفحات حساسی شویم
      if (err.status === YashilStatusCode.RequireReLogin) {
        this.router.navigate(['pages/login']).then();
      }
      return throwError(err);
      // const error = err.error.message || err.statusText;
      // return throwError(error);
    }));
  }
}
