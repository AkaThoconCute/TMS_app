import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '@platform/auth/auth.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './reset-password.page.html',
  styleUrl: './reset-password.page.css'
})
export class ResetPasswordPage {
  step: 1 | 2 = 1;
  email = '';
  token = '';
  newPassword = '';
  confirmPassword = '';
  isLoading = false;
  errorMessages: string[] = [];
  successMessage = '';

  constructor(private authService: AuthService, private router: Router) { }

  requestToken(): void {
    if (this.isLoading) return;
    this.errorMessages = [];
    this.successMessage = '';
    this.isLoading = true;

    this.authService.forgotPassword({ email: this.email }).subscribe({
      next: (result) => {
        this.isLoading = false;
        if (result.success) {
          this.successMessage = 'A reset token has been logged. Please check the server console.';
          this.step = 2;
        } else {
          this.errorMessages = [result.message || 'Failed to send reset token.'];
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessages = [err.message || 'An unexpected error occurred.'];
      }
    });
  }

  submitReset(): void {
    if (this.isLoading) return;
    this.errorMessages = [];
    this.successMessage = '';

    if (this.newPassword !== this.confirmPassword) {
      this.errorMessages = ['Passwords do not match.'];
      return;
    }

    this.isLoading = true;
    this.authService.resetPassword({ email: this.email, token: this.token, newPassword: this.newPassword }).subscribe({
      next: (result) => {
        this.isLoading = false;
        if (result.success) {
          this.successMessage = 'Your password has been reset successfully.';
        } else {
          this.errorMessages = result.errors ?? ['Failed to reset password.'];
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessages = [err.message || 'An unexpected error occurred.'];
      }
    });
  }
}
