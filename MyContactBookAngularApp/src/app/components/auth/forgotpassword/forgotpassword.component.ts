import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.css']
})
export class ForgotpasswordComponent {
  loading: boolean = false;
  username: string = '';
  password: string = '';
  confirmPassword : string = '';
  constructor(
    private authService: AuthService,
    private router: Router,
  ) { }
  checkPasswords(form: NgForm): void {
    const password = form.controls['password'];
    const confirmPassword = form.controls['confirmPassword'];
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
  }
  forgotpassword(): void {
    this.authService.forgetpassword(this.username, this.password,this.confirmPassword).subscribe({
      next: (response) => {
        if (response.success) {
          this.router.navigate(['/forgotpasswordsuccess']);
        } else {
          alert(response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        alert(err.error.message);
      }
    });
  }
}