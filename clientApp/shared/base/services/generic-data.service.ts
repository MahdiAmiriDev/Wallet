import {EventEmitter, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams, HttpResponse} from '@angular/common/http';
import {from, Observable, of, Subject, throwError} from 'rxjs';
import {catchError, debounceTime, map, tap} from 'rxjs/operators';
import {MessageService} from '../messages/message.service';
import CustomStore from 'devextreme/data/custom_store';
import {createStore} from 'devextreme-aspnet-data-nojquery';
import {CachedDataService} from '../../services/cached-data.service';
import {UserDataService} from '../../../_services/user-data.service';
import {fromArray} from "rxjs-compat/observable/fromArray";


const httpOptions = {headers: new HttpHeaders({'Content-Type': 'application/json'})};
const multiPartHttpOption = {headers: new HttpHeaders({enctype: 'multipart/form-data'})};

@Injectable({
  providedIn: 'root'
})
export class GenericDataService {
  private readonly baseUrl: string;

  constructor(
    private cachedDataService: CachedDataService,
    private httpClient: HttpClient,
    private messageService: MessageService,
    private userDataService: UserDataService,
  ) {
    this.baseUrl = '/api';

  }

  public getEntitiesWithAction(entityName: string, actionName: string, params: HttpParams): Observable<any[]> {
    // const params: HttpParams = new HttpParams();
    if (!params) {
      params = new HttpParams();
    }

    return this.httpClient.get<any[]>(`${this.baseUrl}/${entityName}/${actionName}`, {params})
      .pipe(
        tap(res => this.log('fetched res')),
        catchError(this.handleError('getEntities', []))
      );
  }

  public createCustomDatasource(key: string, entityName: string): any {
    const currentUser = this.userDataService.getCurrentUser();
    return createStore({
      key, loadUrl: `${this.baseUrl}/${entityName}`, onBeforeSend(method, ajaxOptions) {
        ajaxOptions.headers = {
          Authorization: `Bearer ${currentUser.token}`
        };
      }
    });
  }

  public getCommonBaseDataForSelect(commonBaseTypeKey: string) {
    const data = this.cachedDataService.getData(commonBaseTypeKey, true);

    if (data) {
      return of(data);
    }
    const param = new HttpParams().set('keyName', commonBaseTypeKey);
    return this.getCustomEntities('commonBaseData', 'GetByKeyName', param).pipe(tap(res => {
        if (res) {
          this.cachedDataService.setData(commonBaseTypeKey, res);
        } else {
          this.cachedDataService.clear(commonBaseTypeKey);
        }
      }
    ));
  }

  getCustomEntities(entityName, actionName: string, params: HttpParams): Observable<any[]> {
    return this.httpClient.get<any[]>(`${this.baseUrl}/${entityName}/${actionName}`, {params: params})
      .pipe(
        tap(res => this.log('fetched res')),
        catchError(this.handleError('getEntities', []))
      );
  }

  public createCustomDataSourceForSelect(key: string, entityName: string): any {
    return this.createCustomDataSourceWithAction(key, entityName, 'getForSelect');
  }

  public createCustomDataSourceWithCustomUrl(key: string, customUrl: string): CustomStore {
    const currentUser = this.userDataService.getCurrentUser();

    return createStore({
      key, loadUrl: `${this.baseUrl}/${customUrl}`, onBeforeSend(method, ajaxOptions) {
        ajaxOptions.headers = {
          Authorization: `Bearer ${currentUser.token}`
        };
      }
    });
  }

  public createCustomDataSourceWithAction(key: string, entityName: string, actionName: string): any {
    const currentUser = this.userDataService.getCurrentUser();

    return createStore({
      key, loadUrl: `${this.baseUrl}/${entityName}/${actionName}`, onBeforeSend(method, ajaxOptions) {
        ajaxOptions.headers = {
          Authorization: `Bearer ${currentUser.token}`
        };
      }
    });
  }

