import { HttpInterceptorFn } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'app/auth/data-access/auth.service';
import { MessageService } from 'primeng/api';

export const authErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const auth = inject(AuthService);
  const messageService = inject(MessageService);

  return next(req).pipe(
    catchError(err => {
      if (err?.status === 401) {
        messageService.add({ severity: 'error', summary: 'Erreur', detail: `Connectez-vous pour réaliser cette action.` });
        auth.logout();
        router.navigateByUrl('/home');
      }
      return throwError(() => err);
    })
  );
};