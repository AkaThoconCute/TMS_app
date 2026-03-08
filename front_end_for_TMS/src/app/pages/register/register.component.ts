import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountApiService } from '../../features/account/services/account-api.service';
import { RegisterDto } from '../../features/account/models/auth.models';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  fullName = '';
  email = '';
  password = '';
  confirmPassword = '';
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private accountApiService: AccountApiService,
    private router: Router
  ) { }

  handleSubmit(): void {
    // Reset messages
    this.errorMessage = '';
    this.successMessage = '';

    // Validate inputs
    if (!this.fullName || !this.email || !this.password || !this.confirmPassword) {
      this.errorMessage = 'Please fill in all fields';
      return;
    }

    if (!this.isValidEmail(this.email)) {
      this.errorMessage = 'Please enter a valid email address';
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    if (this.password.length < 6) {
      this.errorMessage = 'Password must be at least 6 characters long';
      return;
    }

    this.isLoading = true;

    const userData: RegisterDto = {
      fullName: this.fullName,
      email: this.email,
      password: this.password
    };

    this.accountApiService.register(userData).subscribe({
      next: (result) => {
        this.isLoading = false;

        if (result.success) {
          this.successMessage = 'Registration successful! Redirecting to login...';

          // Clear form
          this.fullName = '';
          this.email = '';
          this.password = '';
          this.confirmPassword = '';

          // Redirect to login after a short delay
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 1500);
        } else {
          this.errorMessage = result.errors?.[0] || 'Registration failed';
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.message || 'An error occurred during registration. Please try again.';
        console.error('Registration error:', error);
      }
    });
  }

  private isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }
}
