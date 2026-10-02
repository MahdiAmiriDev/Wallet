import {Component, Input, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';


@Component({
    selector: 'app-representation-detail-tab-based',
    templateUrl: './representation-detail-tab-based.component.html'
})
export class RepresentationDetailTabBasedComponent extends Editable implements OnInit {
    companyId: number;
    founderId: number;
    licenseType: number;
    ownershipType: number;
    @Input()
    showRepresentationCategory: boolean;

    allowEditDesc: boolean;
    tabs: any[] = [];
    formLabels: any = {};

    constructor(private genericDataService: GenericDataService,
                private translate: TranslateService) {
        super();
        this.entityName = 'representation';
    }

    ngOnInit(): void {
        if (this.selectedEntityId && this.selectedEntityId !== 0) {
            this.allowEditDesc = true;
        } else {
            this.allowEditDesc = false;
        }

        let result = forkJoin({
            representationTitle: this.translate.get('shp.Representation.Title'),
            categoryTitle: this.translate.get('shp.Category.Title'),
            aboutTitle: this.translate.get('shp.Representation.About'),
            goalTitle: this.translate.get('shp.Representation.Goal'),
            descriptionTitle: this.translate.get('Description'),
            abilityTitle: this.translate.get('shp.Representation.Ability')
        });

        result.subscribe(({
                              representationTitle, categoryTitle, aboutTitle, goalTitle,
                              descriptionTitle, abilityTitle
                          }) => {
            this.formLabels.representationTitle = representationTitle;
            this.formLabels.categoryTitle = categoryTitle;
            this.formLabels.aboutTitle = aboutTitle;
            this.formLabels.goalTitle = goalTitle;
            this.formLabels.descriptionTitle = descriptionTitle;
            this.formLabels.abilityTitle = abilityTitle;
        });

        this.bindTabs();
    }

    rowInserted(insertedRowId: any) {
        this.selectedEntityId = insertedRowId;
        this.allowEditDesc = true;
        this.bindTabs();
    }

    bindTabs() {
        if (this.tabs.length == 0) {
            this.tabs = [
                {id: 0, title: this.formLabels.representationTitle, template: 'representation'},
                {id: 2, title: this.formLabels.aboutTitle, template: 'about', disabled: !this.allowEditDesc},
                {id: 3, title: this.formLabels.goalTitle, template: 'goal', disabled: !this.allowEditDesc},
                {id: 4, title: this.formLabels.descriptionTitle, template: 'description', disabled: !this.allowEditDesc},
                {id: 5, title: this.formLabels.abilityTitle, template: 'ability', disabled: !this.allowEditDesc},
            ];
            if (this.showRepresentationCategory) {
                this.tabs.push({id: 1, title: this.formLabels.categoryTitle, template: 'category', disabled: !this.allowEditDesc});
            }

        } else {
            this.tabs[1].disabled = !this.allowEditDesc;
            this.tabs[2].disabled = !this.allowEditDesc;
            this.tabs[3].disabled = !this.allowEditDesc;
            this.tabs[4].disabled = !this.allowEditDesc;
            this.tabs[5].disabled = !this.allowEditDesc;
        }
    }
}


