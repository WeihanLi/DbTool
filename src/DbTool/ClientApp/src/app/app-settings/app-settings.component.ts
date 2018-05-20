import { Component, OnInit,Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DbToolSetting } from '../DbToolSetting';
import { SettingService } from '../services/setting.service';

@Component({
  selector: 'app-app-settings',
  templateUrl: './app-settings.component.html',
  styleUrls: ['./app-settings.component.less']
})
export class AppSettingsComponent implements OnInit {

  private _http: HttpClient;
  private _settingService: SettingService;
  setting: DbToolSetting;
  supportedDbType: Array<string>;
  private _localCacheKey = 'dbToolSettings';

  constructor(http: HttpClient, settingService: SettingService) {
    this._http = http;
    this.setting = settingService.setting;
    this.supportedDbType = settingService.supportedDbType;
    this._settingService = settingService;
  }

  ngOnInit() {
  }

  saveSetting() {
    console.group('----save settings-----');
    console.log('setting:' + JSON.stringify(this.setting));
    const result = this._settingService.updateSetting(this.setting);
    if (result) {
      console.log('setting save success');
    } else {
      console.log('setting save fail');
    }
    console.groupEnd();
  }
}
