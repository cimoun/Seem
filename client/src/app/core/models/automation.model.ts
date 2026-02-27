export interface AutomationRule {
  id: string;
  name: string;
  isEnabled: boolean;
  projectId?: string;
  trigger: RuleTrigger;
  actions: RuleAction[];
  executionCount: number;
  createdAt: string;
}

export interface RuleTrigger {
  type: TriggerType;
  conditions: Record<string, unknown>;
}

export interface RuleAction {
  id: string;
  type: ActionType;
  order: number;
  parameters: Record<string, unknown>;
}

export enum TriggerType {
  OnTaskCreated = 'OnTaskCreated',
  OnStatusChanged = 'OnStatusChanged',
  OnDueDateApproaching = 'OnDueDateApproaching',
  OnTaskOverdue = 'OnTaskOverdue',
  OnSchedule = 'OnSchedule',
  OnTagAdded = 'OnTagAdded',
}

export enum ActionType {
  ChangeStatus = 'ChangeStatus',
  SetPriority = 'SetPriority',
  AddTag = 'AddTag',
  SendReminder = 'SendReminder',
  CreateTask = 'CreateTask',
  MoveToColumn = 'MoveToColumn',
  AssignDueDate = 'AssignDueDate',
}

export interface RecurringTask {
  id: string;
  title: string;
  pattern: RecurrencePattern;
  cronExpression?: string;
  nextOccurrence: string;
  projectId?: string;
  isEnabled: boolean;
  createdAt: string;
}

export enum RecurrencePattern {
  Daily = 'Daily',
  Weekly = 'Weekly',
  Biweekly = 'Biweekly',
  Monthly = 'Monthly',
  Quarterly = 'Quarterly',
  Yearly = 'Yearly',
  Custom = 'Custom',
}

export interface Reminder {
  id: string;
  message: string;
  remindAt: string;
  isDismissed: boolean;
  taskItemId?: string;
  createdAt: string;
}
