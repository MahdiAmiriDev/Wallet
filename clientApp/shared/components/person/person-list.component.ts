import {Component} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';
import {PersonDetailComponent} from './person-detail.component';
import {YashilCustomButtonColumn} from '../../base/classes/yashil-cutom-button-column';


@Component({
    selector: 'app-person-list',
    templateUrl: './person-list.component.html'
})
export class PersonListComponent {
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'person';
    detailComponent = PersonDetailComponent;
    customRowButtons: YashilCustomButtonColumn;
    personId: number;
    showPopUp = false;
    popUpTitle: string;
    personFullName: string;

    constructor(private translate: TranslateService) {

        this.showPersonEconomicProfile = this.showPersonEconomicProfile.bind(this);
    }

    ngOnInit(): void {

        let result = forkJoin({
            nameTitle: this.translate.get('um.Person.Name'),
            lastNameTitle: this.translate.get('um.Person.LastName'),
            fatherNameTitle: this.translate.get('um.Person.FatherName'),
            genderTitle: this.translate.get('um.Person.Gender'),
            birthDateTitle: this.translate.get('um.Person.BirthDate'),
            educationGradeTitle: this.translate.get('um.Person.EducationGrade'),
            emailTitle: this.translate.get('um.Person.Email'),
            nationalCodeTitle: this.translate.get('um.Person.NationalCode'),
            isLegalPersonTitle: this.translate.get('um.Person.IsLegalPerson'),
            phoneNumberTitle: this.translate.get('um.Person.PhoneNumber')
        });

        result.subscribe(({
                              nameTitle, lastNameTitle, fatherNameTitle, genderTitle, birthDateTitle,
                              educationGradeTitle, emailTitle, nationalCodeTitle, isLegalPersonTitle, phoneNumberTitle
                          }) => {

            this.columns.push({
                caption: nameTitle,
                dataField: 'name'
            });
            this.columns.push({
                caption: lastNameTitle,
                dataField: 'lastName'
            });
            this.columns.push({
                caption: fatherNameTitle,
                dataField: 'fatherName'
            });
            this.columns.push({
                caption: genderTitle,
                dataField: 'genderTitle'
            });
            this.columns.push({
                caption: birthDateTitle,
                dataField: 'birthDate',
                cellTemplate: 'intDate'
            });
            this.columns.push({
                caption: phoneNumberTitle,
                dataField: 'phoneNumber'
            });
            this.columns.push({
                caption: educationGradeTitle,
                dataField: 'educationGradeTitle'
            });
            this.columns.push({
                caption: emailTitle,
                dataField: 'email'
            });
            this.columns.push({
                caption: nationalCodeTitle,
                dataField: 'nationalCode'
            });
        });
        this.bindCustomRowButton();
    }

    /*
       ایجاد دکمه نمایش اطلاعات اقتصادی شخص
     */
    bindCustomRowButton() {
        this.customRowButtons = {
            width: '150px',
            customButtons: [
                {
                    hint: 'اطلاعات اقتصادی',
                    text: 'اطلاعات اقتصادی',
                    icon: 'bulletlist',
                    type: 'default',
                    stylingMode: 'contained',
                    onClick: this.showPersonEconomicProfile,
                }]
        };
    }

    /*
    نمایش لیست اطلاعات اقتصادی شخص
     */
    showPersonEconomicProfile(e) {
        this.personId = e.row.data.id;
        this.personFullName = e.row.data.name + ' ' + e.row.data.lastName;
        this.popUpTitle = 'اطلاعات اقتصادی شخص';
        this.showPopUp = true;
    }

}

