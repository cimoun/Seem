import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Board } from '../models/project.model';

@Injectable({ providedIn: 'root' })
export class BoardService {
  constructor(private api: ApiService) {}

  getBoard(id: string): Observable<Board & { tasks: Record<string, any[]> }> {
    return this.api.get<Board & { tasks: Record<string, any[]> }>(`/boards/${id}`);
  }
}
