"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CONTENT_ROUTES = void 0;
// Route for content layout without sidebar, navbar and footer for pages like Login, Registration etc...
exports.CONTENT_ROUTES = [
    {
        path: '',
        loadChildren: function () { return Promise.resolve().then(function () { return require('../../modules/content-pages/content-pages.module'); }).then(function (m) { return m.ContentPagesModule; }); }
    },
    {
        path: 'rpt',
        loadChildren: function () { return Promise.resolve().then(function () { return require('../../modules/stimulsoft-report/stimulsoft-report.module'); }).then(function (m) { return m.StimulsoftReportModule; }); }
    },
    {
        path: 'dash',
        loadChildren: function () { return Promise.resolve().then(function () { return require('../../modules/dev-dashboard/dev-dashboard.module'); }).then(function (m) { return m.DevDashboardModule; }); }
    },
    {
        path: 'tms',
        loadChildren: function () { return Promise.resolve().then(function () { return require('../../modules/tms/tms.module'); }).then(function (m) { return m.TmsModule; }); }
    },
    {
        path: 'news',
        loadChildren: function () { return Promise.resolve().then(function () { return require('../../modules/news/news.module'); }).then(function (m) { return m.NewsModule; }); }
    },
    {
        path: 'shp',
        loadChildren: function () { return Promise.resolve().then(function () { return require('../../modules/ecommerce/ecommerce.module'); }).then(function (m) { return m.EcommerceModule; }); }
    },
    {
        path: 'pay',
        loadChildren: function () { return Promise.resolve().then(function () { return require('../../modules/payment/payment.module'); }).then(function (m) { return m.PaymentModule; }); }
    }
];
//# sourceMappingURL=content-layout.routes.js.map