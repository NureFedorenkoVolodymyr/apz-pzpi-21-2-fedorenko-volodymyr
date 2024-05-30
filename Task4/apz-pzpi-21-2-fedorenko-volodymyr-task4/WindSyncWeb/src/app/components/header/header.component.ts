import { Component, OnInit, inject } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    RouterLink,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  authService = inject(AuthService);
  router = inject(Router);

  isAuthenticated: boolean = false;
  currentEmail: string = '';

  ngOnInit(): void {
    this.authService.isAuthenticatedListener()
      .subscribe(authState => {
        this.isAuthenticated = authState;
      });

    this.authService.currentEmailListener()
      .subscribe(email => {
        this.currentEmail = email;
      });
  }

  onLogout() {
    this.authService.removeToken();
    this.router.navigate(['']);
  }
}
