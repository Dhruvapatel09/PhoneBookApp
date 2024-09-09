import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { SigninComponent } from './components/auth/signin/signin.component';
import { SignupComponent } from './components/auth/signup/signup.component';
import { ContactListComponent } from './components/contact/contact-list/contact-list.component';
import { AddContactComponent } from './components/contact/add-contact/add-contact.component';
import { EditContactComponent } from './components/contact/edit-contact/edit-contact.component';
import { ContactDetailsComponent } from './components/contact/contact-details/contact-details.component';
import { authGuard } from './guard/auth.guard';
import { SignupsuccessComponent } from './components/auth/signupsuccess/signupsuccess.component';
import { ContactListPaginationComponent } from './components/contact/contact-list-pagination/contact-list-pagination.component';
import { ContactListFavouritesComponent } from './components/contact/contact-list-favourites/contact-list-favourites.component';
import { ForgotpasswordComponent } from './components/auth/forgotpassword/forgotpassword.component';
import { ForgotpasswordsuccessComponent } from './components/auth/forgotpasswordsuccess/forgotpasswordsuccess.component';
import { ChangepasswordComponent } from './components/auth/changepassword/changepassword.component';
import { EdituserComponent } from './components/auth/edituser/edituser.component';
import { AddContactRfComponent } from './components/contact/add-contact-rf/add-contact-rf.component';
import { EditContactRfComponent } from './components/contact/edit-contact-rf/edit-contact-rf.component';

const routes: Routes = [
  {path:'',redirectTo:'home',pathMatch:'full'},
  {path:'home',component:HomeComponent},
  {path:'privacy',component:PrivacyComponent},
  {path:'contacts',component:ContactListComponent},
  {path:'signin',component:SigninComponent},
  {path:'signup',component:SignupComponent},
  {path:'forgotpassword',component:ForgotpasswordComponent},  
  {path:'forgotpasswordsuccess',component:ForgotpasswordsuccessComponent},  
  {path:'addcontact',component:AddContactComponent,canActivate:[authGuard]},
  {path:'editcontact/:phoneId',component:EditContactComponent,canActivate:[authGuard]},
  {path:'contactdetails/:phoneId',component:ContactDetailsComponent},
  // {path:'contactdelete/:phoneId',component:ContactDetailsComponent},
  {path:'signupsuccess',component:SignupsuccessComponent},
  {path:'contacts-pagination',component:ContactListPaginationComponent},
  {path:'contacts-favourites',component:ContactListFavouritesComponent},
  {path:'changepassword',component:ChangepasswordComponent},
  {path:'edituser/:name',component:EdituserComponent},
  {path:'addcontactrf',component:AddContactRfComponent},
  {path:'editcontactrf',component:EditContactRfComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
