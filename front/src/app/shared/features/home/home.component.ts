import { Component } from "@angular/core";
import { CardModule } from "primeng/card";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"],
  standalone: true,
  imports: [CardModule],
})
export class HomeComponent {
  public readonly appTitle = "ALTEN SHOP";
}