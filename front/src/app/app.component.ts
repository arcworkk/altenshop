import {
  Component,
} from "@angular/core";
import { RouterModule } from "@angular/router";
import { SplitterModule } from 'primeng/splitter';
import { ToolbarModule } from 'primeng/toolbar';
import { PanelMenuComponent } from "./shared/ui/panel-menu/panel-menu.component";
import { SidebarModule } from 'primeng/sidebar';
import { Button } from "primeng/button";
import { MenubarComponent } from "./shared/ui/menubar/menubar.component";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
  standalone: true,
  imports: [MenubarComponent, RouterModule, SplitterModule, ToolbarModule, PanelMenuComponent, SidebarModule, Button],
})
export class AppComponent {
}
