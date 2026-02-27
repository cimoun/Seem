import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [MatToolbarModule, MatIconModule, MatButtonModule, MatInputModule, MatFormFieldModule],
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
      <button mat-icon-button>
        <mat-icon>account_circle</mat-icon>
      </button>
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
  `],
})
export class HeaderComponent {}