  public createCustomDataSourceByFilter(key: string, entityName: string, actionName: string, filterMap: Map<string, string>)
    : CustomStore {
    const currentUser = this.userDataService.getCurrentUser();
    return createStore({
      key,
      loadUrl: `${this.baseUrl}/${entityName}/${actionName}`,
      updateUrl: `${this.baseUrl}/${entityName}/${actionName}`,
      onBeforeSend(method, ajaxOptions) {
        filterMap.forEach((value: string, filterKey: string) => {
          ajaxOptions.data[filterKey] = value;
        });

        ajaxOptions.headers = {
          Authorization: `Bearer ${currentUser.token}`
        };
      }
    });
  }

  public getEntitiesByEntityName(entityName: string): Observable<any[]> {
    return this.httpClient.get<any[]>(`${this.baseUrl}/${entityName}`)
      .pipe(tap(res => this.log('fetched res')), catchError(this.handleError('getEntities', [])));
  }

  public getEntitiesByEntityNameForSelect(entityName: string, customSelectMethod: string = 'getForSelect'): Observable<any[]> {
    return this.httpClient.get<any[]>(`${this.baseUrl}/${entityName}/${customSelectMethod}`)
      .pipe(
        tap(res => this.log('fetched res')),
        catchError(this.handleError('getEntities', []))
      );
  }

  /** GET res from the server */

  getCustomEntitiesByUrl(url: string): Observable<any[]> {
    const params: HttpParams = new HttpParams();
    return this.httpClient.get<any[]>(url, {params})
      .pipe(
        tap(res => this.log('fetched res')),
        catchError(this.handleError('getEntities', []))
      );
  }

  /** GET res from the server */
  getEntities(entityName: string): Observable<any[]> {
    return this.httpClient.get<any[]>(`${this.baseUrl}/${entityName}`)
      .pipe(
        tap(res => this.log('fetched res')),
        catchError(this.handleError('getEntities', []))
      );
  }

  /** GET entity by id. Return `undefined` when id not found */
  getEntityNo404<Data>(entityName, id: number): Observable<any> {
    const url = `${this.baseUrl}/${entityName}/?id=${id}`;
    return this.httpClient.get<any[]>(url)
      .pipe(
        map(res => res[0]), // returns a {0|1} element array
        tap(h => {
          const outcome = h ? `fetched` : `did not find`;
          this.log(`${outcome} entity id=${id}`);
        }),
        catchError(this.handleError<any>(`getEntity id=${id}`))
      );
  }

  /** GET entity by id. Will 404 if id not found */
  getEntity(entityName, id: number, customLoadMethodNameWithParams: string = null): Observable<any> {
    const url = customLoadMethodNameWithParams ? `${this.baseUrl}/${entityName}/${customLoadMethodNameWithParams}` : `${this.baseUrl}/${entityName}/${id}`;
    return this.httpClient.get<any>(url).pipe(
      tap(_ => this.log(`fetched entity id=${id}`)),
      catchError(this.handleError<any>(`getEntity id=${id}`))
    );
  }

  /* GET res whose name contains search term */
  searchEntities(entityName, term: string): Observable<any[]> {
    if (!term.trim()) {
      // if not search term, return empty entity array.
      return of([]);
    }
    return this.httpClient.get<any[]>(`${this.baseUrl}/${entityName}/?name=${term}`).pipe(
      tap(_ => this.log(`found res matching "${term}"`)),
      catchError(this.handleError<any[]>('searchEntities', []))
    );
  }

