import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { DatePipe } from '@angular/common';
import { TaskItem, Priority } from '../../../../core/models/task.model';

@Component({
  selector: 'app-task-card',
  standalone: true,
  imports: [MatCardModule, DatePipe],
  template: `
    <mat-card class="task-card" (click)="clicked.emit(task)" appearance="outlined">
      <mat-card-content class="card-content">
        <div class="card-header">
          <span class="task-key">{{ task.taskKey }}</span>
          <span class="priority-indicator" [class]="'priority-' + task.priority.toLowerCase()"></span>
        </div>
        <p class="task-title">{{ task.title }}</p>
        @if (task.dueDate) {
          <span class="due-date" [class.overdue]="task.isOverdue">
            {{ task.dueDate | date:'MMM d' }}
          </span>
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .task-card {
      cursor: pointer;
      margin-bottom: 8px;
      transition: box-shadow 0.2s;
    }
    .task-card:hover {
      box-shadow: 0 2px 8px rgba(0,0,0,0.15);
    }
    .card-content {
      padding: 4px 0;
    }
    .card-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 4px;
    }
    .task-key {
      font-size: 12px;
      color: #666;
      font-weight: 500;
    }
    .priority-indicator {
      width: 8px;
      height: 8px;
      border-radius: 50%;
    }
    .priority-critical { background: #d32f2f; }
    .priority-high { background: #f44336; }
    .priority-medium { background: #ff9800; }
    .priority-low { background: #4caf50; }
    .priority-lowest { background: #9e9e9e; }
    .task-title {
      margin: 0 0 4px 0;
      font-size: 14px;
      line-height: 1.3;
    }
    .due-date {
      font-size: 12px;
      color: #888;
    }
    .due-date.overdue {
      color: #d32f2f;
      font-weight: 500;
    }
  `],
})
export class TaskCardComponent {
  @Input({ required: true }) task!: TaskItem;
  @Output() clicked = new EventEmitter<TaskItem>();
}
