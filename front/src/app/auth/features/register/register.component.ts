import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../data-access/auth.service';
import { CommonModule } from '@angular/common';
import { RegisterRequest } from 'app/auth/data-access/registerRequest.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
})
export class RegisterComponent {
  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {}

  public error: string | null = null;

  form = this.fb.group({
    username: ['', [Validators.required]],
    firstname: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
    role: ['User']
  });

  onSubmit(): void {
      if (this.form.invalid) return;
  
      const { username, firstname, email, password } = this.form.value;
      if (!username || !firstname || !email || !password) return;
  
      const registerRequest: RegisterRequest = {
      username: username,
      firstname: firstname,
      email: email,
      password: password
      };
  
      this.authService.register(registerRequest).subscribe({
        next: res => {
          console.log('Connecté', res),
          this.router.navigateByUrl('/home');
        },
        error: err => {
          this.error = 'Identifiants invalides.';
          console.error('Erreur login', err)
        }
      });
    }
}
