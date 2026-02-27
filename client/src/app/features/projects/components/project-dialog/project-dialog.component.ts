import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { Project } from '../../../../core/models/project.model';

export interface ProjectDialogData {
  project?: Project;
}

const PRESET_COLORS = [
  { label: 'Blue', value: '#1976d2' },
  { label: 'Teal', value: '#009688' },
  { label: 'Green', value: '#4caf50' },
  { label: 'Orange', value: '#ff9800' },
  { label: 'Red', value: '#f44336' },
  { label: 'Purple', value: '#9c27b0' },
  { label: 'Indigo', value: '#3f51b5' },
  { label: 'Pink', value: '#e91e63' },
];

@Component({
  selector: 'app-project-dialog',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
  ],
  template: `
    <h2 mat-dialog-title>{{ data.project ? 'Edit Project' : 'New Project' }}</h2>
    <mat-dialog-content>
      <form [formGroup]="form" class="dialog-form">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Project Name</mat-label>
          <input matInput formControlName="name" (input)="onNameInput()" />
          @if (form.get('name')?.hasError('required')) {
            <mat-error>Name is required</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Key</mat-label>
          <input matInput formControlName="key" maxlength="5"
                 style="text-transform: uppercase;" />
          @if (form.get('key')?.hasError('required')) {
            <mat-error>Key is required</mat-error>
          }
          @if (form.get('key')?.hasError('maxlength')) {
            <mat-error>Key must be 1-5 characters</mat-error>
          }
          @if (form.get('key')?.hasError('pattern')) {
            <mat-error>Key must be uppercase letters only</mat-error>
          }
          <mat-hint>Auto-generated from name</mat-hint>
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Description</mat-label>
          <textarea matInput formControlName="description" rows="3"></textarea>
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Color</mat-label>
          <mat-select formControlName="color">
            @for (c of presetColors; track c.value) {
              <mat-option [value]="c.value">
                <span class="color-option">
                  <span class="color-swatch" [style.background]="c.value"></span>
                  {{ c.label }}
                </span>
              </mat-option>
            }
          </mat-select>
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary" (click)="onSave()"
              [disabled]="form.invalid">
        {{ data.project ? 'Update' : 'Create' }}
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    .dialog-form {
      display: flex;
      flex-direction: column;
      min-width: 400px;
      padding-top: 8px;
    }
    .full-width {
      width: 100%;
    }
    .color-option {
      display: flex;
      align-items: center;
      gap: 8px;
    }
    .color-swatch {
      width: 16px;
      height: 16px;
      border-radius: 50%;
      display: inline-block;
      border: 1px solid rgba(0,0,0,0.12);
    }
  `],
})
export class ProjectDialogComponent {
  form: FormGroup;
  presetColors = PRESET_COLORS;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ProjectDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ProjectDialogData,
  ) {
    const project = data.project;
    this.form = this.fb.group({
      name: [project?.name || '', Validators.required],
      key: [project?.key || '', [Validators.required, Validators.maxLength(5), Validators.pattern(/^[A-Z]+$/)]],
      description: [project?.description || ''],
      color: [project?.color || PRESET_COLORS[0].value],
    });

    if (project) {
      this.form.get('key')?.disable();
    }
  }

  onNameInput(): void {
    if (!this.data.project) {
      const name: string = this.form.get('name')?.value || '';
      const key = name
        .split(/\s+/)
        .filter(w => w.length > 0)
        .map(w => w[0].toUpperCase())
        .join('')
        .slice(0, 5);
      this.form.get('key')?.setValue(key);
    }
  }

  onSave(): void {
    if (this.form.valid) {
      const value = this.form.getRawValue();
      value.key = value.key.toUpperCase();
      this.dialogRef.close(value);
    }
  }
}
