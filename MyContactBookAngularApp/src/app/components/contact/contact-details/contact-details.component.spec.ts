import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactDetailsComponent } from './contact-details.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { ContactService } from 'src/app/services/contact.service';

describe('ContactDetailsComponent', () => {
  let component: ContactDetailsComponent;
  let fixture: ComponentFixture<ContactDetailsComponent>;
  let categoryService: jasmine.SpyObj<ContactService>;
  let route: ActivatedRoute;
  const mockContact: Contact = {
     phoneId: 1, firstName: 'Test name',
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
  };
  beforeEach(() => {
    const categoryServiceSpy = jasmine.createSpyObj('ContactService', ['getContactById']);
 
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule.withRoutes([])],
      declarations: [ContactDetailsComponent],
      providers: [
        { provide: ContactService, useValue: categoryServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ phoneId: 1 })
          }
        }
      ]
    });
    fixture = TestBed.createComponent(ContactDetailsComponent);
    component = fixture.componentInstance;
    categoryService = TestBed.inject(ContactService) as jasmine.SpyObj<ContactService>;
    route = TestBed.inject(ActivatedRoute);
  });
 
  it('should create', () => {
    expect(component).toBeTruthy();
  });
 
  it('should initialize categoryId from route params and load category details', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockContact, message: '' };
    categoryService.getContactById.and.returnValue(of(mockResponse));
 
    // Act
    fixture.detectChanges(); // ngOnInit is called here
 
    // Assert
    expect(component.phoneId).toBe(1);
    expect(categoryService.getContactById).toHaveBeenCalledWith(1);
    expect(component.contact).toEqual(mockContact);
  });
 
  it('should log "Completed" when category loading completes', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockContact, message: '' };
    categoryService.getContactById.and.returnValue(of(mockResponse));
    spyOn(console, 'log');
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(console.log).toHaveBeenCalledWith('Completed');
  });
});