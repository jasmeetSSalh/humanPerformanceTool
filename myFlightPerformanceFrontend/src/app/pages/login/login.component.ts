import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { MessageModule } from 'primeng/message';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  imports: [CommonModule, FormsModule, ButtonModule, InputTextModule, PasswordModule, MessageModule]
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  username = '';
  password = '';
  loading = false;
  errorMessage: string | null = null;

  signIn(): void {
    if (!this.username || !this.password) {
      this.errorMessage = 'Enter your username and password.';
      return;
    }

    this.loading = true;
    this.errorMessage = null;

    this.authService.login(this.username, this.password).subscribe({
      next: () => {
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
        // Only follow paths inside this app.
        const target = returnUrl?.startsWith('/') && !returnUrl.startsWith('//') && !returnUrl.startsWith('/login')
          ? returnUrl
          : '/';
        this.router.navigateByUrl(target);
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.password = '';
        this.errorMessage = error.status === 429
          ? 'Too many attempts. Wait a minute and try again.'
          : error.status === 401
            ? 'Invalid username or password.'
            : 'Could not reach the server. Try again.';
      }
    });
  }
}
