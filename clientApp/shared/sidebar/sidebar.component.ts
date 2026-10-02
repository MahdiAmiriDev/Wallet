import {AfterViewInit, Component, ElementRef, Inject, Input, OnInit, Renderer2, ViewChild} from '@angular/core';
import {customAnimations} from '../animations/custom-animations';
import {ConfigService} from '../services/config.service';
import {
  ActivatedRoute,
  NavigationStart,
  RouteConfigLoadEnd,
  RouteConfigLoadStart,
  Router,
  RouterEvent
} from '@angular/router';
import {GenericDataService} from '../base/services/generic-data.service';
import {RouteInfo} from './sidebar.metadata';
import {YashilComponent} from '../../core/baseClasses/yashilComponent';
import {CachedDataService} from '../services/cached-data.service';
import {MenuService} from "../services/menu-service";
import {UserDataService} from '../../_services/user-data.service';
import {fromEvent} from "rxjs";
import {debounceTime} from "rxjs/operators";
import {SidebarDirective} from "../directives/sidebar.directive";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  animations: customAnimations
})
export class SidebarComponent extends YashilComponent implements OnInit, AfterViewInit {
  @ViewChild('toggleIcon', {static: true}) toggleIcon: ElementRef;
  @ViewChild('searchMenuInput') searchMenuInput: ElementRef;
  @Input() selectedUser: any = {};


  public menuItems: RouteInfo[];
  public config: any = {};
  public isShowingRouteLoadIndicator: boolean;
  depth: number;
  activeTitles: string[] = [];
  activeTitleSearchBox: string[] = [];
  expanded: boolean;
  navCollapsedOpen = false;
  logoUrl = 'assets/img/logo.png';
  showSearchMenuInput: boolean = true;
  showSearchMenuBox: boolean = false;
  helpButtonPadding : number = 20;
  filteredMenuItems: RouteInfo[];
  sidebarDirective: SidebarDirective;

  constructor(
    private elementRef: ElementRef,
    private renderer: Renderer2,
    private router: Router,
    private route: ActivatedRoute,
    private configService: ConfigService,
    private genericDataService: GenericDataService,
    private cachedDataService: CachedDataService,
    private menuService: MenuService,
    private userDataService: UserDataService,
    @Inject(SidebarDirective) sidebar: SidebarDirective,
  ) {
    super();
    this.menuService.getMenuList().subscribe(res => {
      this.menuItems = res;
    });
    if (this.depth === undefined) {
      this.depth = 0;
      this.expanded = true;
    }
    this.isShowingRouteLoadIndicator = false;
    this.sidebarDirective = sidebar;

    // As the router loads modules asynchronously (via loadChildren), we're going to
    // keep track of how many asynchronous requests are currently active. If there is
    // at least one pending load request, we'll show the indicator.
    let asyncLoadCount = 0;
    router.events.subscribe(
      (event: RouterEvent): void => {
        if (event instanceof RouteConfigLoadStart) {

          this.setBusy(true);
          asyncLoadCount++;

        } else if (event instanceof RouteConfigLoadEnd) {

          asyncLoadCount--;
          this.setBusy(false);
        }

        if (event instanceof NavigationStart) {
          this.showSearchMenuBox = false;
          this.searchMenuInput.nativeElement.value = '';
        }
        // If there is at least one pending asynchronous config load request,
        // then let's show the loading indicator.
        // --
        // CAUTION: I'm using CSS to include a small delay such that this loading
        // indicator won't be seen by people with sufficiently fast connections.
        this.isShowingRouteLoadIndicator = !!asyncLoadCount;

      }
    );
  }


  ngOnInit(): void {
    this.config = this.configService.templateConf;


    if (this.config.layout.sidebar.backgroundColor === 'white') {
      this.logoUrl = 'assets/img/logo-dark.png';
    } else {
      this.logoUrl = 'assets/img/logo.png';
    }

    /* اگر کاربر در خارج از کادر filterMenuBox کلیک کند نمایش کادر را مخفی می کنیم */
    window.addEventListener('click', (e: any) => {
      let filterMenuBox = document.getElementById('filterMenuBox');
      if (filterMenuBox != null && !filterMenuBox.contains(e.target)) {
        this.showSearchMenuBox = false;
      }
    });

    /*اگر کاربر در داخل  کادر searchMenuInput کلیک کند و اگر دارای مقدار وارد شده
     از قبل جهت فیلتر باشیم کادر فیلتر بر اساس  مقادیر قبلی نمایش داده می شود */
    window.addEventListener('click', (e: any) => {
      let searchInput = document.getElementById('searchMenuInput');
      if (searchInput != null && searchInput.contains(e.target) && this.searchMenuInput.nativeElement.value.length > 0) {
        this.showSearchMenuBox = true;
      }
    });

  }

