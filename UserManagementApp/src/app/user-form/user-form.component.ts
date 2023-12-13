// user-form.component.ts
import {
  Component,
  EventEmitter,
  Inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../models/user';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css'],
})
export class UserFormComponent implements OnInit {
  @Input() user!: User;
  @Output() cancel = new EventEmitter<void>();
  @Output() save = new EventEmitter<{ user: User; isEditMode: boolean }>();
  isEditMode: boolean = false;

  userForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.userForm = this.fb.group({
      id: [this.user?.id],
      firstName: [this.user?.firstName, Validators.required],
      lastName: [this.user?.lastName, Validators.required],
      contact: [this.user?.contact || ''],
      country: [this.user?.country || ''],
      email: [this.user?.email, [Validators.required, Validators.email]],
      consentGiven: [
        this.user?.consentGiven,
        [Validators.required, Validators.pattern('true')],
      ],
      consentDate: [this.user?.consentDate],
    });

    this.isEditMode = this.data.isEditMode;
    if (this.data.isEditMode) {
      this.userForm.setValue(this.data.user);
    }
  }

  onSubmit(): void {
    if (this.userForm.invalid) {
      // Handle form errors
      return;
    }

    if (this.userForm.valid) {
      const updatedUser: User = { ...this.user, ...this.userForm.value };
      this.save.emit({ user: updatedUser, isEditMode: this.isEditMode });
    }
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
