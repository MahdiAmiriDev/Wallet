import {Component, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import notify from 'devextreme/ui/notify';
import {confirm} from 'devextreme/ui/dialog';

@Component({
  selector: 'app-wallet-program-detail',
  templateUrl: './wallet-program-detail.component.html'
})
export class WalletProgramDetailComponent extends BaseEdit implements OnInit {
  programTypes: any[] = [];
  allocationMethods: any[] = [];
  readonly status = {draft: 3168, active: 3169, suspended: 3170, expired: 3171, closed: 3172};
  readonly scopeTypes = [
    {id: 3173, title: 'مرکز', field: 'companyId'},
    {id: 3174, title: 'نمایندگی', field: 'representationId'},
    {id: 3175, title: 'شهر', field: 'cityId'},
    {id: 3176, title: 'محصول', field: 'productId'}
  ];
  scopes: any;
  scopeEditor: any = null;
  scopeTargets: any = {};
  assignments: any;
  assignmentUsers: any;
  assignmentEditor: any = null;
  grants: any;
  grantEditor: any = null;
  auditResult: any = null;
  auditLoading = false;
  cardGroups: any;
  cardMembers: any;
  selectedCardGroup: any = null;
  cardGroupEditor: any = null;
  cardNumber = '';
  couponBatches: any;
  selectedCouponBatch: any = null;
  couponBatchEditor: any = null;
  issuedCodes: string[] = [];
  issuedBatchId: number = null;
  batchCreating = false;
  readonly assignmentStatuses = [
    {id: 3177, title: 'فعال'}, {id: 3178, title: 'متوقف'}, {id: 3179, title: 'منقضی'}
  ];
  readonly assignmentSources = [
    {id: 3180, title: 'دستی'}, {id: 3181, title: 'سازمانی'},
    {id: 3182, title: 'شماره کارت'}, {id: 3183, title: 'کوپن'}
  ];

  /** سرویس عمومی CRUD پروژه را برای Program ویرایش‌پذیر آماده می‌کند. */
  constructor(genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'walletProgram';
  }

  /** Lookupهای موجود نوع Program و روش تخصیص را بارگذاری می‌کند. */
  ngOnInit(): void {
    super.ngOnInit();
    this.scopeTypes.forEach(type => this.scopeTargets[type.field] =
      this._genericDataService.createCustomDataSourceWithCustomUrl(
        'id', `walletScope/GetTargets?scopeType=${type.id}`));
    this.assignmentUsers = this._genericDataService.createCustomDataSourceWithCustomUrl(
      'id', 'walletAssignment/GetUsers');
    this._genericDataService.getCommonBaseDataForSelect('WalletProgramType')
      .subscribe(res => this.programTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('WalletProgramAllocationMethodType')
      .subscribe(res => this.allocationMethods = res);
    if (this.isNew) {
      this.entity.currencyCode = '';
      this.entity.totalAmount = 0;
      this.entity.startDate = new Date();
    }
  }

  /** پس از خواندن Program، Scopeهای همان Program را بارگذاری می‌کند. */
  afterLoadData(res: any): any {
    super.afterLoadData(res);
    this.auditResult = null;
    this.refreshScopes();
    this.refreshAssignments();
    this.refreshGrants();
    this.refreshBenefits();
  }

  /** داده‌های Scope را برای فهرست مدیریتی به‌روز می‌کند. */
  refreshScopes(): void {
    if (this.entity.id) {
      this.scopes = this._genericDataService.createCustomDataSourceWithCustomUrl(
        'id', `walletScope?walletProgramId=${this.entity.id}`);
    }
  }

  /** پس از تغییر Scope، نسخه Program و فهرست Scopeها را دوباره می‌خواند. */
  refreshProgramAndScopes(): void {
    this._genericDataService.getEntity('walletProgram', this.entity.id).subscribe(program => {
      if (program) { this.entity = program; }
      this.auditResult = null;
      this.refreshScopes();
      this.refreshAssignments();
      this.refreshGrants();
      this.refreshBenefits();
    });
  }

  /** Assignmentهای Program را بدون تغییر اطلاعات مالی دوباره بارگذاری می‌کند. */
  refreshAssignments(): void {
    if (this.entity.id) {
      this.assignments = this._genericDataService.createCustomDataSourceWithCustomUrl(
        'id', `walletAssignment?walletProgramId=${this.entity.id}`);
    }
  }

  /** Grantهای Ledger مربوط به Program را برای نمایش مدیریتی تازه می‌کند. */
  refreshGrants(): void {
    if (this.entity.id) {
      this.grants = this._genericDataService.createCustomDataSourceWithCustomUrl(
        'id', `walletGrant?walletProgramId=${this.entity.id}`);
    }
  }

  /** گروه‌های کارت و Batchها را فقط برای Program جاری بارگذاری می‌کند. */
  refreshBenefits(): void {
    if (!this.entity.id) { return; }
    this.cardGroups = this._genericDataService.createCustomDataSourceWithCustomUrl(
      'id', `walletCard/Groups?walletProgramId=${this.entity.id}`);
    this.couponBatches = this._genericDataService.createCustomDataSourceWithCustomUrl(
      'id', `walletCouponBatch?walletProgramId=${this.entity.id}`);
  }

  selectCardGroup(event: any): void {
    this.selectedCardGroup = event.data;
    this.cardMembers = this._genericDataService.createCustomDataSourceWithCustomUrl(
      'id', `walletCard/Members?groupId=${event.data.id}`);
  }

  createCardGroup(): void {
    if (!this.isDraft() || !this.cardGroupEditor || !this.cardGroupEditor.title) { return; }
    this._genericDataService.postEntityByUrl('/api/walletCard/CreateGroup',
      {...this.cardGroupEditor, walletProgramId: this.entity.id}).subscribe((res: any) => {
      notify(res && res.message || (res && res.isSuccess ? 'گروه ثبت شد.' : 'ثبت گروه انجام نشد.'),
        res && res.isSuccess ? 'success' : 'error', 4000);
      if (res && res.isSuccess) { this.cardGroupEditor = null; this.refreshBenefits(); }
    });
  }

  addCard(): void {
    if (!this.selectedCardGroup || !this.cardNumber) { return; }
    this._genericDataService.postEntityByUrl('/api/walletCard/AddCard',
      {walletCardGroupId: this.selectedCardGroup.id, cardNumber: this.cardNumber})
      .subscribe((res: any) => {
        this.cardNumber = '';
        notify(res && res.message || (res && res.isSuccess ? 'کارت ثبت شد.' : 'ثبت کارت انجام نشد.'),
          res && res.isSuccess ? 'success' : 'error', 4000);
        if (res && res.isSuccess) { this.selectCardGroup({data: this.selectedCardGroup}); this.refreshBenefits(); }
      });
  }

  activateCardGroup(): void {
    if (!this.selectedCardGroup) { return; }
    this._genericDataService.postEntityByUrl('/api/walletCard/ActivateGroup',
      this.selectedCardGroup.id).subscribe((res: any) => {
      notify(res && res.message || (res && res.isSuccess ? 'گروه فعال شد.' : 'فعال‌سازی انجام نشد.'),
        res && res.isSuccess ? 'success' : 'error', 4000);
      if (res && res.isSuccess) { this.selectedCardGroup = null; this.refreshBenefits(); }
    });
  }

  createCouponBatch(): void {
    if (this.batchCreating || this.issuedBatchId || !this.couponBatchEditor
      || !this.couponBatchEditor.title || !this.couponBatchEditor.quantity
      || !this.couponBatchEditor.amountPerCoupon) { return; }
    this.batchCreating = true;
    this._genericDataService.postEntityByUrl('/api/walletCouponBatch',
      {...this.couponBatchEditor, walletProgramId: this.entity.id}).subscribe((res: any) => {
      this.batchCreating = false;
      notify(res && res.message || (res && res.isSuccess ? 'Batch ثبت شد.' : 'ثبت Batch انجام نشد.'),
        res && res.isSuccess ? 'success' : 'error', 5000);
      if (res && res.isSuccess) {
        this.issuedCodes = res.codes || [];
        this.issuedBatchId = res.entityId;
        this.refreshBenefits();
      }
    }, () => {
      this.batchCreating = false;
      notify('نتیجه ساخت Batch نامشخص است؛ با همان کلید تلاش را بررسی کنید.', 'error', 5000);
    });
  }

  /** تأیید دریافت کدها، شرط فعال‌شدن Batch در رابط مدیریت است. */
  activateIssuedBatch(): void {
    if (!this.issuedBatchId || !this.issuedCodes.length) { return; }
    this._genericDataService.postEntityByUrl('/api/walletCouponBatch/Activate',
      this.issuedBatchId).subscribe((res: any) => {
      notify(res && res.message || (res && res.isSuccess ? 'Batch فعال شد.' : 'فعال‌سازی انجام نشد.'),
        res && res.isSuccess ? 'success' : 'error', 4000);
      if (res && res.isSuccess) {
        this.issuedCodes = []; this.issuedBatchId = null;
        this.couponBatchEditor = null; this.refreshBenefits();
      }
    });
  }

  cancelCouponBatch(): void {
    const key = this.selectedCouponBatch && this.selectedCouponBatch.attemptKey
      || this.couponBatchEditor && this.couponBatchEditor.attemptKey;
    if (!key) { return; }
    confirm('Batch در Draft و تمام کدهای آن لغو شوند؟', 'لغو Batch').then(accepted => {
      if (!accepted) { return; }
      this._genericDataService.postEntityByUrl('/api/walletCouponBatch/Cancel', key)
        .subscribe((res: any) => {
          notify(res && res.message || (res && res.isSuccess ? 'Batch لغو شد.' : 'لغو انجام نشد.'),
            res && res.isSuccess ? 'success' : 'error', 4000);
          if (res && res.isSuccess) {
            this.issuedCodes = []; this.issuedBatchId = null;
            this.couponBatchEditor = null; this.selectedCouponBatch = null; this.refreshBenefits();
          }
        });
    });
  }

  newCouponBatch(): void {
    this.issuedCodes = []; this.issuedBatchId = null;
    this.selectedCouponBatch = null;
    this.couponBatchEditor = {attemptKey: window.crypto.randomUUID(),
      quantity: 100, amountPerCoupon: this.entity.amountPerUser,
      startDate: this.entity.startDate, endDate: this.entity.endDate};
  }

  /** مغایرت‌های Program را از API خواندنی Audit دریافت می‌کند. */
  loadAudit(): void {
    if (!this.entity.id || this.auditLoading) { return; }
    this.auditLoading = true;
    this._genericDataService.getEntity('walletAudit', this.entity.id)
      .subscribe((result: any) => {
        this.auditLoading = false;
        this.auditResult = result && result.isSuccess ? result : null;
        if (!this.auditResult) {
          notify(result && result.message || 'گزارش ممیزی دریافت نشد.', 'error', 4000);
        }
      }, () => {
        this.auditLoading = false;
        notify('گزارش ممیزی دریافت نشد.', 'error', 4000);
      });
  }

  /** ایجاد Grant را فقط برای Program فعال در بازه اعتبار نمایش می‌دهد. */
  canCreateGrant(): boolean {
    return this.canCreateAssignment();
  }

  /** فرم Grant را برای یک Assignment موجود باز می‌کند. */
  addGrant(): void {
    if (!this.canCreateGrant()) { return; }
    this.grantEditor = {amount: null, code: ''};
  }

  /** Grant را با مبلغ و کلید درخواست معتبر به Service مالی ارسال می‌کند. */
  saveGrant(): void {
    if (!this.canCreateGrant() || !this.grantEditor
      || !this.grantEditor.walletAssignmentId || !this.grantEditor.code
      || !this.grantEditor.code.trim() || this.grantEditor.code.trim().length > 150
      || !(this.grantEditor.amount > 0)
      || Number(this.grantEditor.amount.toFixed(2)) !== this.grantEditor.amount) {
      notify('Assignment، مبلغ مثبت با حداکثر دو رقم اعشار و کلید Grant لازم است.', 'error', 4000);
      return;
    }
    this._genericDataService.postEntityByUrl('/api/walletGrant', this.grantEditor)
      .subscribe((res: any) => {
        notify(res && res.message || (res && res.isSuccess ? 'Grant ثبت شد.' : 'ثبت Grant انجام نشد.'),
          res && res.isSuccess ? 'success' : 'error', 4000);
        if (res && res.isSuccess) {
          this.grantEditor = null;
          this.refreshProgramAndScopes();
          this.updateOrInsertAffected = true;
        }
      }, (error: any) => notify(error && error.error && error.error.message
        || 'ثبت Grant انجام نشد.', 'error', 4000));
  }

  /** امکان ایجاد عضویت صفرمانده را فقط در بازه Program فعال نشان می‌دهد. */
  canCreateAssignment(): boolean {
    const now = new Date();
    return !!this.entity.id && this.entity.walletProgramStatusType === this.status.active
      && new Date(this.entity.startDate).getTime() <= now.getTime()
      && (!this.entity.endDate || new Date(this.entity.endDate).getTime() > now.getTime());
  }

  /** فرم ایجاد عضویت دستی را برای Program فعال باز می‌کند. */
  addAssignment(): void {
    if (!this.canCreateAssignment()) { return; }
    this.assignmentEditor = {walletProgramId: this.entity.id};
  }

  /** عضویت دستی را با مبلغ صفر در Service ثبت می‌کند. */
  saveAssignment(): void {
    if (!this.canCreateAssignment() || !this.assignmentEditor
      || !this.assignmentEditor.userId || !this.assignmentEditor.title
      || !this.assignmentEditor.title.trim()) { return; }
    this._genericDataService.postEntityByUrl('/api/walletAssignment', this.assignmentEditor)
      .subscribe((res: any) => {
        notify(res && res.message || (res && res.isSuccess ? 'Assignment ثبت شد.' : 'ثبت Assignment انجام نشد.'),
          res && res.isSuccess ? 'success' : 'error', 4000);
        if (res && res.isSuccess) {
          this.assignmentEditor = null;
          this.refreshProgramAndScopes();
          this.updateOrInsertAffected = true;
        }
      });
  }

  /** فرم ایجاد Scope را فقط برای Program در Draft باز می‌کند. */
  addScope(): void {
    if (!this.entity.id || !this.isDraft()) { return; }
    this.scopeEditor = {walletProgramId: this.entity.id, isActive: true};
  }

  /** نسخه و فیلدهای Scope انتخابی را برای ویرایش در Draft نگه می‌دارد. */
  editScope(event: any): void {
    if (!this.isDraft() || !event || !event.data) { return; }
    this.scopeEditor = {...event.data};
  }

  /** با عوض‌شدن نوع Scope، شناسه‌های نوع قبلی را پاک می‌کند. */
  scopeTypeChanged(event: any): void {
    if (!this.scopeEditor || event.previousValue === undefined
      || event.previousValue === event.value) { return; }
    this.scopeTypes.forEach(type => this.scopeEditor[type.field] = null);
  }

  /** Scope را از API مدیریت Program ثبت می‌کند؛ Rule نهایی در Service است. */
  saveScope(): void {
    if (!this.isDraft() || !this.scopeEditor || !this.scopeEditor.walletScopeType
      || !this.scopeEditor.title || !this.scopeEditor.title.trim()) { return; }
    const type = this.scopeTypes.find(item => item.id === this.scopeEditor.walletScopeType);
    if (!type || !this.scopeEditor[type.field]) {
      notify('هدف Scope را انتخاب کنید.', 'error', 3000);
      return;
    }
    const url = this.scopeEditor.id ? '/api/walletScope/UpdateEntity' : '/api/walletScope';
    this._genericDataService.postEntityByUrl(url, this.scopeEditor).subscribe((res: any) => {
      notify(res && res.message || (res && res.isSuccess ? 'Scope ثبت شد.' : 'ثبت Scope انجام نشد.'),
        res && res.isSuccess ? 'success' : 'error', 4000);
      if (res && res.isSuccess) {
        this.scopeEditor = null;
        this.refreshProgramAndScopes();
        this.updateOrInsertAffected = true;
      }
    });
  }

  /** حذف Scope را پس از تأیید کاربر و فقط در Draft درخواست می‌کند. */
  deleteScope(): void {
    if (!this.isDraft() || !this.scopeEditor || !this.scopeEditor.id) { return; }
    confirm('Scope انتخاب‌شده حذف شود؟', 'حذف Scope').then(confirmed => {
      if (!confirmed) { return; }
      this._genericDataService.postEntityByUrl('/api/walletScope/DeleteEntity',
        {id: this.scopeEditor.id, rowVersion: this.scopeEditor.rowVersion}).subscribe((res: any) => {
        notify(res && res.message || (res && res.isSuccess ? 'Scope حذف شد.' : 'حذف Scope انجام نشد.'),
          res && res.isSuccess ? 'success' : 'error', 4000);
        if (res && res.isSuccess) {
          this.scopeEditor = null;
          this.refreshProgramAndScopes();
          this.updateOrInsertAffected = true;
        }
      });
    });
  }

  /** فیلدهای ساختاری را فقط تا پیش از فعال‌سازی قابل ویرایش می‌داند. */
  isDraft(): boolean {
    return !this.entity.id || this.entity.walletProgramStatusType === this.status.draft;
  }

  /** تغییر بودجه و بازه را فقط در وضعیت‌های مجاز فعال می‌کند. */
  canEditFinancial(): boolean {
    return this.isDraft() || this.entity.walletProgramStatusType === this.status.active
      || this.entity.walletProgramStatusType === this.status.suspended;
  }

  /** پس از نخستین Grant، مبنای تخصیص Program ثابت می‌ماند. */
  canEditAllocation(): boolean {
    return this.canEditFinancial() && !(this.entity.allocatedAmount > 0);
  }

  /** دکمه‌های Lifecycle را فقط برای گذارهای مجاز نمایش می‌دهد. */
  canChangeTo(target: number): boolean {
    const current = this.entity.walletProgramStatusType;
    return current === this.status.draft && target === this.status.active
      || current === this.status.active && (target === this.status.suspended
        || target === this.status.closed || target === this.status.expired)
      || current === this.status.suspended && (target === this.status.active
        || target === this.status.closed || target === this.status.expired);
  }

  /** تغییر وضعیت را با نسخه رکورد به API می‌فرستد؛ اعتبارسنجی نهایی در Service انجام می‌شود. */
  changeStatus(target: number): void {
    if (!this.entity.id || !this.entity.rowVersion || !this.canChangeTo(target)) {
      return;
    }
    this._genericDataService.postEntityByUrl('/api/walletProgram/ChangeStatus',
      {id: this.entity.id, status: target, rowVersion: this.entity.rowVersion})
      .subscribe((res: any) => {
        notify(res.message || (res.isSuccess ? 'وضعیت Program تغییر کرد.' : 'تغییر وضعیت انجام نشد.'),
          res.isSuccess ? 'success' : 'error', 4000);
        if (res.isSuccess && res.program) {
          this.entity = res.program;
          this.updateOrInsertAffected = true;
        }
      }, () => notify('تغییر وضعیت انجام نشد.', 'error', 4000));
  }
}
