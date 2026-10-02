import {Component, OnInit, Type, ViewChild, ViewContainerRef} from '@angular/core';
import {UserDataService} from "../../../_services/user-data.service";
import {LmsUploadedProductItemComponent} from "./lms-uploaded-product-item/lms-uploaded-product-item.component";
import {TbaoUploadedProductItemComponent} from "./tbao-uploaded-product-item/tbao-uploaded-product-item.component";

@Component({
  selector: 'uploaded-product-item-wrapper',
  templateUrl: './uploaded-product-item-wrapper.component.html'
})

export class UploadedProductItemWrapperComponent implements OnInit {

  @ViewChild('viewContainer', { read: ViewContainerRef, static: true })
  viewContainerRef!: ViewContainerRef;

  constructor(private userDataService: UserDataService) {
  }

  ngOnInit() {
    const lmsMode = this.userDataService.getSiteConfigurationLmsMode();
    const component = lmsMode ? LmsUploadedProductItemComponent : TbaoUploadedProductItemComponent;
    this.viewContainerRef.createComponent<any>(component);
  }
}
