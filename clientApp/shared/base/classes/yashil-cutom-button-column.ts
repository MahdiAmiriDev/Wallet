import {YashilCustomButton} from '../models/yasil-custom-button';

export class YashilCustomButtonColumn {
  caption?: string;
  width?: string;
  setAtFirstColumn?: boolean;
  allowExporting?: boolean;
  customButtons: YashilCustomButton[];
}