  ngAfterViewInit(): void {

    setTimeout(() => {
      if (this.config.layout.sidebar.collapsed !== undefined) {
        if (this.config.layout.sidebar.collapsed === true) {
          this.expanded = false;
          this.renderer.addClass(this.toggleIcon.nativeElement, 'ft-toggle-left');
          this.renderer.removeClass(this.toggleIcon.nativeElement, 'ft-toggle-right');
          this.navCollapsedOpen = true;
        } else if (this.config.layout.sidebar.collapsed === false) {
          this.expanded = true;
          this.renderer.removeClass(this.toggleIcon.nativeElement, 'ft-toggle-left');
          this.renderer.addClass(this.toggleIcon.nativeElement, 'ft-toggle-right');
          this.navCollapsedOpen = false;
        }
      }
    }, 0);

    this.getKeyForSearchInMenuItems();
  }


  toggleSlideInOut(): void {
    this.expanded = !this.expanded;
  }

  handleToggle(titles): void {
    this.activeTitles = titles;
  }

  //دریافت عنوان منوی کلیک شده در سرچ باکس
  handleToggleForSearchBox(title): void {
    this.activeTitleSearchBox = title;
  }

  // NGX Wizard - skip url change
  ngxWizardFunction(path: string): void {
    if (path.indexOf('forms/ngx') !== -1) {
      this.router.navigate(['forms/ngx/wizard'], {skipLocationChange: false});
    }
  }

  /* این متد بر اساس ورودی کاربر عملیات جست و جو را با تاخییر 500 میلی ثانیه بعد از
   آخرین کلیک کاربر بر روی صفحه کلید شروع می کند .
   اگر مقدار ورودی خالی شد کادر جستجو را غیر فعال می کند  */
  getKeyForSearchInMenuItems() {
    fromEvent(this.searchMenuInput.nativeElement, 'keyup').pipe(debounceTime(500))
      .subscribe((res: any) => {
        if (res.target.value.length > 0) {
          this.showSearchMenuBox = true;
          this.searchMenuItemsByKey(res.target.value);
        } else if (res.target.value.length == 0) {
          this.showSearchMenuBox = false;
        }
      });
  }

  /* عملیات جستجو در سطوح مختلف را با توجه به مقدار وارد شده توسط کاربر جهت جستجو شروع می کند */
  searchMenuItemsByKey(key: any) {
    let searchInSecondLeve: any;
    let searchInThirdLeve: any;
    let firstLeveSearchResult: any[] = [];
    let secondLeveSearchResult: any[] = [];
    let thirdLeveSearchResult: any[] = [];

    this.menuItems.map(x => {
      if (firstLeveSearchResult.length == 0) {
        firstLeveSearchResult = this.menuItems.filter(x => x.title.includes(`${key}`));
      }
      searchInSecondLeve = x.submenu.filter(x => x.title.includes(`${key}`));
      if (searchInSecondLeve.length > 0) {
        secondLeveSearchResult = secondLeveSearchResult.concat(searchInSecondLeve);
      }
      searchInThirdLeve = x.submenu.map(i => i.submenu.filter(i => i.title.includes(`${key}`)))
      if (searchInThirdLeve.length > 0) {
        searchInThirdLeve.forEach(x => {
          if (x.length > 0) {
            thirdLeveSearchResult = thirdLeveSearchResult.concat(x);
          }});
      }});

    this.filteredMenuItems = firstLeveSearchResult.concat(thirdLeveSearchResult, secondLeveSearchResult);
  }

  //زمانی که کاربر بر روی یکی از ایتم های جستجو شده در سرچ باکس کلیک کند این متد فراخوانی می شود
  //منوی انتخاب شده در سرچ باکس را در منوی اصلی سایت فعال می کند
  onSearchBoxMenuItemSelect(menuItem: any) {
    this.sidebarDirective.searchBoxActiveRoute = menuItem.routePath;
    this.sidebarDirective.activeLinks = [];
    this.sidebarDirective.expandActiveMenuItemForSearchBox();
  }

  onHelpClick(){}
}
