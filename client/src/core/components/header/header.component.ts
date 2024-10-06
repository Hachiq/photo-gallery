import { Component, inject, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { JwtService } from '../../services/jwt.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  authService = inject(AuthService);
  jwtService = inject(JwtService);
  userId?: number;

  ngOnInit(): void {
    this.userId = this.jwtService.getUserId();
  }

  logout() {
    this.authService.logout();
  }
}
