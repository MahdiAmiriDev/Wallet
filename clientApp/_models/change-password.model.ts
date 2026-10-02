import {User} from './user';

export class ChangePasswordModel extends User {
  oldPassword: string;
  oldPasswordStr: string;
  newPassword: string;
  newPasswordStr: string;
  newPasswordConfirm: string;
  newPasswordConfirmStr: string;
}
