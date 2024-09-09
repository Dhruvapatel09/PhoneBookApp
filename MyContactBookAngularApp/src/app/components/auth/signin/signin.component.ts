import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { LocalstorageService } from 'src/app/services/helpers/localstorage.service';
import { LocalstorageKeys } from 'src/app/services/helpers/localstoragekeys';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent {
  loading: boolean = false;
  username:string='';
  password:string='';

  constructor(private authService:AuthService,
    private localStorageHelper:LocalstorageService,
    private router:Router,
    private cdr:ChangeDetectorRef
  ){}

    login(){
      this.loading=true;
      this.authService.signIn(this.username,this.password).subscribe({
         next:(response)=>{
          if(response.success){
            this.localStorageHelper.setItem(LocalstorageKeys.TokenName,response.data);
            this.localStorageHelper.setItem(LocalstorageKeys.UserId,this.username);
            this.cdr.detectChanges();//manually trigger change detection
            this.router.navigate(['/home'])
          }else{
            alert(response.message);
          }
          this.loading=false;

         },
         error:(err)=>{
          this.loading=false;
          alert(err.error.message);
         }
      });
    }
}

