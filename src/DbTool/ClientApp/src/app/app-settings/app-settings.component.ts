import { Component, OnInit } from '@angular/core';
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
  setting: DbToolSetting;
  supportedDbType: Array<string>;
  private _localCacheKey = 'dbToolSettings';

  constructor(http: HttpClient, setting: SettingService) {
    this._http = http;
    this.setting = setting.setting;
    this.supportedDbType = setting.supportedDbType;
  }

  ngOnInit() {
  }
}
