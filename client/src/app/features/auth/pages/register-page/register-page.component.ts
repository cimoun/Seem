import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators, ReactiveFormsModule, ValidationErrors } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-register-page',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
  ],
  template: `
    <div class="register-container">
      <mat-card class="register-card">
        <mat-card-header>
          <mat-card-title>
            <h1 class="app-title">Seem</h1>
          </mat-card-title>
          <mat-card-subtitle>Create a new account</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Username</mat-label>
              <input matInput formControlName="username" autocomplete="username" />
              @if (form.get('username')?.hasError('required') && form.get('username')?.touched) {
                <mat-error>Username is required</mat-error>
              }
              @if (form.get('username')?.hasError('minlength') && form.get('username')?.touched) {
                <mat-error>Username must be at least 3 characters</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Email</mat-label>
              <input matInput formControlName="email" type="email" autocomplete="email" />
              @if (form.get('email')?.hasError('required') && form.get('email')?.touched) {
                <mat-error>Email is required</mat-error>
              }
              @if (form.get('email')?.hasError('email') && form.get('email')?.touched) {
                <mat-error>Enter a valid email address</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Display Name</mat-label>
              <input matInput formControlName="displayName" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Password</mat-label>
              <input matInput [type]="hidePassword ? 'password' : 'text'"
                     formControlName="password" autocomplete="new-password" />
              <button mat-icon-button matSuffix type="button"
                      (click)="hidePassword = !hidePassword">
                <mat-icon>{{ hidePassword ? 'visibility_off' : 'visibility' }}</mat-icon>
              </button>
              @if (form.get('password')?.hasError('required') && form.get('password')?.touched) {
                <mat-error>Password is required</mat-error>
              }
              @if (form.get('password')?.hasError('minlength') && form.get('password')?.touched) {
                <mat-error>Password must be at least 6 characters</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Confirm Password</mat-label>
              <input matInput [type]="hideConfirm ? 'password' : 'text'"
                     formControlName="confirmPassword" autocomplete="new-password" />
              <button mat-icon-button matSuffix type="button"
                      (click)="hideConfirm = !hideConfirm">
                <mat-icon>{{ hideConfirm ? 'visibility_off' : 'visibility' }}</mat-icon>
              </button>
              @if (form.get('confirmPassword')?.hasError('required') && form.get('confirmPassword')?.touched) {
                <mat-error>Please confirm your password</mat-error>
              }
              @if (form.get('confirmPassword')?.hasError('passwordMismatch') && form.get('confirmPassword')?.touched) {
                <mat-error>Passwords do not match</mat-error>
              }
            </mat-form-field>

            <button mat-raised-button color="primary" type="submit"
                    class="full-width submit-btn" [disabled]="loading">
              @if (loading) {
                <mat-spinner diameter="20"></mat-spinner>
              } @else {
                Create Account
              }
            </button>
          </form>
        </mat-card-content>
        <mat-card-actions align="end">
          <a mat-button [routerLink]="'/login'">Already have an account? Sign in</a>
        </mat-card-actions>
      </mat-card>
    </div>
  `,
  styles: [`
    .register-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 100vh;
      background: #f5f5f5;
    }
    .register-card {
      width: 100%;
      max-width: 440px;
      padding: 24px;
    }
    .app-title {
      margin: 0 0 8px 0;
      font-size: 28px;
      font-weight: 700;
    }
    .full-width {
      width: 100%;
    }
    .submit-btn {
      margin-top: 8px;
      height: 48px;
    }
    mat-card-content {
      padding-top: 16px;
    }
    mat-card-actions {
      padding: 8px 0 0 0;
    }
  `],
})
export class RegisterPageComponent {
  form: FormGroup;
  hidePassword = true;
  hideConfirm = true;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar,
  ) {
    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      displayName: [''],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
    });

    this.form.get('confirmPassword')?.addValidators(this.matchPassword.bind(this));
  }

  private matchPassword(control: AbstractControl): ValidationErrors | null {
    const password = this.form?.get('password')?.value;
    const confirm = control.value;
    return password === confirm ? null : { passwordMismatch: true };
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    const { confirmPassword, ...registerData } = this.form.value;
    this.authService.register(registerData).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.loading = false;
        const message = err.error?.errors?.[0] || 'Registration failed. Please try again.';
        this.snackBar.open(message, 'Close', { duration: 5000 });
      },
    });
  }
}
