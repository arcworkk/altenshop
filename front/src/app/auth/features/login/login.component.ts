import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../data-access/auth.service';
import { CommonModule } from '@angular/common';
import { LoginRequest } from '../../data-access/loginRequest.model';
import { Router, RouterLink } from '@angular/router';
import { CardModule } from 'primeng/card';
import { MessageModule } from 'primeng/message';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, CardModule, MessageModule, PasswordModule, ButtonModule, RouterLink ],
})
export class LoginComponent implements OnInit {
  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {}

  ngOnInit(): void { 
    if (this.authService.isLoggedIn()) { 
      this.router.navigateByUrl('/home');
    } 
  }

  public error: string | null = null;

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  onSubmit(): void {
    if (this.form.invalid) return;

    const { email, password } = this.form.value;
    if (!email || !password) return;

    const loginRequest: LoginRequest = {
    email: email,
    password: password
    };

    this.authService.login(loginRequest).subscribe({
      next: res => {
        console.log('Connecté', res),
        this.router.navigateByUrl('/home');
      },
      error: err => {
        this.error = `Identifiants invalides. Erreur : ${err.error.error}`;
        console.error('Erreur login', err)
      }
    });
  }
}