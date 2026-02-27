import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';

interface NavItem {
  label: string;
  icon: string;
  route: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, MatListModule, MatIconModule],
  template: `
    <div class="sidebar">
      <div class="logo">
        <h2>Seem</h2>
      </div>
      <mat-nav-list>
        @for (item of navItems; track item.route) {
          <a mat-list-item [routerLink]="item.route" routerLinkActive="active">
            <mat-icon matListItemIcon>{{ item.icon }}</mat-icon>
            <span matListItemTitle>{{ item.label }}</span>
          </a>
        }
      </mat-nav-list>
    </div>
  `,
  styles: [`
    .sidebar {
      width: 240px;
      height: 100vh;
      background: #fafafa;
      border-right: 1px solid #e0e0e0;
      display: flex;
      flex-direction: column;
    }
    .logo {
      padding: 16px 24px;
      border-bottom: 1px solid #e0e0e0;
      h2 { margin: 0; font-weight: 600; }
    }
    .active {
      background: rgba(0, 0, 0, 0.04);
      font-weight: 500;
    }
  `],
})
export class SidebarComponent {
  navItems: NavItem[] = [
    { label: 'Tasks', icon: 'task_alt', route: '/tasks' },
    { label: 'Projects', icon: 'folder', route: '/projects' },
    { label: 'Knowledge Base', icon: 'menu_book', route: '/knowledge-base' },
    { label: 'Analytics', icon: 'analytics', route: '/analytics' },
    { label: 'Workflows', icon: 'account_tree', route: '/workflows' },
    { label: 'Settings', icon: 'settings', route: '/settings' },
  ];
}
