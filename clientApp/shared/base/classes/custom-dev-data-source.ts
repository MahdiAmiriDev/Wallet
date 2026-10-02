import DataSource from 'devextreme/data/data_source';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Inject, Input, Directive} from '@angular/core';
import {YashilComponent} from '../../../core/baseClasses/yashilComponent';
import * as AspNetData from "devextreme-aspnet-data-nojquery";

@Directive()
export class CustomDevDataSource extends YashilComponent {
  @Input()
  allowDeleteInForm = true;
  rowKey = 'id';
  // This Property Used For Set Tree List RowKeys
  @Input()
  selectedRowKeys: any[] = [];
  private httpClient: HttpClient;

  constructor(@Inject(HttpClient) httpClient: HttpClient) {
    super();
    this.httpClient = httpClient;
  }

  getCustomDataSourceWithCustomListUrl(customListUrl: string): DataSource {
    return this.getCustomDataSource(null, [], customListUrl);
  }

  getCustomDataSourceAssignedList(entityName, pageFilters: any[], selectedId: number): DataSource {
    return this.getCustomDataSource(entityName, pageFilters, `${entityName}/GetAssignedListAsync?id=${selectedId}`);
  }

  getCustomDataSourceNotAssignedList(entityName, pageFilters: any[], selectedId: number): DataSource {
    return this.getCustomDataSource(entityName, pageFilters, `${entityName}/GetNotAssignedListAsync?id=${selectedId}`);
  }

  getCustomDataSource(entityName, pageFilters: any[], customListUrl: string, customInlineUpdateMethodName: string = null,
                      batchUpdate= false,customParameterForPostMethod: any = null,usePostMethod:boolean = false): DataSource {
    return new DataSource(
      {
        key: 'id',
        store: AspNetData.createStore({
          key: "id",
          loadUrl: customListUrl,
          onBeforeSend: function (method, ajaxOptions) {
            ajaxOptions.xhrFields = { withCredentials: true };
          }
        }),
        load: (loadOptions) => {
          let params: HttpParams = new HttpParams();
          [
            'skip',
            'take',
            'requireTotalCount',
            'requireGroupCount',
            'sort',
            // 'filter',
            'totalSummary',
            'group',
            'groupSummary',
            // tslint:disable-next-line:only-arrow-functions typedef
          ].forEach(i => {
              if (i in loadOptions && (loadOptions[i] && loadOptions[i] !== '')) {
                params = params.set(i, JSON.stringify(loadOptions[i]));
              }
            }
          );
          let rf = [];
          const filterParamKey = 'filter';
          if (loadOptions[filterParamKey] && loadOptions[filterParamKey] !== '') {
            const filters = loadOptions[filterParamKey];
            if (filters[1] === 'and') {
              rf = filters;
            } else {
              rf.push(filters);
            }
          }
          params = params.set('filter', JSON.stringify(rf));
          let cp: any[];
          cp = [];
          for (const filter of pageFilters) {
            cp.push(filter);
          }
          params = params.set('customParams', JSON.stringify(cp));
          let listUrl = entityName;
          if (customListUrl != null) {
            listUrl = customListUrl;
          }
          if(usePostMethod){
            return this.httpClient.post(`/api/${listUrl}`,customParameterForPostMethod,{params})
              .toPromise()
              .then(result => {
                this.afterLoadData(result);
                return {
                  data: result['data'],
                  totalCount: result['totalCount'],
                  summary: result['summary'],
                  groupCount: result['groupCount']
                };
              });

          }else{
            return this.httpClient.get(`/api/${listUrl}`, {params})
              .toPromise()
              .then(result => {
                this.afterLoadData(result);
                return {
                  data: result['data'],
                  totalCount: result['totalCount'],
                  summary: result['summary'],
                  groupCount: result['groupCount'],
                  average:result['average'],
                };
              });
          }
        },
        update: (key: any | string | number, values: any) => {
          if (!batchUpdate){
            const changedProps = Object.keys(values);
            for (let i = 0; i < changedProps.length; i++) {
              changedProps[i] = changedProps[i].charAt(0).toUpperCase() + changedProps[i].slice(1);
            }
            values.id = key;
            const data = {EditModel: values, ChangedProps: changedProps};
            const updateMethodName = customInlineUpdateMethodName ? customInlineUpdateMethodName : 'inlineEditEntity';
            return this.httpClient.post<any>(`/api/${entityName}/${updateMethodName}`, data, {})
              .toPromise().then(res => {
                let isSuccess = false;
                let message = 'بروز خطا در عملیات ذخیره سازی';
                // اگر مقدار بازگشتی resultStatus باشد
                if (typeof res !== 'boolean') {
                  if (res) {
                    if (res.isSuccess) {
                      isSuccess = true;
                      message = 'اطلاعات با موفقیت ذخیره شد';
                    } else {
                      if (res.message) {
                        message = res.message;
                      }
                    }
                  }
                  // اگر مقدار بازگشتی boolean باشد
                } else {
                  if (res) {
                    isSuccess = true;
                    message = 'اطلاعات با موفقیت ذخیره شد';
                  }
                }
                if (isSuccess) {
                  this.showMessage(message);
                } else {
                  this.showError(message);
                }
              });
          }
        }
      }
    );
  }

  // :TODO  This Method Must Be Private and Other Class Cannot change it,but Other Class Maybe Need for  it
  afterLoadData(result): void {
  }

}
