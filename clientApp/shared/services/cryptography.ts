import {Injectable} from '@angular/core';
import * as CryptoJS from 'crypto-js';
import * as Forge from 'node-forge';

@Injectable({
  providedIn: 'root'
})

export class Cryptography {
  // encrypt(txt: string): string {
  //   const key = CryptoJS.enc.Utf8.parse('8080808080808080');
  //   const iv = CryptoJS.enc.Utf8.parse('8080808080808080');
  //   return CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txt), key,
  //     {
  //       keySize: 128 / 8,
  //       iv,
  //       mode: CryptoJS.mode.CBC,
  //       padding: CryptoJS.pad.Pkcs7
  //     }).toString();
  // }

  encryptRsa(txt: string, key: any): string {
    if (txt) {
      const rsa = Forge.pki.publicKeyFromPem(key.publicKey);
      const res = window.btoa(rsa.encrypt(txt));
      return res;
    }
    return txt;
  }
}
