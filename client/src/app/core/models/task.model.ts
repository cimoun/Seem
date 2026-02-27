export interface TaskItem {
  id: string;
  taskKey: string;
  title: string;
  description?: string;
  status: TaskItemStatus;
  priority: Priority;
  projectId: string;
  boardColumnId?: string;
  parentTaskId?: string;
  dueDate?: string;
  startDate?: string;
  completedAt?: string;
  estimatedMinutes?: number;
  actualMinutes?: number;
  sortOrder: number;
  isOverdue: boolean;
  completionPercentage?: number;
  subtaskCount: number;
  completedSubtaskCount: number;
  tags: Tag[];
  createdAt: string;
  updatedAt?: string;
}

export interface Tag {
  id: string;
  name: string;
  color: string;
}

export enum TaskItemStatus {
  Todo = 'Todo',
  InProgress = 'InProgress',
  InReview = 'InReview',
  Blocked = 'Blocked',
  Done = 'Done',
  Cancelled = 'Cancelled',
}

export enum Priority {
  Lowest = 'Lowest',
  Low = 'Low',
  Medium = 'Medium',
  High = 'High',
  Critical = 'Critical',
}

export interface CreateTaskRequest {
  title: string;
  description?: string;
  priority: Priority;
  projectId: string;
  boardColumnId?: string;
  parentTaskId?: string;
  dueDate?: string;
  startDate?: string;
  estimatedMinutes?: number;
  tagIds?: string[];
}
