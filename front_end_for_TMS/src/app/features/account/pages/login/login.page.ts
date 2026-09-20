import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../platform/auth/auth.service';
import { LoginRequest } from '../../../../platform/auth/auth.models';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.page.html',
  styleUrl: './login.page.css'
})
export class LoginPage {
  email = signal('');
  password = signal('');
  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  handleSubmitLogin(): void {
    // Reset messages
    this.errorMessage.set('');
    this.successMessage.set('');

    // Validate and model request
    const currentEmail = this.email();
    const currentPassword = this.password();

    if (!currentEmail || !currentPassword) {
      this.errorMessage.set('Please fill in all fields');
      return;
    }

    if (!this.isValidEmail(currentEmail)) {
      this.errorMessage.set('Please enter a valid email address');
      return;
    }

    const loginRequest: LoginRequest = {
      email: currentEmail,
      password: currentPassword
    };

    // Process login request
    this.loading.set(true);

    this.authService.login(loginRequest).subscribe({
      next: (response) => {
        this.loading.set(false);

        if (response.success) {
          // Clear up form fields
          this.successMessage.set('Login successful! Go to home page...');
          this.email.set('');
          this.password.set('');

          // Redirect to home/dashboard page
          setTimeout(() => {
            this.router.navigate(['/home']);
          }, 1 * 1000);
        } else {
          const error = response.error!;
          this.errorMessage.set(error.message);
        }
      },
      error: (error) => {
        this.loading.set(false);
        this.errorMessage.set('An error occurred during login. Please try again.');
      }
    });
  }

  private isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }
}
