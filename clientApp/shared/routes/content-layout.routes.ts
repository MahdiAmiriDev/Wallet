import {Routes} from '@angular/router';
import {AuthGuard} from "../_guards";

// Route for content layout without sidebar, navbar and footer for pages like Login, Registration etc...
export const CONTENT_ROUTES: Routes = [
  {
    path: '',
    loadChildren: () => import('../../modules/content-pages/content-pages.module').then(m => m.ContentPagesModule)
  },
  {
    path: 'rpt',
    loadChildren: () => import('../../modules/stimulsoft-report/stimulsoft-report.module').then(m => m.StimulsoftReportModule)
  },
  {
    path: 'dash',
    loadChildren: () => import('../../modules/dev-dashboard/dev-dashboard.module').then(m => m.DevDashboardModule)
  },
  {
    path: 'tms',
    loadChildren: () => import('../../modules/tms/tms.module').then(m => m.TmsModule)
  },
  {
    path: 'news',
    loadChildren: () => import('../../modules/news/news.module').then(m => m.NewsModule)
  },
  {
    path: 'shp',
    loadChildren: () => import('../../modules/ecommerce/ecommerce.module').then(m => m.EcommerceModule)
  },
  {
    path: 'pay',
    loadChildren: () => import('../../modules/payment/payment.module').then(m => m.PaymentModule)
  },
  {
    path: 'yl',
    loadChildren: () => import('../../modules/yashil-light/yashil-light.module').then(m => m.YashilLightModule)
  },
  {
    path: 'cor',
    loadChildren: () => import('../../modules/corporation/corporation.module').then(m => m.CorporationModule)
  }
];
