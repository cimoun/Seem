import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Project } from '../models/project.model';

@Injectable({ providedIn: 'root' })
export class ProjectService {
  constructor(private api: ApiService) {}

  getAll(): Observable<Project[]> {
    return this.api.get<Project[]>('/projects');
  }

  getById(id: string): Observable<Project> {
    return this.api.get<Project>(`/projects/${id}`);
  }

  create(data: Partial<Project>): Observable<Project> {
    return this.api.post<Project>('/projects', data);
  }

  update(id: string, data: Partial<Project>): Observable<Project> {
    return this.api.put<Project>(`/projects/${id}`, data);
  }

  archive(id: string): Observable<void> {
    return this.api.patch<void>(`/projects/${id}/archive`);
  }
}
