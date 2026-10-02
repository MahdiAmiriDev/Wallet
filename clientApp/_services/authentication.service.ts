import {Directive, EventEmitter, Injectable, Output} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {map} from 'rxjs/operators';
import {Router} from '@angular/router';
import {CachedDataService} from '../shared/services/cached-data.service';
import {User} from '../_models';
import {ChangePasswordModel} from '../_models/change-password.model';
import {AuthenticationEnum} from './authentication.enum';
import {GenericDataService} from '../shared/base/services/generic-data.service';
import {PasswordModel} from '../_models/password.model';
import {UserDataService} from './user-data.service';
import {Observable} from 'rxjs';
import * as moment from "jalali-moment";
@Directive()
@Injectable({providedIn: 'root'})
// tslint:disable-next-line:directive-class-suffix
export class AuthenticationService {
  @Output() isLoggedIn: EventEmitter<boolean> = new EventEmitter();
  public loggedInLastState = false;

  // tslint:disable-next-line:max-line-length
  constructor(private http: HttpClient, private router: Router, private cachedDataService: CachedDataService, private genericDataService: GenericDataService
    , private userDataService: UserDataService) {
  }

  loginByPassword(us: User): any {
    us.passwordStr = null;
    const formData = this.getFormData(us);
    // قبل از آنکه اقدام به لاگین کنیم باید اطلاعات ذخیره شده را پاک کنیم زیرا ممکن است کاربر دکمه خروج از برنامه را نزده باشد
    return this.login('/api/authentication/authenticate', formData);
  }

  // tslint:disable-next-line:typedef
  loginBySms(us: User): any {
    const formData = this.getFormData(us);
    return this.login('/api/User/authenticateByCodeWithCaptchaAsync', formData);
  }

  // tslint:disable-next-line:typedef
  login(url, dataParams) {
    this.cachedDataService.clearLocalAndSessionStorage();
    return this.genericDataService.postEntityByUrl(url, dataParams).pipe(map(user => {
      // login successful if there's a jwt token in the response
      return this.checkToken(user);
    }));
  }


  checkToken(user: any): any {
    if (user && user.token) {
      // store user details and jwt token in local storage to keep user logged in between page refreshes
      const applicationId = this.storeTokenInClient(user);
      user.applicationId = applicationId;
      this.isLoggedIn.emit(true);
      this.loggedInLastState = true;
    } else {
      this.isLoggedIn.emit(false);
      this.loggedInLastState = false;
    }
    return user;
  }

  renewToken(applicationId: number): any {
    return this.http.post<any>(`/api/authentication/renewTokenWithApplicationId`, {applicationId}).pipe(map(user => {
      // login successful if there's a jwt token in the response
      if (user && user.token) {
        // this.http.get(`antiforgery`);
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        this.storeTokenInClient(user);
        this.userDataService.setCurrenApplication(applicationId.toString(), false);
      }
    }));
  }

  storeTokenInClient(user: any): number {

    this.userDataService.setCurrentUser(user);
    const userData = this.userDataService.getCurrentUser(false);
    const payloadHash = JSON.parse(userData).token.toString().split('.')[1];
    const payload = JSON.parse(atob(payloadHash));
    this.cachedDataService.claimData = payload;
    if (payload.CI) {
      this.userDataService.setCredentialIdentity(payload.CI, false);
    }
    // ست کردن مقدار اینکه کاربر باید رمز عبورش را عوض کند یا نه
    if (payload.UCP) {
      this.userDataService.setUserMustChangePassword(payload.UCP, false);
    }
    // جهت نمایش یا عدم نمایش صفحه توافقنامه کاربران
    if (payload.UAC && payload.UAC === '1' && !user.isReAuthenticated) {
      this.userDataService.setUserAgreementConfirmation(payload.UAC, false);
    }

    const expiresAt = moment().add(+payload.ST, 'minute');
    this.userDataService.setExpiresAt(expiresAt.valueOf());
    // Application Id
    return payload.CI;
  }

  getCredentialHeaders(): any {
    const currentUserToken = this.userDataService.getCurrentUser(false);
    if (currentUserToken) {
      try {
        const currentUser = JSON.parse(currentUserToken);
        if (currentUser && currentUser.token) {
          return {Authorization: `Bearer ${currentUser.token}`};
        }
      } catch (e) {
        console.log('Invalid User Token');
        return {};
      }
    }
  }

  logout(navigate = true): void {
    this.http.post<any>(`/api/authentication/logout`, {}).subscribe(x => {
        this.cachedDataService.clearLocalAndSessionStorage();
        this.isLoggedIn.emit(false);
        if (navigate) {
          this.router.navigate(['pages/login']);
        }
      }
    );
  }

  logoutUser(userId: number): void {
    this.http.post<any>(`/api/authentication/LogOutUser?userId=${userId}`, {}).subscribe(x => {
      }
    );
  }

  changePassword(changePasswordModel: ChangePasswordModel): any {
    const formData = this.getFormData(changePasswordModel);
    return this.http.post<any>(`/api/authentication/changePassword`, formData);
  }

  checkPasswordIsCorrect(passwordModel: PasswordModel): any {
    const formData = this.getFormData(passwordModel);
    return this.http.post<any>(`/api/authentication/checkPasswordIsCorrect`, formData);
  }

  clearChangePasswordFlag(): void {
    this.cachedDataService.clear(AuthenticationEnum.UserMustChangePassword);
  }

  getEncryptionKey(): Observable<any> {
    return this.genericDataService.getCustomEntitiesByUrl(`api/User/GetKeyForEncryption`);
  }

  private getFormData(jsonData: any): FormData {
    const formData = new FormData();
    for (const key in jsonData) {
      formData.append(key, jsonData[key]);
    }
    return formData;
  }

  isUserLogin(){
    return this.genericDataService.getEntity('authentication', null, 'IsUserLogin');
  }

}
