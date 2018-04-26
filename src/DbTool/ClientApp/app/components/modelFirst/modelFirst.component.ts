import { Component, Inject } from "@angular/core";
import { Http, Headers } from "@angular/http";

@Component({
    selector: "modelFirst",
    templateUrl: "./modelFirst.component.html"
})
export class ModelFirstComponent {

    private _baseUrl:string;
    private _http:Http;

    constructor(http: Http, @Inject("BASE_URL") baseUrl: string) {
        this._baseUrl = baseUrl;
        this._http = http;
    }

    notify():void {
        let headers:Headers = new Headers();
        headers.append("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");

        this._http.post(this._baseUrl + "api/Notification/SendNotification", "title=DbTool&message=Welcome to ModelFirst", {
            headers: headers
        })
        .subscribe(result => {
            console.log(result);
        }, error => console.error(error));
    }
}
