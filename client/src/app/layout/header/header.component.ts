import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatMenuModule } from '@angular/material/menu';
import { AsyncPipe } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatMenuModule,
    AsyncPipe,
  ],
  template: `
    <mat-toolbar class="header">
      <mat-form-field appearance="outline" class="search-field">
        <mat-icon matPrefix>search</mat-icon>
        <input matInput placeholder="Search tasks, articles..." />
      </mat-form-field>
      <span class="spacer"></span>
      <button mat-icon-button>
        <mat-icon>notifications</mat-icon>
      </button>
      @if (authService.currentUser$ | async; as user) {
        <span class="username">{{ user.displayName || user.username }}</span>
      }
      <button mat-icon-button [matMenuTriggerFor]="accountMenu">
        <mat-icon>account_circle</mat-icon>
      </button>
      <mat-menu #accountMenu="matMenu">
        <button mat-menu-item (click)="onLogout()">
          <mat-icon>logout</mat-icon>
          <span>Logout</span>
        </button>
      </mat-menu>
    </mat-toolbar>
  `,
  styles: [`
    .header {
      background: white;
      border-bottom: 1px solid #e0e0e0;
      padding: 0 16px;
    }
    .search-field {
      width: 320px;
      margin: 0;
      font-size: 14px;
    }
    .spacer { flex: 1; }
    .username {
      font-size: 14px;
      color: #555;
      margin-right: 4px;
    }
  `],
})
export class HeaderComponent {
  constructor(
    public authService: AuthService,
    private router: Router,
  ) {}

  onLogout(): void {
    this.authService.logout();
  }
}
