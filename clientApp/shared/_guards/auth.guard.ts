import {Injectable} from '@angular/core';
import {
  ActivatedRouteSnapshot, CanActivate, CanLoad, Route,
  Router,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree
} from '@angular/router';
import {AuthenticationService} from '../../_services';
import {GenericDataService} from '../base/services/generic-data.service';
import * as moment from 'moment';
import {CachedDataService} from '../services/cached-data.service';
import {AuthenticationEnum} from '../../_services/authentication.enum';
import {UserDataService} from '../../_services/user-data.service';
import {Observable} from 'rxjs';
import {MenuService} from "../services/menu-service";

@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {
  // tslint:disable-next-line:variable-name
  _isLoggedIn: boolean;
  get isLoggedIn(): boolean {
    return this._isLoggedIn;
  }

  set isLoggedIn(val: boolean) {
    this._isLoggedIn = val;
  }

  constructor(private cachedDataService: CachedDataService,
              private authService: AuthenticationService,
              private menuService: MenuService,
              private router: Router,
              private genericDataService: GenericDataService,
              private userDataService: UserDataService) {
    this.authService.isLoggedIn.subscribe(
      x =>
        this.isLoggedIn = x
    );
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    // checkExpireTime
    if (!moment().isBefore(this.getExpiration())) {
      this.router.navigate(['pages/login']).then();
      return false;
    }
    // check applicationId
    const applicationId = this.userDataService.getCredentialIdentity(false);
    if (!applicationId) {
      this.router.navigate(['pages/setUserApplication']).then();
      return false;
    }
    // check UserAgreement
    const userAgreementConfirmed = this.userDataService.getUserAgreementConfirmation(false);
    if (userAgreementConfirmed && userAgreementConfirmed === '1') {
      this.router.navigate(['pages/userAgreementConfirmation']).then();
      return false;
    }
    // check UserChangePassword
    const userMustChangePassword = this.userDataService.getUserMustChangePassword(false);
    if (userMustChangePassword && userMustChangePassword === 'true') {
      this.router.navigate(['pages/changePassword']).then();
      return false;
    }
    return true;
    // check menu access
    // return this.menuService.getMenuList().map((menuItems: any) => {
    //   const splitUrl = state.url.split('/');
    //   /*
    //   اگر کاربر برای دسترسی به پنل ادمین /tms
    //   را بزند قبول میکنیم
    //    */
    //   if (splitUrl.length == 2) {
    //     return true;
    //   }
    //   const moduleKy = splitUrl[1];
    //   if (moduleKy === "") {
    //     return true;
    //   }
    //   const rootMenu = menuItems.filter(x => x.moduleKey === moduleKy);
    //   if (!rootMenu || rootMenu.length === 0) {
    //     return false;
    //   }
    //   let leafNodes = this.menuService.getLeafNodesOfMenu(menuItems);
    //   if (!leafNodes || leafNodes.length === 0) {
    //     return false;
    //   }
    //   return leafNodes.some(x => x.path == state.url);
    // });
  }

  getExpiration(): any {
    const expiration = localStorage.getItem(AuthenticationEnum.ExpiresAt);
    const expiresAt = JSON.parse(expiration);
    return new Date(expiresAt); // The 0 there is the key, which sets the date to the epoch
  }

}


