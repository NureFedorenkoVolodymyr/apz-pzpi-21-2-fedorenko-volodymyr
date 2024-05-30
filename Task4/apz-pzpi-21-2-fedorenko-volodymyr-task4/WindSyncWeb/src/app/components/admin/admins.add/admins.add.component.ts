import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { RegisterViewModel } from '../../../../assets/models/register.viewmodel';


@Component({
  selector: 'app-admins.add',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './admins.add.component.html',
  styleUrl: './admins.add.component.scss'
})
export class AdminsAddComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  registerForm = this.fb.group({
    username: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  onSubmit(): void {
    if (this.registerForm.valid) {
      const registerModel = this.registerForm.value as RegisterViewModel;

      this.authService.registerAdmin(registerModel).subscribe(
        response => {
          console.log('Register admin successful', response);

          this.router.navigate(['admin', 'admins']);
        },
        error => {
          console.error('Register admin failed', error);
        }
      );
    }
  }
}
