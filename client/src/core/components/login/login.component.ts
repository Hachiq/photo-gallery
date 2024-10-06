import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CONFIGURATION } from '../../configuration/config';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  form = new FormGroup({
    username: new FormControl(''),
    password : new FormControl('')
  })

  router = inject(Router);
  authService = inject(AuthService);

  login() {
    const data = {
      username: this.form.controls.username.value,
      password: this.form.controls.password.value,
    }

    this.authService.login(data).subscribe({
      next: (response) => {
        localStorage?.setItem(CONFIGURATION.auth.tokenKey, response);
        location.href = 'albums';
      },
      error: (error) => {
        try {
          const parsedResponse = JSON.parse(error.error);
          if (parsedResponse.status === 400) {
            this.form.setErrors({ invalid: true });
          }
        } catch {
          console.error(error);
        }
      }
    });
  }
}
