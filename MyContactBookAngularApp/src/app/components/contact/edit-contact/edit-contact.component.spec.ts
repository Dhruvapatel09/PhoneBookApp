import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditContactComponent } from './edit-contact.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';
import { Contact } from 'src/app/models/contact.model';
import { ContactService } from 'src/app/services/contact.service';
import { ActivatedRoute, Router } from '@angular/router';

describe('EditContactComponent', () => {
  let component: EditContactComponent;
  let fixture: ComponentFixture<EditContactComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let route: ActivatedRoute;
  const mockCategory: Contact = {
    phoneId: 1,
    countryId: 2,
    stateId: 2,
    firstName: "Test",
    lastName: "Test",
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
    birthdate: "09-2003-08"
  };
  beforeEach(() => {
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['getContactById']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, FormsModule, RouterTestingModule.withRoutes([])],
      declarations: [EditContactComponent],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ phoneId: 1 })
          }
        }
      ]
    });
    fixture = TestBed.createComponent(EditContactComponent);
    component = fixture.componentInstance;
    route = TestBed.inject(ActivatedRoute);
    // fixture.detectChanges();
  });
 
  it('should create', () => {
    expect(component).toBeTruthy();
  });
 
  it('should initialize phoneId from route params and load contact details', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockCategory, message: '' };
    contactServiceSpy.getContactById.and.returnValue(of(mockResponse));
 
    // Act
    fixture.detectChanges(); // ngOnInit is called here
 
    // Assert
    expect(component.phoneId).toBe(1);
    expect(contactServiceSpy.getContactById).toHaveBeenCalledWith(1);
    expect(component.contact).toEqual(mockCategory);
  });
 
  it('should log error message if category loading fails', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: false, data: mockCategory, message: 'Failed to fetch contact' };
    contactServiceSpy.getContactById.and.returnValue(of(mockResponse));
    spyOn(console, 'error');
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contact', 'Failed to fetch contact');
  });
 
  it('should alert error message on HTTP error', () => {
    // Arrange
    spyOn(window, 'alert');
    const mockError = { error: { message: 'HTTP error' } };
    contactServiceSpy.getContactById.and.returnValue(throwError(mockError));
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(window.alert).toHaveBeenCalledWith('HTTP error');
  });
 
  it('should log "Completed" when contact loading completes', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockCategory, message: '' };
    contactServiceSpy.getContactById.and.returnValue(of(mockResponse));
    spyOn(console, 'log');
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(console.log).toHaveBeenCalledWith('completed');
  });
});