import { Component } from '@angular/core';
import { SettingService } from '../services/setting.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
  supportedDbType: Array<string>;

  constructor(setting: SettingService) {
    this.supportedDbType = setting.supportedDbType;
  }
}