  //////// Save methods //////////
  postEntityByUrl(url: string, entity: any, withFile: boolean = false): Observable<any> {
    let data = entity;
    let option = httpOptions;
    if (entity instanceof FormData) {
      option = multiPartHttpOption;
    }
    if (withFile) {
      const formData = this.mapEntityToFormData(entity);
      data = formData;
      option = multiPartHttpOption;
    }
    return this.httpClient.post<any>(url, data, option)
      .pipe(
        tap((a: any) => this.log(`postEntityByUrl entity w/ id=${entity.id}`)),
        catchError(this.handleError<any>('postEntityByUrl')));
  }
  // با توجه به محدودیت در طول URL در روش httpGet  برای ارسال پارامترهای طولانی و دریافت فایل از این متد استفاده می شود
  postAndDownload(
    url: string,
    entity: any
  ): Observable<{ blob: Blob; fileName: string }> {

    return this.httpClient.post(url, entity, {
      responseType: 'blob',

      observe: 'response'
    }).pipe(
      map(response => {
        const contentDisposition =
          response.headers.get('Content-Disposition');
        let fileName = 'download';
        if (contentDisposition) {
          const match = contentDisposition.match(
            /filename\*?=(?:UTF-8'')?["']?([^;"']+)["']?/i
          );
          if (match?.[1]) {
            fileName = decodeURIComponent(match[1]);
          }
        }
        return {
          blob: response.body!,
          fileName: fileName
        };
      })
    );
  }
  /** POST: add a new entity to the server */
  // اگر فرم دارای فایل باشد که همراه اطلاعات دیگر ذخیره میشود باید مقدار withFile=true باشد
  addEntity(entityName, entity: any, customAddMethodName: string = null, withFile = false): Observable<any> {
    if (!customAddMethodName) {
      customAddMethodName = '';
    }
    let data = entity;
    let options = httpOptions;
    if (withFile) {
      const formData = this.mapEntityToFormData(entity);
      data = formData;
      options = multiPartHttpOption;
    }
    return this.httpClient.post<any>(`${this.baseUrl}/${entityName}/${customAddMethodName}`, data, options);
    /*
        .pipe(
            tap((a: any) => this.log(`added entity w/ id=${entity.id}`)),
            catchError(this.handleError<any>('addEntity')));
     */
  }

  mapEntityToFormData(entity: any): FormData {
    const formData = new FormData();
    Object.keys(entity).forEach(key => {
      if (Array.isArray(entity[key])) {
        {
          Object.keys(entity[key]).forEach(p => {
            formData.append(key, entity[key][p]);
          });
        }
      } else {
        formData.append(key, entity[key]);
      }
    });
    return formData;
  }

  /** DELETE: delete the entity from the server */
  deleteEntity(entityName, entity: any | number, deleteActionName: string): Observable<any> {
    if (!deleteActionName) {
      deleteActionName = 'DeleteEntity';
    }
    const id = typeof entity === 'number' ? entity : entity.id;
    const url = `${this.baseUrl}/${entityName}/${deleteActionName}/${id}`;

    return this.httpClient.post<any>(url, httpOptions).pipe(
      tap(_ => this.log(`deleted entity id=${id}`)),
      catchError(this.handleError<any>('deleteEntity'))
    );
  }

  /** PUT: update the entity on the server */
  // برای فرم هایی که دارای فایل هستند باید withFile=true شود
  updateEntity(entityName, entity: any, customUpdateMethodName: string = null, withFile = false): Observable<any> {
    if (!customUpdateMethodName) {
      customUpdateMethodName = 'UpdateEntity';
    }
    let data = entity;
    let options = httpOptions;
    if (withFile) {
      const formData = this.mapEntityToFormData(entity);
      data = formData;
      options = multiPartHttpOption;
    }
    const postUrl = customUpdateMethodName ? `${this.baseUrl}/${entityName}/${customUpdateMethodName}` : `${this.baseUrl}/${entityName}`;
    return this.httpClient.post(postUrl, data, options).pipe(
      tap(), catchError(this.handleError<any>('updateEntity'))
    );
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T): any {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      // console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      // this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      if (error && error.error) {
        return throwError(error);
      }
      return of(result as T);
      // return of(error.error);
    };
  }

  /** Log a EntityService message with the MessageService */
  private log(message: string): void {
    this.messageService.add(`EntityService: ${message}`);
  }
}
