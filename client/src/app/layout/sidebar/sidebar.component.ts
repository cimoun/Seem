import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { Project } from '../../core/models/project.model';
import { ProjectService } from '../../core/services/project.service';

interface NavItem {
  label: string;
  icon: string;
  route: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, MatListModule, MatIconModule, MatDividerModule],
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

      @if (projects.length > 0) {
        <mat-divider></mat-divider>
        <div class="section-header">Projects</div>
        <mat-nav-list dense>
          @for (project of projects; track project.id) {
            <a mat-list-item
               [routerLink]="getProjectBoardRoute(project)"
               routerLinkActive="active">
              <span class="project-color" matListItemIcon
                    [style.background]="project.color"></span>
              <span matListItemTitle>{{ project.name }}</span>
            </a>
          }
        </mat-nav-list>
      }
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
      overflow-y: auto;
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
    .section-header {
      padding: 16px 24px 4px;
      font-size: 12px;
      font-weight: 600;
      text-transform: uppercase;
      letter-spacing: 0.5px;
      color: #888;
    }
    .project-color {
      width: 12px;
      height: 12px;
      border-radius: 3px;
      display: inline-block;
      flex-shrink: 0;
    }
  `],
})
export class SidebarComponent implements OnInit {
  navItems: NavItem[] = [
    { label: 'Tasks', icon: 'task_alt', route: '/tasks' },
    { label: 'Projects', icon: 'folder', route: '/projects' },
    { label: 'Knowledge Base', icon: 'menu_book', route: '/knowledge-base' },
    { label: 'Analytics', icon: 'analytics', route: '/analytics' },
    { label: 'Workflows', icon: 'account_tree', route: '/workflows' },
    { label: 'Settings', icon: 'settings', route: '/settings' },
  ];

  projects: Project[] = [];

  constructor(private projectService: ProjectService) {}

  ngOnInit(): void {
    this.projectService.getAll().subscribe({
      next: (projects) => {
        this.projects = projects.filter(p => !p.isArchived);
      },
    });
  }

  getProjectBoardRoute(project: Project): string {
    if (project.boards && project.boards.length > 0) {
      return `/boards/${project.boards[0].id}`;
    }
    return `/projects`;
  }
}
