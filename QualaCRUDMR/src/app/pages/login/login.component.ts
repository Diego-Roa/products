import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { AuthService } from '../../services/auth.service';
import { ResponseHttp, Token } from '../../interfaces/response-access';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, NzButtonModule, NzCheckboxModule, NzFormModule, NzInputModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  loginForm!: FormGroup;

  constructor(private readonly authService: AuthService, private fb: FormBuilder, private router: Router){
    this.loginForm = this.fb.group({
    username: this.fb.control('', [Validators.required]),
    password: this.fb.control('', [Validators.required]),
    remember: this.fb.control(true)
  });
  }

  submitForm(): void {
    if (this.loginForm.valid) {
      const request = {
        username: this.loginForm.get('username')?.value,
        password: this.loginForm.get('password')?.value,
        role: 'Administrator'
      }
      this.authService.login(request).subscribe({
        next: (response: ResponseHttp<Token>) => {
          if (response.result) {
            sessionStorage.setItem('access_token', response.data.token);
            this.router.navigateByUrl('home');
          }
        }
      })
    } else {
      Object.values(this.loginForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

}
