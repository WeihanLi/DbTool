import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import { RouterModule } from "@angular/router";

import { AppComponent } from "./components/app/app.component";
import { NavMenuComponent } from "./components/navmenu/navmenu.component";
import { HomeComponent } from "./components/home/home.component";
import { ModelFirstComponent } from "./components/modelFirst/modelFirst.component";
import { DbFirstComponent } from "./components/dbFirst/dbFirst.component";
import { CodeFirstComponent } from "./components/codeFirst/codeFirst.component";
import { SettingsComponent } from "./components/settings/settings.component";

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        ModelFirstComponent,
        CodeFirstComponent,
        DbFirstComponent,
        HomeComponent,
        SettingsComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: "", redirectTo: "home", pathMatch: "full" },
            { path: "home", component: HomeComponent },
            { path: "modelFirst", component: ModelFirstComponent },
            { path: "codeFirst", component: CodeFirstComponent },
            { path: "dbFirst", component: DbFirstComponent },
            { path: "settings", component: SettingsComponent },
            { path: "**", redirectTo: "home" }
        ])
    ]
})
export class AppModuleShared {
}