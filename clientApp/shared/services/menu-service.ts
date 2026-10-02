import {Injectable} from '@angular/core';
import {EMPTY, from, Observable, of, Subject} from 'rxjs';
import {GenericDataService} from "../base/services/generic-data.service";
import {CachedDataService} from "./cached-data.service";
import {exhaustMap, switchMap} from "rxjs/operators";
import {UserDataService} from '../../_services/user-data.service';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  private menu$: Observable<any> = of(1);
  private userMenu$: Observable<any>;

  constructor(private genericDataService: GenericDataService,
              private cachedDataService: CachedDataService,
              private userDataService: UserDataService) {
    this.userMenu$ = this.menu$.pipe(
      exhaustMap((x) =>
        this.genericDataService.getEntitiesWithAction('Menu', 'UserMenu', null)
      )
    );
  }

  getMenuList(): Observable<any> {
    const menuItems = this.userDataService.getMenuItems();
    if (menuItems) {
      return new Observable(o => o.next(menuItems));
    } else {
      this.userMenu$.subscribe((res: any) => {
        this.userDataService.setMenuItems(res);
      });
      return this.userMenu$;
    }

  }

  /*
  دریافت نودهای برگ
   */
  getLeafNodesOfMenu(nodes, result = []) {
    for (let i = 0, length = nodes.length; i < length; i++) {
      if (!nodes[i].submenu || nodes[i].submenu.length === 0) {
        result.push(nodes[i]);
      } else {
        result = this.getLeafNodesOfMenu(nodes[i].submenu, result);
      }
    }
    return result;
  }

  refreshMenuList() {

  }
}
