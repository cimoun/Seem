export interface Project {
  id: string;
  name: string;
  description?: string;
  key: string;
  color: string;
  isArchived: boolean;
  nextTaskNumber: number;
  boards: Board[];
  taskCount: number;
  createdAt: string;
  updatedAt?: string;
}

export interface Board {
  id: string;
  name: string;
  projectId: string;
  columns: BoardColumn[];
  createdAt: string;
}

export interface BoardColumn {
  id: string;
  name: string;
  position: number;
  mappedStatus: string;
  wipLimit?: number;
  boardId: string;
}
