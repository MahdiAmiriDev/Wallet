"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Full_ROUTES = void 0;
// Route for content layout with sidebar, navbar and footer
exports.Full_ROUTES = [
    {
        path: '',
        loadChildren: function () { return Promise.resolve().then(function () { return require('../../modules/full-layout-page/full-pages.module'); }).then(function (m) { return m.FullPagesModule; }); }
    }
];
//# sourceMappingURL=full-layout.routes.js.map