import {Directive, Injectable} from '@angular/core';
import {UserDataEnum} from './user-data.enum';
import {CachedDataService} from '../shared/services/cached-data.service';
import {Observable} from "rxjs/Observable";
import {forkJoin} from "rxjs";
import {Cryptography} from "../shared/services/cryptography";

@Directive()
@Injectable({providedIn: 'root'})
export class UserDataService {
  encryptionKey: any;
  public claimData: any;

  constructor(private cachedDataService: CachedDataService,private cryptography: Cryptography) {
  }

  getClaimData(): any {
    if (!this.claimData) {
      const token = this.getCurrentUser(false);
      const payloadHash = JSON.parse(token).token.toString().split('.')[1];
      const payload = JSON.parse(atob(payloadHash));
      this.claimData = payload;
    }
    return this.claimData;
  }

  getCurrentUser(parseJson = true): any {
    return this.cachedDataService.getData(UserDataEnum.CurrentUser, parseJson, true);
  }

  getAdditionalUserProp(parseJson = true): any {
    return this.cachedDataService.getData(UserDataEnum.AdditionalUserProp, parseJson, false);
  }

  getUserResourceAction(parseJson = true): any {
    return this.cachedDataService.getData(UserDataEnum.UserResourceAction, parseJson, false);
  }

  getMenuItems(parseJson = true): any {
    return this.cachedDataService.getData(UserDataEnum.MenuItems, parseJson, false);
  }

  getUserMustChangePassword(parseJson = true): any {
    return this.cachedDataService.getData(UserDataEnum.UserMustChangePassword, parseJson, false);
  }

  getCredentialIdentity(parseJson = true): any {
    return this.cachedDataService.getData(UserDataEnum.CredentialIdentity, parseJson, false);
  }

  getUserAgreementConfirmation(parseJson = true): any {
    return this.cachedDataService.getData(UserDataEnum.UserAgreementConfirmation, parseJson, false);
  }

  getExpiresAt(parseJson = true): any {
    return this.cachedDataService.getData(UserDataEnum.ExpiresAt, parseJson, false);
  }

  setCurrentUser(value: any, isJson = true): void {
    this.cachedDataService.setData(UserDataEnum.CurrentUser, value, isJson, true);
  }

  setAdditionalUserProp(value: any, isJson = true): void {
    this.cachedDataService.setData(UserDataEnum.AdditionalUserProp, value, isJson, false);
  }

  setUserResourceAction(value: any, isJson = true): void {
    this.cachedDataService.setData(UserDataEnum.UserResourceAction, value, isJson, false);
  }

  setMenuItems(value: any, isJson = true): void {
    this.cachedDataService.setData(UserDataEnum.MenuItems, value, isJson, false);
  }


  clearMenuItems(): void {
    this.cachedDataService.clear(UserDataEnum.MenuItems);
  }

  setCurrenApplication(value: any, isJson = true): void {
    this.cachedDataService.setData(UserDataEnum.ApplicationId, value, isJson, false);
  }

  setCredentialIdentity(value: any, isJson = true): void {
    this.cachedDataService.setData(UserDataEnum.CredentialIdentity, value, isJson, false);
  }

  setUserMustChangePassword(value: any, isJson = true): void {
    this.cachedDataService.setData(UserDataEnum.UserMustChangePassword, value, isJson, false);
  }

  setUserAgreementConfirmation(value: any, isJson = true): void {
    if (value) {
      this.cachedDataService.setData(UserDataEnum.UserAgreementConfirmation, value, isJson, false);
    } else {
      this.cachedDataService.clear(UserDataEnum.UserAgreementConfirmation);
    }
  }

  setExpiresAt(value: any, isJson = true): void {
    this.cachedDataService.setData(UserDataEnum.ExpiresAt, value, isJson, false);
  }

  /* دریافت مقدار userHasAccessToResource از حافظه session مرورگر */
  getUserHasAccessToResource(): boolean {
    return  this.cachedDataService.getData('userHasAccessToResource', true, true);
  }

  /* ست کردن مقدار userHasAccessToResource در حافظه session بر اساس این که آیا کاربر دسترسی دارد یا نه */
  setUserHasAccessToResource(value: any): any {
    if (value) {
      this.cachedDataService.setData('userHasAccessToResource', true, false, true);
    } else {
      this.cachedDataService.setData('userHasAccessToResource', false, false, true);
      return this.getUserHasAccessToResource();
    }
  }

  /*ست کردن این که آیا سامانه در حال حاظر در حالت ال ام اس است یا نه*/
  setSiteConfigurationLmsMode(value:any): void{
    value ? this.cachedDataService.setData('lmsMode',value,false,false) : this.cachedDataService.setData('lmsMode',false,true,false);
  }

  /*دریافت این که آیا سامانه در حال حاظر در حالت ال ام اس است یا نه*/
  getSiteConfigurationLmsMode(): boolean{
    return this.cachedDataService.getData('lmsMode',true,false);
  }


  setShareData(key:string,value:any): void{
    value ? this.cachedDataService.setData(key,value,false,false) : this.cachedDataService.setData(key,false,true,false);
  }


  getShareData(key:string): boolean{
    return this.cachedDataService.getData(key,true,false);
  }

  /*دریافت داده ها مورد نیاز بعد از لاگین شدن کاربر*/
   prepareDataAfterLogin(userAdditionalInfo:Observable<any>,userResourceAction:Observable<any>,siteLmsMode:Observable<any>): Observable<any> {
    let result = forkJoin({
      userAdditionalInfo:userAdditionalInfo,
      userResourceAction:userResourceAction,
      siteLmsMode:siteLmsMode,
    });
    result.subscribe(({userAdditionalInfo, userResourceAction,siteLmsMode}) => {
      this.setUserResourceAction(userResourceAction);
      this.setAdditionalUserProp(userAdditionalInfo);
      this.setSiteConfigurationLmsMode(siteLmsMode);
    });
    return result;
  }
}
