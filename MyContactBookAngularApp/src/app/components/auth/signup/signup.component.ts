import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  user = {
    userId: 0,
    firstName: '',
    lastName: '',
    loginId: '',
    email: '',
    contactNumber: '',
    password: '',
    confirmPassword: '',
    fileName : null,
    imageByte:''
  };
  loading: boolean = false;
  imageUrl: string | ArrayBuffer | null = null;  
  @ViewChild('imageInput') imageInput!: ElementRef;
  constructor(private authService: AuthService, private router: Router) { }

  checkPasswords(form: NgForm): void {
    const password = form.controls['password'];
    const confirmPassword = form.controls['confirmPassword'];

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
  }

  onSubmit(signUpForm: NgForm): void {
    if (signUpForm.valid) {
      this.loading=true;
      console.log(signUpForm.value);
      if (this.imageUrl === null) {
        // If file has been removed, clear the imageByte and fileName in the contact object
        this.user.imageByte = '';
        this.user.fileName = null;
      }
      this.authService.signUp(this.user).subscribe({
        next: (response) => {
          if (response.success) {
            this.router.navigate(['/signupsuccess']);
          } else {
            alert(response.message)
          }
          this.loading=false;
        },
        error: (err) => {
          console.error(err.error.message);
          alert(err.error.message);
          this.loading = false;
        },
        complete:() =>{
          this.loading = false;
          console.log("completed");
        }
      });
    }
  }
  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.user.imageByte = (reader.result as string).split(',')[1]; 
        this.user.fileName = file.name;
        this.imageUrl = reader.result; 
      };
      reader.readAsDataURL(file);
    }
    
  }
  removeFile() {
    this.imageUrl = null; // Clear the imageUrl variable to remove the image
    // You may want to also clear any associated form data here if needed
    this.user.fileName = null;
    this.imageInput.nativeElement.value = '';
}

}

