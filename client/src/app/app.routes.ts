import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: 'tasks', pathMatch: 'full' },
      {
        path: 'tasks',
        loadComponent: () =>
          import('./features/tasks/pages/my-tasks-page/my-tasks-page.component')
            .then(m => m.MyTasksPageComponent),
      },
      {
        path: 'boards/:id',
        loadComponent: () =>
          import('./features/tasks/pages/board-page/board-page.component')
            .then(m => m.BoardPageComponent),
      },
      {
        path: 'projects',
        loadComponent: () =>
          import('./features/projects/pages/project-list-page/project-list-page.component')
            .then(m => m.ProjectListPageComponent),
      },
      {
        path: 'knowledge-base',
        loadComponent: () =>
          import('./features/knowledge-base/pages/spaces-page/spaces-page.component')
            .then(m => m.SpacesPageComponent),
      },
      {
        path: 'analytics',
        loadComponent: () =>
          import('./features/analytics/pages/dashboard-page/dashboard-page.component')
            .then(m => m.DashboardPageComponent),
      },
      {
        path: 'workflows',
        loadComponent: () =>
          import('./features/workflows/pages/workflow-list-page/workflow-list-page.component')
            .then(m => m.WorkflowListPageComponent),
      },
      {
        path: 'settings',
        loadComponent: () =>
          import('./features/settings/pages/settings-page/settings-page.component')
            .then(m => m.SettingsPageComponent),
      },
    ],
  },
];
