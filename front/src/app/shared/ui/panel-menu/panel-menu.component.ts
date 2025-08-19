import {
    Component,
  } from "@angular/core";
  import { PanelMenuModule } from 'primeng/panelmenu';
  import { MenuItem } from 'primeng/api';
  import { Menu, MenuModule } from 'primeng/menu';
  import { ToastModule } from 'primeng/toast';
  import { AvatarModule } from 'primeng/avatar';
  import { AvatarGroupModule } from 'primeng/avatargroup';
import { ButtonModule } from "primeng/button";
import { AuthService } from "app/auth/data-access/auth.service";
import { Router } from "@angular/router";
  
  @Component({
    selector: "app-panel-menu",
    standalone: true,
    imports: [MenuModule, ToastModule, AvatarModule, AvatarGroupModule, ButtonModule],
    templateUrl: "./panel-menu.component.html",
  })
  export class PanelMenuComponent {
    constructor( private authService: AuthService, private router: Router ) {}

    public readonly items: MenuItem[] = [
        {
            label: 'Accueil',
            icon: 'pi pi-home',
            routerLink: ['/home'],
            command: () => this.blurActive()
        },
        {
            label: 'Produits',
            icon: 'pi pi-barcode',
            routerLink: ['/products/list'],
            command: () => this.blurActive()
        }
    ]

    private blurActive() {
      //Gestion de l'erreur : Blocked aria-hidden on an element because its descendant retained focus.
      const el = document.activeElement as HTMLElement | null;
      el?.blur();
    }

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
  