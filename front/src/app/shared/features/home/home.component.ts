import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { Router, RouterLink } from "@angular/router";
import { AuthService } from "app/auth/data-access/auth.service";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"],
  standalone: true,
  imports: [CardModule, RouterLink, ButtonModule, CommonModule],
})
export class HomeComponent {
  constructor( private authService: AuthService, private router: Router ) {}

  public readonly appTitle = "ALTEN SHOP";

  get isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  get email() { 
    return this.authService.getEmail();
  }

  logout() {
    this.authService.logout();
    this.goToLogin();
  }

  goToLogin() { 
    return this.router.navigateByUrl('/auth/login');
  }

  get role() {
    return this.authService.getRole();
  }
}