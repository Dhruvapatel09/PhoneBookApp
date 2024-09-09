import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UpdatePassword } from 'src/app/models/changepassword';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-changepassword',
  templateUrl: './changepassword.component.html',
  styleUrls: ['./changepassword.component.css']
})
export class ChangepasswordComponent {
  username:string|null|undefined;
  user:UpdatePassword = {
    password: '',
    confirmPassword: '',
    username: ''
  };
  loading=false;
  constructor(private authService:AuthService, private router:Router, private route:ActivatedRoute) {}

  ngOnInit(): void {
    this.authService.getUsername().subscribe((username:string|null|undefined)=>{
      this.username = username;
    })
  }

  checkPasswords(form: NgForm):void {
    const password = form.controls['password'];
    const confirmPassword = form.controls['confirmPassword'];
 
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
  }

  onSubmit(updatePasswordForm:NgForm):void{
    if(updatePasswordForm.valid){
      this.user.username = this.username;
      this.authService.passwordDiscovery(this.user).subscribe({
        next:(response)=>{
          if(response.success){
            alert("password changed successfully:",);
            this.router.navigate(['/signin']);
          }else{
            alert(response.message);
          }
          this.loading=false;
        },
        error:(err)=>{
          console.error('Failed to update password');
          alert(err.error.message);
        },
        complete:()=>{
          console.log('completed');
        }
      });
      this.router.navigate(['/contacts-pagination',this.username]);
    }
  }
}

