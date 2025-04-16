import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { SnackBarService } from '../../services/snack-bar.service';
import { ErrorStandartize } from '../../utils/errorUtils';

import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'; 

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule ,MatProgressSpinnerModule],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent {
  form: FormGroup;
  isLogin = true; // toggle between sign in / sign up
  loading = false;
  error: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService ,private router: Router ,private snackBarService: SnackBarService) {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  toggleMode() {
    this.isLogin = !this.isLogin;
    this.error = null;
  }

  onSubmit() {
    if (this.form.invalid) return;
    this.loading = true;
    const { username, password } = this.form.value;

    const auth = this.isLogin
      ? this.authService.signIn({ username, password })
      : this.authService.signUp({ username, password });

    auth.subscribe({
      next: (res) => {
        this.loading = false;
        this.snackBarService.show('success', 'successfully signed in');
        this.router.navigate(['/product']);
      },
      error: (err) => {
        try {
          this.error =  ErrorStandartize(err)
        }catch{
          this.error =err.error?.message || 'somting goes wrong'
        }finally{
          this.snackBarService.show(`${this.isLogin ? "login" : "signup"} error` ,this.error || 'somting goes wrong')
          this.loading = false;
        }
      }
    });
  }
}
