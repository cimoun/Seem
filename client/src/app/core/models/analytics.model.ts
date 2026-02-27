export interface DashboardStats {
  totalTasks: number;
  completedTasks: number;
  overdueTasks: number;
  tasksCompletedToday: number;
  averageCompletionMinutes: number;
  completionRate: number;
}

export interface ProductivityData {
  date: string;
  tasksCompleted: number;
  averageMinutes: number;
  completedLate: number;
}

export interface CompletionStats {
  status: string;
  count: number;
  percentage: number;
}

export interface DeadlineForecast {
  taskId: string;
  taskKey: string;
  title: string;
  dueDate: string;
  estimatedCompletionDate: string;
  isAtRisk: boolean;
}
