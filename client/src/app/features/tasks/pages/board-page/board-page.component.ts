import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TaskItem } from '../../../../core/models/task.model';
import { BoardColumn } from '../../../../core/models/project.model';
import { BoardService } from '../../../../core/services/board.service';
import { TaskService } from '../../../../core/services/task.service';
import { TaskCardComponent } from '../../components/task-card/task-card.component';
import { TaskDialogComponent, TaskDialogData } from '../../components/task-dialog/task-dialog.component';

interface ColumnWithTasks {
  column: BoardColumn;
  tasks: TaskItem[];
}

@Component({
  selector: 'app-board-page',
  standalone: true,
  imports: [
    DragDropModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    TaskCardComponent,
  ],
  template: `
    @if (loading) {
      <div class="loading-container">
        <mat-spinner diameter="48"></mat-spinner>
      </div>
    } @else {
      <div class="board-header">
        <h1>{{ boardName }}</h1>
      </div>
      <div class="board-container" cdkDropListGroup>
        @for (col of columns; track col.column.id) {
          <div class="board-column">
            <div class="column-header">
              <h3 class="column-title">{{ col.column.name }}</h3>
              <span class="task-count">{{ col.tasks.length }}</span>
            </div>
            <div class="column-tasks"
                 cdkDropList
                 [cdkDropListData]="col.tasks"
                 [id]="col.column.id"
                 (cdkDropListDropped)="onDrop($event, col.column)">
              @for (task of col.tasks; track task.id) {
                <div cdkDrag [cdkDragData]="task">
                  <app-task-card [task]="task" (clicked)="editTask($event)"></app-task-card>
                </div>
              }
            </div>
            <button mat-button class="add-task-btn" (click)="addTask(col.column)">
              <mat-icon>add</mat-icon>
              Add task
            </button>
          </div>
        }
      </div>
    }
  `,
  styles: [`
    .loading-container {
      display: flex;
      justify-content: center;
      padding: 48px;
    }
    .board-header {
      padding: 16px 24px 0;
      h1 {
        margin: 0;
        font-size: 24px;
        font-weight: 500;
      }
    }
    .board-container {
      display: flex;
      gap: 16px;
      padding: 16px 24px;
      overflow-x: auto;
      min-height: calc(100vh - 140px);
      align-items: flex-start;
    }
    .board-column {
      min-width: 280px;
      max-width: 320px;
      background: #f5f5f5;
      border-radius: 8px;
      display: flex;
      flex-direction: column;
    }
    .column-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 12px 16px;
      border-bottom: 1px solid #e0e0e0;
    }
    .column-title {
      margin: 0;
      font-size: 14px;
      font-weight: 600;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }
    .task-count {
      background: #e0e0e0;
      border-radius: 12px;
      padding: 2px 8px;
      font-size: 12px;
      font-weight: 500;
    }
    .column-tasks {
      padding: 8px;
      min-height: 60px;
      flex: 1;
    }
    .add-task-btn {
      margin: 4px 8px 8px;
      color: #888;
      justify-content: flex-start;
    }
    .cdk-drag-preview {
      box-shadow: 0 4px 16px rgba(0,0,0,0.2);
      border-radius: 4px;
    }
    .cdk-drag-placeholder {
      opacity: 0.3;
    }
    .cdk-drag-animating {
      transition: transform 250ms cubic-bezier(0, 0, 0.2, 1);
    }
    .cdk-drop-list-dragging .cdk-drag {
      transition: transform 250ms cubic-bezier(0, 0, 0.2, 1);
    }
  `],
})
export class BoardPageComponent implements OnInit {
  boardId = '';
  boardName = '';
  projectId = '';
  columns: ColumnWithTasks[] = [];
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private boardService: BoardService,
    private taskService: TaskService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
  ) {}

  ngOnInit(): void {
    this.boardId = this.route.snapshot.paramMap.get('id') || '';
    this.loadBoard();
  }

  loadBoard(): void {
    this.loading = true;
    this.boardService.getBoard(this.boardId).subscribe({
      next: (board) => {
        this.boardName = board.name;
        this.projectId = board.projectId;
        const sortedColumns = (board.columns || []).sort((a, b) => a.position - b.position);
        this.columns = sortedColumns.map(column => ({
          column,
          tasks: (board.tasks?.[column.id] || []) as TaskItem[],
        }));
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.snackBar.open('Failed to load board', 'Close', { duration: 3000 });
      },
    });
  }

  onDrop(event: CdkDragDrop<TaskItem[]>, targetColumn: BoardColumn): void {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
    }

    const task = event.container.data[event.currentIndex];
    this.taskService.moveTask(task.id, targetColumn.id, event.currentIndex).subscribe({
      error: () => {
        this.snackBar.open('Failed to move task', 'Close', { duration: 3000 });
        this.loadBoard();
      },
    });
  }

  addTask(column: BoardColumn): void {
    const dialogRef = this.dialog.open(TaskDialogComponent, {
      data: {
        projectId: this.projectId,
        boardColumnId: column.id,
      } as TaskDialogData,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.taskService.create(result).subscribe({
          next: () => {
            this.snackBar.open('Task created', 'Close', { duration: 3000 });
            this.loadBoard();
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
            this.loadBoard();
          },
          error: () => {
            this.snackBar.open('Failed to update task', 'Close', { duration: 3000 });
          },
        });
      }
    });
  }
}
