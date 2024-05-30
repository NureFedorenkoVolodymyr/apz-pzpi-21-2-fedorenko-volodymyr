import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-farms.delete',
  standalone: true,
  imports: [
    MatButtonModule,
    MatDialogModule
  ],
  templateUrl: './farms.delete.component.html',
  styleUrl: './farms.delete.component.scss'
})
export class FarmsDeleteComponent {
  constructor(public dialogRef: MatDialogRef<FarmsDeleteComponent>) {}

  onConfirm() {
    this.dialogRef.close(true);
  }

  onCancel() {
    this.dialogRef.close(false);
  }
}
