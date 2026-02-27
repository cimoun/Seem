import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Project } from '../../../../core/models/project.model';
import { ProjectService } from '../../../../core/services/project.service';
import { ProjectDialogComponent, ProjectDialogData } from '../../components/project-dialog/project-dialog.component';

@Component({
  selector: 'app-project-list-page',
  standalone: true,
  imports: [
    DatePipe,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatSnackBarModule,
    MatChipsModule,
    MatProgressSpinnerModule,
  ],
  template: `
    <div class="page-container">
      <div class="page-header">
        <h1>Projects</h1>
      </div>

      @if (loading) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <div class="project-grid">
          @for (project of projects; track project.id) {
            <mat-card class="project-card" (click)="openBoard(project)" appearance="outlined">
              <div class="color-bar" [style.background]="project.color"></div>
              <mat-card-header>
                <mat-card-title>{{ project.name }}</mat-card-title>
                <mat-card-subtitle>
                  <mat-chip-set>
                    <mat-chip>{{ project.key }}</mat-chip>
                  </mat-chip-set>
                </mat-card-subtitle>
              </mat-card-header>
              <mat-card-content>
                @if (project.description) {
                  <p class="project-description">{{ project.description }}</p>
                }
                <div class="project-meta">
                  <span class="meta-item">
                    <mat-icon class="meta-icon">task_alt</mat-icon>
                    {{ project.taskCount }} tasks
                  </span>
                  <span class="meta-item">
                    <mat-icon class="meta-icon">calendar_today</mat-icon>
                    {{ project.createdAt | date:'mediumDate' }}
                  </span>
                </div>
              </mat-card-content>
              <mat-card-actions align="end">
                <button mat-icon-button (click)="editProject(project, $event)">
                  <mat-icon>edit</mat-icon>
                </button>
              </mat-card-actions>
            </mat-card>
          }
          @empty {
            <div class="empty-state">
              <mat-icon class="empty-icon">folder_off</mat-icon>
              <h3>No projects yet</h3>
              <p>Create your first project to get started.</p>
            </div>
          }
        </div>
      }

      <button mat-fab color="primary" class="fab-button" (click)="createProject()">
        <mat-icon>add</mat-icon>
      </button>
    </div>
  `,
  styles: [`
    .page-container {
      padding: 24px;
      position: relative;
      min-height: calc(100vh - 64px);
    }
    .page-header {
      margin-bottom: 24px;
      h1 {
        margin: 0;
        font-size: 24px;
        font-weight: 500;
      }
    }
    .loading-container {
      display: flex;
      justify-content: center;
      padding: 48px;
    }
    .project-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
      gap: 16px;
    }
    .project-card {
      cursor: pointer;
      position: relative;
      overflow: hidden;
      transition: box-shadow 0.2s;
    }
    .project-card:hover {
      box-shadow: 0 4px 12px rgba(0,0,0,0.12);
    }
    .color-bar {
      height: 4px;
      width: 100%;
    }
    .project-description {
      color: #666;
      font-size: 14px;
      margin: 8px 0;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      overflow: hidden;
    }
    .project-meta {
      display: flex;
      gap: 16px;
      margin-top: 12px;
    }
    .meta-item {
      display: flex;
      align-items: center;
      gap: 4px;
      font-size: 13px;
      color: #888;
    }
    .meta-icon {
      font-size: 16px;
      width: 16px;
      height: 16px;
    }
    .empty-state {
      grid-column: 1 / -1;
      text-align: center;
      padding: 48px;
      color: #888;
    }
    .empty-icon {
      font-size: 64px;
      width: 64px;
      height: 64px;
      color: #ccc;
    }
    .fab-button {
      position: fixed;
      bottom: 32px;
      right: 32px;
    }
  `],
})
export class ProjectListPageComponent implements OnInit {
  projects: Project[] = [];
  loading = true;

  constructor(
    private projectService: ProjectService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.loading = true;
    this.projectService.getAll().subscribe({
      next: (projects) => {
        this.projects = projects;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.snackBar.open('Failed to load projects', 'Close', { duration: 3000 });
      },
    });
  }

  createProject(): void {
    const dialogRef = this.dialog.open(ProjectDialogComponent, {
      data: {} as ProjectDialogData,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.projectService.create(result).subscribe({
          next: () => {
            this.snackBar.open('Project created', 'Close', { duration: 3000 });
            this.loadProjects();
          },
          error: () => {
            this.snackBar.open('Failed to create project', 'Close', { duration: 3000 });
          },
        });
      }
    });
  }

  editProject(project: Project, event: Event): void {
    event.stopPropagation();
    const dialogRef = this.dialog.open(ProjectDialogComponent, {
      data: { project } as ProjectDialogData,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.projectService.update(project.id, result).subscribe({
          next: () => {
            this.snackBar.open('Project updated', 'Close', { duration: 3000 });
            this.loadProjects();
          },
          error: () => {
            this.snackBar.open('Failed to update project', 'Close', { duration: 3000 });
          },
        });
      }
    });
  }

  openBoard(project: Project): void {
    if (project.boards && project.boards.length > 0) {
      this.router.navigate(['/boards', project.boards[0].id]);
    } else {
      this.projectService.getById(project.id).subscribe(fullProject => {
        if (fullProject.boards && fullProject.boards.length > 0) {
          this.router.navigate(['/boards', fullProject.boards[0].id]);
        } else {
          this.snackBar.open('No board found for this project', 'Close', { duration: 3000 });
        }
      });
    }
  }
}
