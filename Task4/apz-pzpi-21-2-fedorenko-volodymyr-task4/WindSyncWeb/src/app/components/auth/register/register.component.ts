import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { RegisterViewModel } from '../../../../assets/models/register.viewmodel';
import { LoginViewModel } from '../../../../assets/models/login.viewmodel';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
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

      this.authService.register(registerModel).subscribe(
        response => {
          console.log('Register successful', response);

          this.authService.login(registerModel as LoginViewModel)
            .subscribe(loginResult => {
              this.authService.setToken(loginResult);
              this.router.navigate(['']);
            });
        },
        error => {
          console.error('Register failed', error);
        }
      );
    }
  }
}
