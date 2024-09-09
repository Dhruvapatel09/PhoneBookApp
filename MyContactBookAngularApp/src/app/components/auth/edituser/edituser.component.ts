import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { EditUser } from 'src/app/models/edituser.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-edituser',
  templateUrl: './edituser.component.html',
  styleUrls: ['./edituser.component.css']
})
export class EdituserComponent {
  user : EditUser={
    userId : 0,
    firstName : '',
    lastName :'',
    loginId: '',
    email:'',
    contactNumber: '',
    password: '',
    confirmPassword : '',
    file : '',
    fileName : null
  };
  imageUrl: string | ArrayBuffer | null = null;
  loading : boolean = false;
  @ViewChild('imageInput') imageInput!: ElementRef;


  constructor(
    private authService : AuthService,
    private router :Router,
  private route : ActivatedRoute

  ){}
  ngOnInit(): void {
    const loginId = this.route.snapshot.paramMap.get('name')!;
   this.loacontactDetails(loginId);
  } 

  loacontactDetails(loginId:string):void{
    this.authService.fetchUserByloginId(loginId).subscribe({
      next: (response) => {
        if (response.success) {
          // Parse the date string received from the server
          console.log(response.data);
          this.user = response.data;
         
          // Set imageUrl if contact has an image
          if (this.user.file) {
            this.imageUrl = 'data:image/jpeg;base64,' + this.user.file;
          }
        } else {
          console.error('Failed to fetch user: ', response.message)
        }
      },
      error: (err) => {
        alert(err.error.message);
      },
      complete: () => {
        console.log('Completed');
      }
    });
  }

  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        // Store the file byte as a base64 string
        this.user.file = reader.result?.toString().split(',')[1] || '';
        this.imageUrl = reader.result; // Set the image preview URL
         this.user.fileName = file.name;
         console.log(this.user.fileName);
      };
      reader.readAsDataURL(file); // Read the file as a data URL
    }
  }
  
  removeFile() {
    this.imageUrl = null; // Clear the imageUrl variable to remove the image
    // You may want to also clear any associated form data here if needed
    this.user.fileName = null;
    this.imageInput.nativeElement.value = '';
}

onSubmit(form :NgForm){
  if(form.valid )
  {
   
    if (this.imageUrl === null) {
      // If file has been removed, clear the imageByte and fileName in the contact object
      this.user.file = '';
      this.user.fileName = null;
    }
    this.authService.editUser(this.user)
    .subscribe({ 
     next: (response : ApiResponse<EditUser>) => {
       console.log(response.data)
       if(response.success)
      {
       alert("user updated successfully:", );
       this.router.navigate(['/contacts-pagination']);
      
       
      }
      else if (!response.success){
       alert(response.message)

     }
  },
  error:(error) => {
    console.error("Error occurred while updating contact:", error);
    alert(error.error.message)
   
  }

});
}
}

}

