import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { UserService } from '../services/user.service';
import { User } from '../models/user';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { UserDetailComponent } from '../user-detail/user-detail.component';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../shared/confirm-dialog/confirm-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserModalService } from '../services/user-modal.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})
export class UserListComponent implements OnInit, AfterViewInit {
  users: User[] = [];
  pageSize = 10;
  pageIndex = 0;
  displayedColumns: string[] = ['firstName', 'lastName', 'email', 'actions'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  selectedUser: User | null = null;

  constructor(
    private userService: UserService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private userModalService: UserModalService
  ) {}

  ngOnInit(): void {
    this.userModalService.getUserSaved().subscribe((data) => {
      if (data.isEditMode) {
        // User is updated
        data.user.consentDate = new Date();
        this.userService.updateUser(data.user.id, data.user).subscribe(
          () => {
            this.showSuccessToast('User updated successfully');
          },
          (error) => {
            console.error('Error updating user:', error);
          },
          () => {
            this.loadUsers();
          }
        );
      } else {
        // User is created
        this.userService.createUser(data.user).subscribe(
          () => {
            this.showSuccessToast('User successfully created');
          },
          (error) => {
            console.error('Error creating user:', error);
          },
          () => {
            this.loadUsers();
          }
        );
      }
    });
  }

  ngAfterViewInit(): void {
    // Load users and set up paginator after the view is initialized
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAllUsers().subscribe((users) => {
      this.users = users;

      // Check if paginator is defined before setting its length
      if (this.paginator) {
        this.paginator.length = users.length;
      } else {
        console.error('Paginator is undefined');
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadUsers();
  }

  selectUser(user: User): void {
    const dialogRef = this.dialog.open(UserDetailComponent, {
      width: '500px',
      data: user,
      panelClass: 'custom-dialog-container',
    });
  }
  openUserForm(user: User | null): void {
    this.userModalService.openUserModal(user).subscribe();
  }

  deleteUser(id: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
        title: 'Are you sure?',
        message: 'You are about to delete this user...',
      },
    });

    dialogRef.afterClosed().subscribe((dialogResult) => {
      if (dialogResult) {
        this.userService.deleteUser(id).subscribe(
          () => {
            this.showSuccessToast('User deleted successfully');
            this.loadUsers();
          },
          (error) => {
            console.error('Error deleting user', error);
          }
        );
      }
    });
  }

  private showSuccessToast(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000, // Duration in milliseconds
      panelClass: ['mat-toolbar', 'mat-primary'],
    });
  }
}
