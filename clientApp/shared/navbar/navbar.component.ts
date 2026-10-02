import {AfterViewInit, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {LayoutService} from '../services/layout.service';
import {ConfigService} from '../services/config.service';
import {AuthenticationService} from '../../_services';
import {Router} from '@angular/router';
import {GenericDataService} from '../base/services/generic-data.service';
import {UserDataService} from "../../_services/user-data.service";
import {DomSanitizer, SafeHtml} from "@angular/platform-browser";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit, AfterViewInit {
  showPopUp: boolean =false;
  titlePopUp:string ='تغییرات آخرین نسخه سامانه  LMS'
  currentLang = 'en';
  toggleClass = 'ft-maximize';
  placement = 'bottom-left';
  public isCollapsed = true;
  @Output()
  toggleHideSidebar = new EventEmitter<boolean>();
  @Input()
  selectedUser: any = {};
  public config: any = {};
  assignedApplications: any[] = [];
  allowUserSelectApplication = false;
  lmsMode: boolean;
  listOfLastVersion: any = {};
  formattedText: SafeHtml;
  rawHtml:any;

  constructor(private authenticationService: AuthenticationService,
              private router: Router, private layoutService: LayoutService, private configService: ConfigService,
              private genericDataService: GenericDataService,
              private userDataService:UserDataService,
              private sanitizer: DomSanitizer) {

  }

  ngOnInit(): void {
    this.config = this.configService.templateConf;
    this.lmsMode = this.userDataService.getSiteConfigurationLmsMode();
    this.genericDataService.getEntitiesByEntityNameForSelect('UserApplication', 'getCurrentUserApplicationsByCustomFilterForList')
      .subscribe((res: any) => {
        if (res && res?.data?.length > 1) {
          this.allowUserSelectApplication = true;
        }
      });
  }

  //نمایش پاپ آپ تغییرات آخرین نسخه
  showPopup(e):void{
    this.showPopUp = true;
      this.genericDataService.getEntitiesWithAction('VersionInfo', 'LastVersionInfo', null).subscribe(res => {
        if (res) {
          this.listOfLastVersion = res;
          this.rawHtml= this.listOfLastVersion.description;
          this.formattedText = this.formatHtml(this.rawHtml);
        } else {
          this.listOfLastVersion = {};
        }
      });
  }

//برداشته شدن تگ p و قراردادن تگ li در پاپ آپ تغییرات آخرین نسخه
  formatHtml(html: any): SafeHtml {
    const replaced = html
      .replace(/<p>/g, '<li style="border-bottom: 1px solid #dddddd;padding: 0.5rem 0;">') // Remove opening <p>
      .replace(/<\/p>/g, '</li>'); // Replace closing </p> with line break
    return this.sanitizer.bypassSecurityTrustHtml(replaced);
  }


  onClick(e): void {
    this.router.navigate(['pages/setUserApplication']).then();
  }

  ngAfterViewInit(): void {
    if (this.config.layout.dir) {
      const dir = this.config.layout.dir;
      // if (dir === 'rtl') {
      //     this.placement = 'bottom-left';
      // } else if (dir === 'ltr') {
      //     this.placement = 'bottom-right';
      // }
    }
  }


  ChangeLanguage(language: string): void {
  }

  ToggleClass(): void {
    if (this.toggleClass === 'ft-maximize') {
      this.toggleClass = 'ft-minimize';
    } else {
      this.toggleClass = 'ft-maximize';
    }
  }

  toggleNotificationSidebar(): void {
    this.layoutService.emitChange(true);
  }

  toggleSidebar(): void {
    const appSidebar = document.getElementsByClassName('app-sidebar')[0];
    if (appSidebar.classList.contains('hide-sidebar')) {
      this.toggleHideSidebar.emit(false);
    } else {
      this.toggleHideSidebar.emit(true);
    }
  }

  logOut(): void {
    this.authenticationService.logout();
  }

  changePassword(): void {
    this.router.navigate(['pages/changePassword']);
  }
}
