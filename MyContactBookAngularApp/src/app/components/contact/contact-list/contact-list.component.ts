import { ChangeDetectorRef, Component } from '@angular/core';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { AuthService } from 'src/app/services/auth.service';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})
export class ContactListComponent {
  contacts: Contact[] | undefined;
  phoneId: number | undefined;
  loading:boolean=false;
  isAuthenticated:boolean=false;
  username:string |null|undefined;
  constructor(private contactService: ContactService,private authService:AuthService,private cdr:ChangeDetectorRef) {

  }
  ngOnInit(): void {
    this.loadContacts();
    this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated=authState;
      this.cdr.detectChanges();
     });
     this.authService.getUsername().subscribe((username:string |null|undefined)=>{
      this.username=username;
      this.cdr.detectChanges();
     });
  }
 
  loadContacts():void{
    this.loading = true;
    this.contactService.getAllContacts().subscribe({
      next:(response: ApiResponse<Contact[]>) =>{
        if(response.success){
          this.contacts = response.data;
        }
        else{
          console.error('Failed to fetch contacts', response.message);
        }
        this.loading = false;
      },
      error:(error => {
        console.error('Failed to fetch contacts', error);
        this.loading = false;
      })
    }
  )
  }
  confirmDelete(phoneId: number): void {
    if (confirm('Are you sure want to delete ?')) {
      // alert('yes');
      this.loading = true;
      this.phoneId = phoneId;
      this.deleteCategory();
    }
  }
  deleteCategory(): void {

      this.contactService.deleteContactById(this.phoneId).subscribe({
        next: (response) => {
          if (response.success) {
            // alert(response.message);
            this.loadContacts();
          } else {
            alert(response.message);
          }
        },
        error: (err) => {
          this.loading = false;
          alert(err.error.message);
        },
        complete: () => {
          this.loading = false;
          console.log("Completed");
        }
      })
    }
  
  }