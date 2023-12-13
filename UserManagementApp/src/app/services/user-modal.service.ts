import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserFormComponent } from '../user-form/user-form.component';
import { User } from '../models/user';
import { Observable, Subject, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserModalService {
  private userSaved = new Subject<{ user: User; isEditMode: boolean }>();
  private userCancelled = new Subject<void>();

  constructor(private dialog: MatDialog) {}

  openUserModal(user: User | null): Observable<User | undefined> {
    const dialogRef = this.dialog.open(UserFormComponent, {
      width: '500px',
      data: {
        user: user,
        isEditMode: !!user,
      },
    });

    // Subscribe to UserFormComponent events
    dialogRef.componentInstance.cancel.subscribe(() => {
      this.userCancelled.next();
      dialogRef.close();
    });
    dialogRef.componentInstance.save.subscribe((data) => {
      if (!data.user.consentGiven) {
        this.userSaved.error("Consent is required");
        return;
      }
      
      this.userSaved.next(data);
      dialogRef.close(user);
    });

    return dialogRef.afterClosed().pipe(
      map((result: User | undefined) => {
        return result;
      })
    );
  }

  getUserSaved(): Observable<any> {
    return this.userSaved.asObservable();
  }

  getUserCancelled(): Observable<void> {
    return this.userCancelled.asObservable();
  }
}
