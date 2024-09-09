import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { SignupComponent } from './components/auth/signup/signup.component';
import { SigninComponent } from './components/auth/signin/signin.component';
import { HttpClientModule } from '@angular/common/http';
import { ContactListComponent } from './components/contact/contact-list/contact-list.component';
import { AddContactComponent } from './components/contact/add-contact/add-contact.component';
import { EditContactComponent } from './components/contact/edit-contact/edit-contact.component';
import { ContactDetailsComponent } from './components/contact/contact-details/contact-details.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SignupsuccessComponent } from './components/auth/signupsuccess/signupsuccess.component';
import { ContactListPaginationComponent } from './components/contact/contact-list-pagination/contact-list-pagination.component';
import { ContactListFavouritesComponent } from './components/contact/contact-list-favourites/contact-list-favourites.component';
import { ForgotpasswordComponent } from './components/auth/forgotpassword/forgotpassword.component';
import { ForgotpasswordsuccessComponent } from './components/auth/forgotpasswordsuccess/forgotpasswordsuccess.component';
import { ChangepasswordComponent } from './components/auth/changepassword/changepassword.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { EdituserComponent } from './components/auth/edituser/edituser.component';
import { AddContactRfComponent } from './components/contact/add-contact-rf/add-contact-rf.component';
import { EditContactRfComponent } from './components/contact/edit-contact-rf/edit-contact-rf.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PrivacyComponent,
    ContactListComponent,
    SignupComponent,
    SigninComponent,
    AddContactComponent,
    EditContactComponent,
    ContactDetailsComponent,
    SignupsuccessComponent,
    ContactListPaginationComponent,
    ContactListFavouritesComponent,
    ForgotpasswordComponent,
    ForgotpasswordsuccessComponent,
    ChangepasswordComponent,
    EdituserComponent,
    AddContactRfComponent,
    EditContactRfComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule, //added to connect with api
    FormsModule, NgbModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
