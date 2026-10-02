import {Injectable} from '@angular/core';
import {UserDataService} from '../../_services/user-data.service';

@Injectable({
    providedIn: 'root'
})
export class CachedDataService {
    public claimData: any;

    constructor() {
    }

    // getClaimData(): any {
    //     if (!this.claimData) {
    //         const token = this.userDataService.getCurrentUser(false);
    //         const payloadHash = JSON.parse(token).token.toString().split('.')[1];
    //         const payload = JSON.parse(atob(payloadHash));
    //         this.claimData = payload;
    //     }
    //     return this.claimData;
    // }

    setData(key: string, value: any, isJson = true, setToSessionStorage = true): void {
        if (isJson && isJson === true) {
            setToSessionStorage ? sessionStorage.setItem(key, JSON.stringify(value)) : localStorage.setItem(key, JSON.stringify(value));
        } else {
            setToSessionStorage ? sessionStorage.setItem(key, value) : localStorage.setItem(key, value);
        }
    }

    getData(key: string, parseJson = true, getDataFromSessionStorage = true): any {
        const data = getDataFromSessionStorage ? sessionStorage.getItem(key) : localStorage.getItem(key);
        if (parseJson === true && data) {
            return JSON.parse(data);
        } else {
            return data;
        }
    }

    clear(key: string): void {
        sessionStorage.removeItem(key);
        localStorage.removeItem(key);
    }

    clearAll(): void {
        sessionStorage.clear();
        localStorage.clear();
    }

    // پاک کردن تمام اطلاعات موجود در localStorage
    //و SessionStorage
    clearLocalAndSessionStorage(): void {
        while (localStorage.length > 0) {
            localStorage.removeItem(localStorage.key(localStorage.length - 1));
        }
        while (sessionStorage.length > 0) {
            sessionStorage.removeItem(sessionStorage.key(sessionStorage.length - 1));
        }
    }
}
