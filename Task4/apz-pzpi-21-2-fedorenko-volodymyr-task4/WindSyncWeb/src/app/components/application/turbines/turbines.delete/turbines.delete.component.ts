import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-turbines.delete',
  standalone: true,
  imports: [
    MatDialogModule,
    MatButtonModule
  ],
  templateUrl: './turbines.delete.component.html',
  styleUrl: './turbines.delete.component.scss'
})
export class TurbinesDeleteComponent {
  constructor(public dialogRef: MatDialogRef<TurbinesDeleteComponent>) {}

  onConfirm() {
    this.dialogRef.close(true);
  }

  onCancel() {
    this.dialogRef.close(false);
  }
}
