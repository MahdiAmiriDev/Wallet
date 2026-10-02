import {Component, Input, OnInit} from '@angular/core';
import {Editable} from '../../../shared/base/classes/editable';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';


@Component({
    selector: 'app-company-detail-tab-based',
    templateUrl: './company-detail-tab-based.component.html'
})
export class CompanyDetailTabBasedComponent extends Editable implements OnInit {
    @Input()
    parentId: number;
    @Input()
    establishedLicenseType: number;

    allowEditDesc: boolean;
    tabs: any[] = [];
    formLabels: any = {};
    @Input()
    showCompanyCategory: boolean;

    constructor(private translate: TranslateService) {
        super();
        this.entityName = 'company';
    }

    ngOnInit(): void {
        this.allowEditDesc = this.selectedEntityId && this.selectedEntityId !== 0;

        let result = forkJoin({
            companyTitle: this.translate.get('shp.Company.Company'),
            companyMainCategoryTitle: this.translate.get('shp.CompanyMainCategory.Title'),
            representationTitle: this.translate.get('shp.Representation.Title'),
            aboutTitle: this.translate.get('shp.Company.About'),
            goalTitle: this.translate.get('shp.Company.Goal'),
            abilityTitle: this.translate.get('shp.Company.Ability'),
            iconTitle: this.translate.get('Icon'),
            logoTitle: this.translate.get('Logo'),
            descriptionTitle: this.translate.get('Description')
        });

        result.subscribe(({
                              companyTitle, companyMainCategoryTitle, representationTitle, aboutTitle,
                              goalTitle, abilityTitle, iconTitle, logoTitle, descriptionTitle
                          }) => {

            this.formLabels.companyTitle = companyTitle;
            this.formLabels.companyMainCategoryTitle = companyMainCategoryTitle;
            this.formLabels.representationTitle = representationTitle;
            this.formLabels.aboutTitle = aboutTitle;
            this.formLabels.goalTitle = goalTitle;
            this.formLabels.abilityTitle = abilityTitle;
            this.formLabels.iconTitle = iconTitle;
            this.formLabels.logoTitle = logoTitle;
            this.formLabels.descriptionTitle = descriptionTitle;

            this.bindTabs();
        });
    }

    rowInserted(insertedRowId: any): void {
        this.selectedEntityId = insertedRowId;
        this.allowEditDesc = true;
        this.bindTabs();
    }

    bindTabs(): void {
        if (this.tabs.length == 0) {
            this.tabs = [
                {id: 0, title: this.formLabels.companyTitle, template: 'company'},
                {id: 2, title: this.formLabels.representationTitle, template: 'representation', disabled: !this.allowEditDesc},
                {id: 3, title: this.formLabels.aboutTitle, template: 'about', disabled: !this.allowEditDesc},
                {id: 4, title: this.formLabels.goalTitle, template: 'goal', disabled: !this.allowEditDesc},
                {id: 5, title: this.formLabels.abilityTitle, template: 'ability', disabled: !this.allowEditDesc},
                {id: 6, title: this.formLabels.iconTitle, template: 'icon', disabled: !this.allowEditDesc},
                {id: 7, title: this.formLabels.logoTitle, template: 'logo', disabled: !this.allowEditDesc},
                {id: 8, title: this.formLabels.descriptionTitle, template: 'description', disabled: !this.allowEditDesc},
            ];
            if (this.showCompanyCategory) {
                this.tabs.push({
                    id: 1,
                    title: this.formLabels.companyMainCategoryTitle,
                    template: 'mainCategories',
                    disabled: !this.allowEditDesc
                });
            }
        } else {
            this.tabs[1].disabled = !this.allowEditDesc;
            this.tabs[2].disabled = !this.allowEditDesc;
            this.tabs[3].disabled = !this.allowEditDesc;
            this.tabs[4].disabled = !this.allowEditDesc;
            this.tabs[5].disabled = !this.allowEditDesc;
            this.tabs[6].disabled = !this.allowEditDesc;
            this.tabs[7].disabled = !this.allowEditDesc;
            this.tabs[8].disabled = !this.allowEditDesc;
        }
    }


}


