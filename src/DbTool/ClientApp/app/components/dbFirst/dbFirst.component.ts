import { Component, Inject } from "@angular/core";
import { Http } from "@angular/http";

@Component({
    selector: "dbFirst",
    templateUrl: "./dbFirst.component.html"
})
export class DbFirstComponent {

    constructor(http: Http, @Inject("BASE_URL") baseUrl: string) {
        http.get(baseUrl + "api/SampleData/WeatherForecasts").subscribe(result => {
            console.log(result);
        }, error => console.error(error));
    }
}
