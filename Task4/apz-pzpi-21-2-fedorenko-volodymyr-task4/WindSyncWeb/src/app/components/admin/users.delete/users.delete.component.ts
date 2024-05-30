import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-users-delete',
  standalone: true,
  imports: [
    MatButtonModule,
    MatDialogModule
  ],
  templateUrl: './users.delete.component.html',
  styleUrl: './users.delete.component.scss'
})
export class UsersDeleteComponent {
  constructor(public dialogRef: MatDialogRef<UsersDeleteComponent>) {}

  onConfirm() {
    this.dialogRef.close(true);
  }

  onCancel() {
    this.dialogRef.close(false);
  }
}
