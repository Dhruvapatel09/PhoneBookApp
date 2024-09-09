import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddContactComponent } from './add-contact.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ContactService } from 'src/app/services/contact.service';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';
import { ContactListPaginationComponent } from '../contact-list-pagination/contact-list-pagination.component';

describe('AddContactComponent', () => {
  let component: AddContactComponent;
  let fixture: ComponentFixture<AddContactComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let routerSpy: Router
  beforeEach(() => {
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['addContact']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, FormsModule, RouterTestingModule.withRoutes([{path:'contacts-pagination', component:ContactListPaginationComponent}])],
      declarations: [AddContactComponent],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
      ]
    });
    fixture = TestBed.createComponent(AddContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    routerSpy = TestBed.inject(Router)
  });
 
  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should navigate to /categories on successful category addition', () => {
    spyOn(routerSpy,'navigate');
    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    contactServiceSpy.addContact.and.returnValue(of(mockResponse));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
       firstName: 'Test name',
        lastName: 'last name',
        countryId: 2,
          stateId: 2,
          email: "Test@gmail.com",
          phoneNumber: "1234567891",
          image: '',
          imageByte: "",
          company: "company 1",
          gender: "F",
          favourites: true,
          country: {
            countryId: 1,
            countryName: "country 1"
          },
          state: {
            countryId: 1,
            stateId: 2,
            stateName: "state 1"
          },
          birthdate: "09-08-2008"
      },
      controls: {
        
        phoneId: {value:1}, firstName: {value:'Test name'},
        lastName: {value:'last name'},
        countryId: {value:2},
          stateId:{value: 2},
          email: {value:"Test@gmail.com"},
          phoneNumber: {value:"1234567891"},
          image: {value:''},
          imageByte: {value:""},
          company:{value: "company 1"},
          gender:{value: "F"},
          favourites: {value:true},
          birthdate:{value: "09-08-2008"}
      }
    };
 
    component.onSubmit(form);
 
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/contacts-pagination']);
    expect(component.loading).toBe(false);
  });
 
  it('should alert error message on unsuccessful category addition', () => {
    spyOn(window, 'alert');
    const mockResponse: ApiResponse<string> = { success: false, data: '', message: 'Error adding category' };
    contactServiceSpy.addContact.and.returnValue(of(mockResponse));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
        firstName: 'Test name',
         lastName: 'last name',
         countryId: 2,
           stateId: 2,
           email: "Test@gmail.com",
           phoneNumber: "1234567891",
           image: '',
           imageByte: "",
           company: "company 1",
           gender: "F",
           favourites: true,
           country: {
             countryId: 1,
             countryName: "country 1"
           },
           state: {
             countryId: 1,
             stateId: 2,
             stateName: "state 1"
           },
           birthdate: "09-08-2008"
       },
       controls: {
         
         phoneId: {value:1}, firstName: {value:'Test name'},
         lastName: {value:'last name'},
         countryId: {value:2},
           stateId:{value: 2},
           email: {value:"Test@gmail.com"},
           phoneNumber: {value:"1234567891"},
           image: {value:''},
           imageByte: {value:""},
           company:{value: "company 1"},
           gender:{value: "F"},
           favourites: {value:true},
           birthdate:{value: "09-08-2008"}
       }
    };
 
    component.onSubmit(form);
 
    expect(window.alert).toHaveBeenCalledWith('Error adding category');
    expect(component.loading).toBe(false);
  });
 
  it('should alert error message on HTTP error', () => {
    spyOn(window, 'alert');
    const mockError = { error: { message: 'HTTP error' } };
    contactServiceSpy.addContact.and.returnValue(throwError(mockError));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
        firstName: 'Test name',
         lastName: 'last name',
         countryId: 2,
           stateId: 2,
           email: "Test@gmail.com",
           phoneNumber: "1234567891",
           image: '',
           imageByte: "",
           company: "company 1",
           gender: "F",
           favourites: true,
           country: {
             countryId: 1,
             countryName: "country 1"
           },
           state: {
             countryId: 1,
             stateId: 2,
             stateName: "state 1"
           },
           birthdate: "09-08-2008"
       },
       controls: {
         
         phoneId: {value:1}, firstName: {value:'Test name'},
         lastName: {value:'last name'},
         countryId: {value:2},
           stateId:{value: 2},
           email: {value:"Test@gmail.com"},
           phoneNumber: {value:"1234567891"},
           image: {value:''},
           imageByte: {value:""},
           company:{value: "company 1"},
           gender:{value: "F"},
           favourites: {value:true},
           birthdate:{value: "09-08-2008"}
       }
    };
 
    component.onSubmit(form);
 
    expect(window.alert).toHaveBeenCalledWith('HTTP error');
    expect(component.loading).toBe(false);
  });
 
  it('should not call categoryService.AddCategory on invalid form submission', () => {
    const form = <NgForm>{ valid: false };
 
    component.onSubmit(form);
 
    expect(contactServiceSpy.addContact).not.toHaveBeenCalled();
    expect(component.loading).toBe(false);
  });
});