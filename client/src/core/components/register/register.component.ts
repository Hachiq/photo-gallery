import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  form = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
    password : new FormControl('', [Validators.required, Validators.minLength(6)])
  })

  router = inject(Router);
  authService = inject(AuthService);

  register() {
    const data = {
      username: this.form.controls.username.value,
      password: this.form.controls.password.value,
    }

    this.authService.register(data).subscribe({
      next: () => {
        this.router.navigate(['login']);
      },
      error: (error) => {
        if (error.error.status === 409) {
          this.form.get('username')?.setErrors({ conflict: true });
        }
      }
    });
  }
}
