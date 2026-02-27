import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TaskItem, TaskItemStatus, Priority } from '../../../../core/models/task.model';
import { Project } from '../../../../core/models/project.model';
import { TaskService } from '../../../../core/services/task.service';
import { ProjectService } from '../../../../core/services/project.service';
import { TaskDialogComponent, TaskDialogData } from '../../components/task-dialog/task-dialog.component';

@Component({
  selector: 'app-my-tasks-page',
  standalone: true,
  imports: [
    DatePipe,
    ReactiveFormsModule,
    MatTableModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatDialogModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
  ],
  template: `
    <div class="page-container">
      <div class="page-header">
        <h1>My Tasks</h1>
      </div>

      <div class="filter-bar">
        <form [formGroup]="filterForm" class="filters">
          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Project</mat-label>
            <mat-select formControlName="projectId" (selectionChange)="applyFilters()">
              <mat-option value="">All Projects</mat-option>
              @for (project of projects; track project.id) {
                <mat-option [value]="project.id">{{ project.name }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Status</mat-label>
            <mat-select formControlName="status" (selectionChange)="applyFilters()">
              <mat-option value="">All Statuses</mat-option>
              @for (s of statuses; track s) {
                <mat-option [value]="s">{{ s }}</mat-option>
              }
            </mat-select>
          </mat-form-field>

          <mat-form-field appearance="outline" class="filter-field">
            <mat-label>Priority</mat-label>
            <mat-select formControlName="priority" (selectionChange)="applyFilters()">
              <mat-option value="">All Priorities</mat-option>
              @for (p of priorities; track p) {
                <mat-option [value]="p">{{ p }}</mat-option>
              }
            </mat-select>
          </mat-form-field>
        </form>
      </div>

      @if (loading) {
        <div class="loading-container">
          <mat-spinner diameter="48"></mat-spinner>
        </div>
      } @else {
        <table mat-table [dataSource]="tasks" class="tasks-table">
          <ng-container matColumnDef="taskKey">
            <th mat-header-cell *matHeaderCellDef>Key</th>
            <td mat-cell *matCellDef="let task">
              <span class="task-key">{{ task.taskKey }}</span>
            </td>
          </ng-container>

          <ng-container matColumnDef="title">
            <th mat-header-cell *matHeaderCellDef>Title</th>
            <td mat-cell *matCellDef="let task">{{ task.title }}</td>
          </ng-container>

          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef>Status</th>
            <td mat-cell *matCellDef="let task">
              <mat-chip [class]="'status-' + task.status.toLowerCase()">
                {{ task.status }}
              </mat-chip>
            </td>
          </ng-container>

          <ng-container matColumnDef="priority">
            <th mat-header-cell *matHeaderCellDef>Priority</th>
            <td mat-cell *matCellDef="let task">
              <span class="priority-badge" [class]="'priority-' + task.priority.toLowerCase()">
                {{ task.priority }}
              </span>
            </td>
          </ng-container>

          <ng-container matColumnDef="dueDate">
            <th mat-header-cell *matHeaderCellDef>Due Date</th>
            <td mat-cell *matCellDef="let task">
              @if (task.dueDate) {
                <span [class.overdue]="task.isOverdue">
                  {{ task.dueDate | date:'mediumDate' }}
                </span>
              }
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"
              class="task-row" (click)="editTask(row)"></tr>
        </table>

        @if (tasks.length === 0) {
          <div class="empty-state">
            <mat-icon class="empty-icon">inbox</mat-icon>
            <h3>No tasks found</h3>
            <p>Create a new task or adjust your filters.</p>
          </div>
        }
      }

      <button mat-fab color="primary" class="fab-button" (click)="createTask()">
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
      margin-bottom: 16px;
      h1 {
        margin: 0;
        font-size: 24px;
        font-weight: 500;
      }
    }
    .filter-bar {
      margin-bottom: 16px;
    }
    .filters {
      display: flex;
      gap: 12px;
      flex-wrap: wrap;
    }
    .filter-field {
      min-width: 160px;
    }
    .loading-container {
      display: flex;
      justify-content: center;
      padding: 48px;
    }
    .tasks-table {
      width: 100%;
    }
    .task-row {
      cursor: pointer;
    }
    .task-row:hover {
      background: rgba(0,0,0,0.02);
    }
    .task-key {
      font-family: monospace;
      font-size: 13px;
      color: #1976d2;
      font-weight: 500;
    }
    .priority-badge {
      font-size: 12px;
      font-weight: 500;
      padding: 2px 8px;
      border-radius: 12px;
    }
    .priority-critical { background: #ffebee; color: #c62828; }
    .priority-high { background: #fce4ec; color: #d32f2f; }
    .priority-medium { background: #fff3e0; color: #e65100; }
    .priority-low { background: #e8f5e9; color: #2e7d32; }
    .priority-lowest { background: #f5f5f5; color: #757575; }
    .overdue {
      color: #d32f2f;
      font-weight: 500;
    }
    .empty-state {
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
export class MyTasksPageComponent implements OnInit {
  tasks: TaskItem[] = [];
  projects: Project[] = [];
  loading = true;
  displayedColumns = ['taskKey', 'title', 'status', 'priority', 'dueDate'];
  statuses = Object.values(TaskItemStatus);
  priorities = Object.values(Priority);
  filterForm: FormGroup;

  constructor(
    private taskService: TaskService,
    private projectService: ProjectService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private fb: FormBuilder,
  ) {
    this.filterForm = this.fb.group({
      projectId: [''],
      status: [''],
      priority: [''],
    });
  }

  ngOnInit(): void {
    this.projectService.getAll().subscribe(projects => {
      this.projects = projects;
    });
    this.loadTasks();
  }

  loadTasks(): void {
    this.loading = true;
    const filters = this.filterForm.value;
    const params: { projectId?: string; status?: string; priority?: string } = {};
    if (filters.projectId) params.projectId = filters.projectId;
    if (filters.status) params.status = filters.status;
    if (filters.priority) params.priority = filters.priority;

    this.taskService.getAll(params).subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.snackBar.open('Failed to load tasks', 'Close', { duration: 3000 });
      },
    });
  }

  applyFilters(): void {
    this.loadTasks();
  }

  createTask(): void {
    const dialogRef = this.dialog.open(TaskDialogComponent, {
      data: {} as TaskDialogData,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.taskService.create(result).subscribe({
          next: () => {
            this.snackBar.open('Task created', 'Close', { duration: 3000 });
            this.loadTasks();
          },
          error: () => {
            this.snackBar.open('Failed to create task', 'Close', { duration: 3000 });
          },
        });
      }
    });
  }

  editTask(task: TaskItem): void {
    const dialogRef = this.dialog.open(TaskDialogComponent, {
      data: { task } as TaskDialogData,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.taskService.update(task.id, result).subscribe({
          next: () => {
            this.snackBar.open('Task updated', 'Close', { duration: 3000 });
            this.loadTasks();
          },
          error: () => {
            this.snackBar.open('Failed to update task', 'Close', { duration: 3000 });
          },
        });
      }
    });
  }
}
