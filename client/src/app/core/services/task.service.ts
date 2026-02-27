import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { TaskItem, CreateTaskRequest } from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class TaskService {
  constructor(private api: ApiService) {}

  getAll(params?: { projectId?: string; status?: string; priority?: string }): Observable<TaskItem[]> {
    const queryParams: Record<string, string> = {};
    if (params?.projectId) queryParams['projectId'] = params.projectId;
    if (params?.status) queryParams['status'] = params.status;
    if (params?.priority) queryParams['priority'] = params.priority;
    return this.api.get<TaskItem[]>('/tasks', queryParams);
  }

  getById(id: string): Observable<TaskItem> {
    return this.api.get<TaskItem>(`/tasks/${id}`);
  }

  create(data: CreateTaskRequest): Observable<TaskItem> {
    return this.api.post<TaskItem>('/tasks', data);
  }

  update(id: string, data: Partial<TaskItem>): Observable<TaskItem> {
    return this.api.put<TaskItem>(`/tasks/${id}`, data);
  }

  delete(id: string): Observable<void> {
    return this.api.delete(`/tasks/${id}`);
  }

  changeStatus(id: string, status: string): Observable<TaskItem> {
    return this.api.patch<TaskItem>(`/tasks/${id}/status`, { status });
  }

  moveTask(id: string, columnId: string, sortOrder: number): Observable<TaskItem> {
    return this.api.patch<TaskItem>(`/tasks/${id}/move`, { boardColumnId: columnId, sortOrder });
  }
}
