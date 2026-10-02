import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {RepresentationDetailTabBasedComponent} from './representation-detail-tab-based.component';
import {CachedDataService} from '../../../shared/services/cached-data.service';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';
import {UserDataService} from '../../../_services/user-data.service';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
    selector: 'app-representation-list',
    templateUrl: './representation-list.component.html'
})
export class RepresentationListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    representationId: number;
    educationalCenterId: number;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    groupList: { value: number, title: string }[] = [];
    entityName = 'representation';
    detailComponent = RepresentationDetailTabBasedComponent;
    private baseListUrl = 'representation/GetByCustomFilterForList?';
    lmsMode: boolean = false;
    showRepresentationCategory:boolean;
    constructor(private cachedDataService: CachedDataService,
                private translate: TranslateService,
                private genericDataService : GenericDataService,
                private userDataService: UserDataService) {
        super();
        this.getGroupTitle = this.getGroupTitle.bind(this);

    }

    // tslint:disable-next-line:variable-name
    _companyId: number;

    get companyId(): number {
        return this._companyId;
    }

    @Input()
    set companyId(val) {
        if (val !== this._companyId) {
            this._companyId = val;
        }
    }

    // tslint:disable-next-line:variable-name
    _founderId: number;

    get founderId(): number {
        return this._founderId;
    }

    @Input()
    set founderId(val) {
        if (val !== this._founderId) {
            this._founderId = val;
        }
    }

    // tslint:disable-next-line:variable-name
    _licenseType: number;

    get licenseType(): number {
        return this._licenseType;
    }

    @Input()
    set licenseType(val) {
        if (val !== this._licenseType) {
            this._licenseType = val;
        }
    }

    // tslint:disable-next-line:variable-name
    _ownershipType: number;

    get ownershipType(): number {
        return this._ownershipType;
    }

    @Input()
    set ownershipType(val) {
        if (val !== this._ownershipType) {
            this._ownershipType = val;
        }
    }

    ngOnInit(): void {
        this.bindColumns();
        this.getDisplayRepresentationCategory();
        this.lmsMode = this.userDataService.getSiteConfigurationLmsMode();
        const data = this.userDataService.getAdditionalUserProp();
        if (data) {
            if (data.educationalCenterId) {
                this.educationalCenterId = data.educationalCenterId;
            }
            if (data.representationId) {
                this.representationId = data.representationId;
            }
        }
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
    }

    bindColumns(): void {
        let result = forkJoin({
            companyTitle: this.translate.get('shp.Company.Title'),
            codeTitle: this.translate.get('Code'),
            title: this.translate.get('Title'),
            stateTitle: this.translate.get('shp.Representation.State'),
            cityIdTitle: this.translate.get('um.Address.CityId'),
            founderIdTitle: this.translate.get('shp.Representation.FounderId'),
            telephoneTitle: this.translate.get('um.Address.Telephone'),
            isCompanyMainRepresentationTitle: this.translate.get('shp.Representation.IsCompanyMainRepresentation'),
            licenseNumberTitle: this.translate.get('shp.Representation.LicenseNumber'),
            licenseExpireTimeTitle: this.translate.get('shp.Representation.LicenseExpireTime'),
            economicCodeTitle: this.translate.get('shp.Representation.EconomicCode'),
            confirmedTitle: this.translate.get('shp.Representation.Confirmed'),
        });

        result.subscribe(({
                              companyTitle, codeTitle, title, stateTitle, cityIdTitle, founderIdTitle,
                              telephoneTitle, isCompanyMainRepresentationTitle, licenseNumberTitle, licenseExpireTimeTitle,
                              economicCodeTitle, confirmedTitle
                          }) => {
            if (!this.companyId) {
                if (!this.companyId) {
                    this.columns.push({
                        caption: companyTitle,
                        dataField: 'companyId',
                        groupIndex: 1,
                        customizeText: this.getGroupTitle,
                        width: '150px'
                    });
                }
            }
            this.columns.push({
                caption: codeTitle,
                dataField: 'code',
                allowGrouping: 0,
                width: '100px',
            }, {
                caption: title,
                dataField: 'title',
                width: '200px',
                allowGrouping: 0
            }, {
                caption: stateTitle,
                dataField: 'stateTitle',
                allowGrouping: 0,
                width: '120px',
            }, {
                caption: cityIdTitle,
                dataField: 'cityTitle',
                allowGrouping: 0,
                width: '180px',
            }, {
                caption: telephoneTitle,
                dataField: 'telephone',
                width: '120px',
                allowGrouping: 0
            });
            if (!this.lmsMode) {
                this.columns.push({
                    caption: founderIdTitle,
                    dataField: 'founderFullName',
                    width: '150px',
                    allowGrouping: 0
                }, {
                    caption: isCompanyMainRepresentationTitle,
                    dataField: 'isCompanyMainRepresentation',
                    allowGrouping: 0,
                    width: '100px',
                }, {
                    caption: licenseNumberTitle,
                    dataField: 'licenseNumber',
                    allowGrouping: 0,
                    width: '80px',
                }, {
                    caption: licenseExpireTimeTitle,
                    dataField: 'licenseExpireTime',
                    allowGrouping: 0,
                    width: '80px'
                }, {
                    caption: economicCodeTitle,
                    dataField: 'economicCode',
                    allowGrouping: 0,
                    width: '80px'
                });
            }

            this.columns.push({
                caption: confirmedTitle,
                dataField: 'confirmed',
                allowGrouping: 0,
                width: '80px',
            });


        });
    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as RepresentationDetailTabBasedComponent);

        if (this.companyId) {
            comp.companyId = this.companyId;
        }

        if (this.founderId) {
            comp.founderId = this.founderId;
        }

        if (this.licenseType) {
            comp.licenseType = this.licenseType;
        }

        if (this.ownershipType) {
            comp.ownershipType = this.ownershipType;
        }
        if(this.showRepresentationCategory)
        {
            comp.showRepresentationCategory = this.showRepresentationCategory;
        }

    }

    public bindCustomDataSources(): boolean {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.companyId) {
                customListUrl += `companyId=${this.companyId}&`;
            }
            if (this.founderId) {
                customListUrl += `founderId=${this.founderId}&`;
            }
            if (this.licenseType) {
                customListUrl += `licenseType=${this.licenseType}&`;
            }
            if (this.ownershipType) {
                customListUrl += `ownershipType=${this.ownershipType}&`;
            }
        }
        let res = false;
        if (this.companyId || this.founderId || this.licenseType || this.ownershipType) {
            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;
    }

    /*
  دریافت عنوان گروه
    */
    getGroupTitle(cellInfo): string {
        const groups = this.groupList.filter(x => x.value === cellInfo.value);
        if (groups.length > 0) {
            return groups[0].title;
        } else {
            const currentRowTitle = this.listForm.gridDataSource.items().filter(x => x.key === cellInfo.value)[0]?.items[0]?.companyTitle;
            if (currentRowTitle) {
                this.groupList.push({value: cellInfo.value, title: currentRowTitle});
                return currentRowTitle;
            }
            return '-';
        }
    }

    getDisplayRepresentationCategory() {
        let key = 'Site.Configuration.AssignCategoryForCompanyAndRepresentationAfterInsert';
        this.genericDataService.getEntitiesWithAction('AppConfig', `GetAppConfigValue?key=${key}`, null)
            .subscribe((res: any) => {
                if (res) {
                    this.showRepresentationCategory = !JSON.parse(res.value.toLowerCase());

                }
            });
    }
}
