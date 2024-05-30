import { Component, OnInit, inject } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { UserViewModel } from '../../../../assets/models/user.viewmodel';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { UsersDeleteComponent } from '../users.delete/users.delete.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-admins.view',
  standalone: true,
  imports: [
    MatTableModule,
    MatButtonModule,
    RouterLink
  ],
  templateUrl: './admins.view.component.html',
  styleUrl: './admins.view.component.scss'
})
export class AdminsViewComponent implements OnInit {
  private authService = inject(AuthService);
  private dialog = inject(MatDialog);

  users: UserViewModel[] = [];

  displayedColumns: string[] = ['id', 'userName', 'email', 'delete'];

  ngOnInit(): void {
    this.refreshUserList();
  }

  refreshUserList(){
    this.authService.getAdmins()
      .subscribe(result => {
        this.users = result;
      });
  }

  onUserDelete(id: string): void {
    const dialogRef = this.dialog.open(UsersDeleteComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.authService.deleteUser(id)
          .subscribe(() => {
            this.refreshUserList();
          });
      }
    });
  }
}
