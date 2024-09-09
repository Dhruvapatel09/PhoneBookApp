import { ChangeDetectorRef, Component } from '@angular/core';
import { AuthService } from './services/auth.service';
import { EditUser } from './models/edituser.model';
import { ApiResponse } from './models/ApiResponse{T}';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'MyContactBookAngularApp';
  isAuthenticated:boolean=false;
  username: string | null | undefined;
  loginId: string | undefined;
  user: EditUser | null = null;
  imageUrl: string | ArrayBuffer | null = null;
  private userSubscription: Subscription | undefined;
  constructor(private authService: AuthService,private cdr:ChangeDetectorRef) { }
  // ngOnInit(): void {
  //   this.authService.isAuthenticated().subscribe((authState:boolean)=>{
  //     this.isAuthenticated=authState;
  //     this.cdr.detectChanges()//manually trigger detection
  //   });
  //   this.authService.getUsername().subscribe((username:string|null|undefined)=>{
  //     this.username=username;
  //     this.cdr.detectChanges()//manually trigger detection
  //   });
  //  
  //   const loginId = this.username;
  //   this.authService.fetchUserByloginId(loginId).subscribe(
  //     (response: ApiResponse<EditUser>) => {
  //       Extract the userId from the response
  //       if (response.data) {
  //         this.user = response.data;
          
          
  //       }
  //     },
      
  //     error => {
  //       console.error('Error fetching user:', error);
  //     });
      
    
  //   this.cdr.detectChanges();
    
  //   }
  signOut() {
    this.authService.SignOut();
  }
  ngOnInit(): void {
    this.userSubscription = this.authService.getUsername().subscribe((username: string | null | undefined) => {
      this.username = username;
      if (this.username) {
        this.fetchUserDetails(this.username);
      }
      this.cdr.detectChanges(); //Manually trigger change detection.
    });

    this.authService.isAuthenticated().subscribe((authState: boolean) => {
      this.isAuthenticated = authState;
      this.cdr.detectChanges(); //Manually trigger change detection.
    });
  }

  ngOnDestroy(): void {
    // Unsubscribe to prevent memory leaks
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }
  fetchUserDetails(loginId: string): void {
    this.authService.fetchUserByloginId(loginId).subscribe(
      (response: ApiResponse<EditUser>) => {
        if (response.success && response.data) {
          this.user = response.data;
          if (this.user.file) {
            this.imageUrl = 'data:image/jpeg;base64,' + this.user.file;
          } else {
            this.imageUrl = null;
          }
        }
      },
      error => {
        console.error('Error fetching user:', error);
      }
    );
  }
}
