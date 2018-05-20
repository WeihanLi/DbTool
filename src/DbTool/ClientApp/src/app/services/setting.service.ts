import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DbToolSetting } from '../DbToolSetting';

@Injectable()
export class SettingService {
  private _http: HttpClient;
  setting: DbToolSetting;
  supportedDbType: Array<string>;
  private _localCacheKey = 'dbToolSettings';

  constructor(http: HttpClient) {
    this._http = http;
    // loadSetting
    this.loadSupportedDbType();
    this.loadSetting();
  }

  private loadSupportedDbType() {
    if (this.supportedDbType && this.supportedDbType.length > 0) {
      return;
    } else {
      const su: string = localStorage.getItem('supportedDbType');
      if (su) {
        this.supportedDbType = JSON.parse(su);
        return;
      }
      this._http.get<Array<string>>('/api/Setting/SupportedDbType')
        // clone the data object, using its known Config shape
        .subscribe((data: Array<string>) => {
          if (data && data.length > 0) {
            this.supportedDbType = data;
            localStorage.setItem('supportedDbType', JSON.stringify(data));
          }
        });
    }
  }

  private loadSetting() {
    if (this.setting) {
      return;
    } else {
      const localSetting: string = localStorage.getItem(this._localCacheKey);
      if (localSetting) {
        this.setting = JSON.parse(localSetting);
        return;
      }
      this._http.get<DbToolSetting>('/api/Setting')
        // clone the data object, using its known Config shape
        .subscribe((data: DbToolSetting) => {
          if (data && data.DbType) {
            this.setting = data;
            localStorage.setItem(this._localCacheKey, JSON.stringify(data));
          }
        });
    }
  }
}
