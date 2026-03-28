import { Component, effect, inject, signal, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { TagModule } from 'primeng/tag';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '@platform/auth/auth.service';
import { UpdateProfileDto, ChangePasswordDto } from '@platform/auth/auth.models';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    ButtonModule,
    InputTextModule,
    PasswordModule,
    TagModule,
    ToastModule,
  ],
  providers: [MessageService],
  templateUrl: './profile.page.html',
})
export class ProfilePage {
  private readonly authService = inject(AuthService);
  private readonly messageService = inject(MessageService);
  private readonly fb = inject(FormBuilder);

  readonly currentUser = toSignal(this.authService.currentUser$);
  savingProfile = signal(false);
  changingPassword = signal(false);

  profileForm: FormGroup = this.fb.group({
    userName: ['', Validators.required],
  });

  passwordForm: FormGroup = this.fb.group(
    {
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
    },
    { validators: ProfilePage.passwordsMatchValidator }
  );

  constructor() {
    effect(() => {
      const user = this.currentUser();
      if (user) {
        untracked(() => {
          if (this.profileForm.pristine) {
            this.profileForm.patchValue({ userName: user.userName });
          }
        });
      }
    });
  }

  private static passwordsMatchValidator(group: AbstractControl): ValidationErrors | null {
    const newPwd = group.get('newPassword')?.value;
    const confirmPwd = group.get('confirmPassword')?.value;
    return newPwd && confirmPwd && newPwd !== confirmPwd ? { passwordsMismatch: true } : null;
  }

  saveProfile(): void {
    if (this.profileForm.invalid || this.savingProfile()) return;
    this.savingProfile.set(true);
    const dto: UpdateProfileDto = { userName: this.profileForm.getRawValue().userName };
    this.authService.updateProfile(dto).subscribe({
      next: () => {
        this.savingProfile.set(false);
        this.profileForm.markAsPristine();
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Profile updated successfully.',
          life: 3000,
        });
      },
      error: (err: Error) => {
        this.savingProfile.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: err.message || 'Failed to update profile.',
          life: 5000,
        });
      },
    });
  }

  changePassword(): void {
    if (this.passwordForm.invalid || this.changingPassword()) return;
    this.changingPassword.set(true);
    const raw = this.passwordForm.getRawValue();
    const dto: ChangePasswordDto = { currentPassword: raw.currentPassword, newPassword: raw.newPassword };
    this.authService.changePassword(dto).subscribe({
      next: (result) => {
        this.changingPassword.set(false);
        if (result.success) {
          this.passwordForm.reset();
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Password changed successfully.',
            life: 3000,
          });
        } else {
          const detail = result.errors?.[0] || 'Failed to change password.';
          this.messageService.add({ severity: 'error', summary: 'Error', detail, life: 5000 });
        }
      },
      error: (err: Error) => {
        this.changingPassword.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: err.message || 'Failed to change password.',
          life: 5000,
        });
      },
    });
  }
}
