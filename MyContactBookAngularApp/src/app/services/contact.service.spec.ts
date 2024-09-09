import { TestBed } from '@angular/core/testing';

import { ContactService } from './contact.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Contact } from '../models/contact.model';
import { addContact } from '../models/addContact';
describe('ContactService', () => {
  let service: ContactService;
  let httpMock: HttpTestingController;
  const mockApiResponse: ApiResponse<Contact[]> = {
    success: true,
    data: [
      {
        phoneId: 1,
        countryId: 2,
        stateId: 2,
        firstName: "Test",
        lastName: "Test",
        email: "Test@gmail.com",
        phoneNumber: "1234567891",
        image: "Test",
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
        birthdate: "09-09-2009"
      },
      {
        phoneId: 2,
        countryId: 2,
        stateId: 2,
        firstName: "Test 2",
        lastName: "Test 2",
        email: "test2@gmail.com",
        phoneNumber: "1234567881",
        image: "Test",
        imageByte: "",
        gender: "F",
        favourites: true,
        company: 'Test',
        country: {
          countryId: 1,
          countryName: "country 1"
        },
        state: {
          countryId: 1,
          stateId: 2,
          stateName: "state 1"
        },
        birthdate: "09-09-2009"
      }
    ],
    message: ''
  }

  const mockSuccessResponse: ApiResponse<string> = {
    success: true,
    message: "Contact saved successfully.",
    data: ""
  };
  const mockErrorResponse: ApiResponse<string> = {
    success: true,
    message: "Contact already exists.",
    data: ""
  };
  const mockHttpError = {
    success: false,
    statusText: "Internal Server Error",
    status: 500
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(ContactService);
    httpMock = TestBed.inject(HttpTestingController);
  });


  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all contacts successfully', () => {
    //Arrange
    const apiUrl = 'http://localhost:5190/api/Contact/GetAllContacts';

    //Act
    service.getAllContacts().subscribe((response) => {
      //Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle an empty contact list', () => {
    //Arrange
    const apiUrl = 'http://localhost:5190/api/Contact/GetAllContacts';

    const emptyResponse: ApiResponse<Contact[]> = {
      success: true,
      data: [],
      message: ''
    }
    //Act
    service.getAllContacts().subscribe((response) => {
      //Assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });

  it('should handle HTTP error gracefully', () => {
    //Arrange
    const apiUrl = 'http://localhost:5190/api/Contact/GetAllContacts';
    const errorMessage = 'Failed to load contacts';
    //Act
    service.getAllContacts().subscribe(
      () => fail('expected an error, not contacts'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    //Respond with error
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });

  it('should add a contact successfully', () => {
    //Arrange
    const addContact: addContact = {
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
    //Act
    service.addContact(addContact).subscribe(response => {
      //Assert
      expect(response).toBe(mockSuccessResponse);
    });

    const req = httpMock.expectOne('http://localhost:5190/api/Contact/Create');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);
  });

  it('should handle failed contact addition', () => {
    //Arrange
    const addCategory: addContact = {
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
    const mockErrorResponse: ApiResponse<string> = {
      success: true,
      message: "Category already exists.",
      data: ""
    };
    //Act
    service.addContact(addCategory).subscribe(response => {
      //Assert
      expect(response).toBe(mockErrorResponse);
    });

    const req = httpMock.expectOne('http://localhost:5190/api/Contact/Create');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);
  });

  it('should handle error response', () => {
    //Arrange
    const addCategory: addContact = {
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
    const mockHttpError = {
      statusText: "Internal Server Error",
      status: 500
    }
    //Act
    service.addContact(addCategory).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error')
      }

    });

    const req = httpMock.expectOne('http://localhost:5190/api/Contact/Create');
    expect(req.request.method).toBe('POST');
    req.flush({}, mockHttpError);
  });

  it('should update a category successfully', () => {
    //Arrange
    const updatedContact: Contact = {
      phoneId:1,
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
    const mockSuccessResponse: ApiResponse<Contact> = {
      success: true,
      data: {} as Contact,
      message: 'Contact updated successfully.'
    };

    //Act
    service.editContact(updatedContact).subscribe(
      response => {
        //Assert
        expect(response).toEqual(mockSuccessResponse);
      });
    const req = httpMock.expectOne('http://localhost:5190/api/Contact/ModifyContact');
    expect(req.request.method).toBe('PUT');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed category update', () => {
    //Arrange
    const updatedCategory: Contact = {
      phoneId:1,
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
    const mockErrorResponse: ApiResponse<Contact> = {
      success: true,
      message: "Contact already exists.",
      data: {} as Contact
    };
    //Act
    service.editContact(updatedCategory).subscribe(
      response => {
        //Assert
        expect(response).toEqual(mockErrorResponse);
      });
    const req = httpMock.expectOne('http://localhost:5190/api/Contact/ModifyContact');
    expect(req.request.method).toBe('PUT');
    req.flush(mockErrorResponse);

  });

  it('should handle error response for update', () => {
    //Arrange
    const updatedCategory: Contact = {
      phoneId:1,
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
    const mockHttpError = {
      statusText: "Internal Server Error",
      status: 500
    };
    //Act
    service.editContact(updatedCategory).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error')
      }

    });
    const req = httpMock.expectOne('http://localhost:5190/api/Contact/ModifyContact');
    expect(req.request.method).toBe('PUT');
    req.flush({}, mockHttpError);

  });
  it('should fetch category by id successfully', () =>{
    //Arrange
    const contactId=1;
    const mockSuccessResponse:ApiResponse<Contact>={
      success:true,
      data:{
        phoneId:1,
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
      },
      message:''
    };
    //Act
    service.getContactById(contactId).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toEqual(mockSuccessResponse.data);
      expect(response.data.phoneId).toEqual(contactId);
    });

    const req=httpMock.expectOne('http://localhost:5190/api/Contact/GetContactById/'+contactId);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);

  });

  it('should handle failed contact retrival', () =>{
    //Arrange
    const contactId=1;
    const mockErrorResponse:ApiResponse<Contact>={
      success:false,
      data:{} as Contact,
      message:'No record found!'
    };
    //Act
    service.getContactById(contactId).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toBe('No record found!');
      expect(response.success).toBeFalse();
    });

    const req=httpMock.expectOne('http://localhost:5190/api/Contact/GetContactById/'+contactId);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);

  });

  it('should handle HTTP error', () =>{
    //Arrange
    const contactId=1;
    const mockHttpError={
      status:500,
      statusText:'Internal Server Error'
    };
    //Act
    service.getContactById(contactId).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error')
      }
    });

    const req=httpMock.expectOne('http://localhost:5190/api/Contact/GetContactById/'+contactId);
    expect(req.request.method).toBe('GET');
    req.flush({}, mockHttpError);   

  });
  it('should delete contact successfully', () =>{
    //Arrange
    const categoryId=1;
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      data:'',
      message:'Contact deleted successfully.'
    };
    //Act
    service.deleteContactById(categoryId).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('Contact deleted successfully.');
      expect(response.data).toEqual(mockSuccessResponse.data);
    });

    const req=httpMock.expectOne('http://localhost:5190/api/Contact/Remove/'+categoryId);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockSuccessResponse);

  });

  it('should handle failed delete contact', () =>{
    //Arrange
    const categoryId=1;
    const mockSuccessResponse:ApiResponse<string>={
      success:false,
      data:'',
      message:'Something went wrong, please try after sometimes.'
    };
    //Act
    service.deleteContactById(categoryId).subscribe(response=>{
      //Assert
      expect(response.success).toBeFalse();
      expect(response.message).toBe('Something went wrong, please try after sometimes.');
      expect(response.data).toEqual(mockSuccessResponse.data);
    });

    const req=httpMock.expectOne('http://localhost:5190/api/Contact/Remove/'+categoryId);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockSuccessResponse);

  });
  it('should handle HTTP error', () =>{
    //Arrange
    const categoryId=1;
    const mockHttpError={
      status:500,
      statusText:'Internal Server Error'
    };
    //Act
    service.deleteContactById(categoryId).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error')
      }
    });
    const req=httpMock.expectOne('http://localhost:5190/api/Contact/Remove/'+categoryId);
    expect(req.request.method).toBe('DELETE');
    req.flush({},mockHttpError);

  });

  it('should fetch all contact count successfully', () => {
    //Arrange
    const letter="a";
    const searchQuery="search";
    const apiUrl = 'http://localhost:5190/api/Contact/GetContactsCount?letter='+letter+'&searchQuery='+searchQuery;
    const mockApiResponse = { data: 2 }; 
    //Act
    service.getAllContactsCount(letter,searchQuery).subscribe((response) => {
      //Assert
      expect(response.data).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);

    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle an empty contact list', () => {
    //Arrange
    const letter="a";
    const searchQuery="search";
    const apiUrl = 'http://localhost:5190/api/Contact/GetContactsCount?letter='+letter+'&searchQuery='+searchQuery;
    const mockApiResponse = { data: 0 }; 
    const emptyResponse: ApiResponse<number> = {
      success: true,
      data: 0,
      message: ''
    }
    //Act
    service.getAllContactsCount(letter,searchQuery).subscribe((response) => {
      //Assert
      expect(response.data).toBe(0);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });

  it('should handle HTTP error gracefully', () => {
    //Arrange
    const letter="a";
    const searchQuery="search";
    const apiUrl = 'http://localhost:5190/api/Contact/GetContactsCount?letter='+letter+'&searchQuery='+searchQuery;
    const errorMessage = 'Failed to load contacts';
    //Act
    service.getAllContactsCount(letter,searchQuery).subscribe(
      () => fail('expected an error, not contacts'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    //Respond with error
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });

  it('should fetch all contacts with pagination successfully', () => {
    //Arrange
    const letter="a";
    const searchQuery="search";
    const sortOrder="asc";
    const pageNumber=1;
    const pageSize=1;
    const apiUrl = 'http://localhost:5190/api/Contact/GetAllContactsByPagination?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize+'&searchQuery='+searchQuery+'&sortOrder=asc';

    //Act
    service.getAllContactsWithPagination(pageNumber,pageSize,letter,sortOrder,searchQuery).subscribe((response) => {
      //Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle an empty contact list with pagination', () => {
    //Arrange
    const letter="a";
    const searchQuery="search";
    const sortOrder="asc";
    const pageNumber=1;
    const pageSize=1;
    const apiUrl = 'http://localhost:5190/api/Contact/GetAllContactsByPagination?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize+'&searchQuery='+searchQuery+'&sortOrder='+sortOrder;

    const emptyResponse: ApiResponse<Contact[]> = {
      success: true,
      data: [],
      message: ''
    }
    //Act
    service.getAllContactsWithPagination(pageNumber,pageSize,letter,sortOrder,searchQuery).subscribe((response) => {
      //Assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });

  it('should handle HTTP error gracefully', () => {
    //Arrange
    const letter="a";
    const searchQuery="search";
    const sortOrder="asc";
    const pageNumber=1;
    const pageSize=1;
    const apiUrl = 'http://localhost:5190/api/Contact/GetAllContactsByPagination?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize+'&searchQuery='+searchQuery+'&sortOrder='+sortOrder;

    const errorMessage = 'Failed to load contacts';
    //Act
    service.getAllContactsWithPagination(pageNumber,pageSize,letter,sortOrder,searchQuery).subscribe(
      () => fail('expected an error, not contacts'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    //Respond with error
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });

  it('should fetch all fav contacts successfully', () => {
    //Arrange
    const letter="a";
    const sortOrder="asc";
    const pageNumber=1;
    const pageSize=1;
    const apiUrl = 'http://localhost:5190/api/Contact/favourites?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sortOrder;

    //Act
    service.getAllFavContactsWithLetter(pageNumber,pageSize,letter,sortOrder).subscribe((response) => {
      //Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle an empty contact list', () => {
    //Arrange
    const letter="a";
    const sortOrder="asc";
    const pageNumber=1;
    const pageSize=1;
    const apiUrl = 'http://localhost:5190/api/Contact/favourites?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sortOrder;

    const emptyResponse: ApiResponse<Contact[]> = {
      success: true,
      data: [],
      message: ''
    }
    //Act
    service.getAllFavContactsWithLetter(pageNumber,pageSize,letter,sortOrder).subscribe((response) => {
      //Assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });

  it('should handle HTTP error gracefully', () => {
    //Arrange
    const letter="a";
    const sortOrder="asc";
    const pageNumber=1;
    const pageSize=1;
    const apiUrl = 'http://localhost:5190/api/Contact/favourites?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sortOrder;

    const errorMessage = 'Failed to load contacts';
    //Act
    service.getAllFavContactsWithLetter(pageNumber,pageSize,letter,sortOrder).subscribe(
      () => fail('expected an error, not contacts'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    //Respond with error
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });

  it('should fetch all fav contacts without letter successfully', () => {
    //Arrange
    const sortOrder="asc";
    const pageNumber=1;
    const pageSize=1;
    const apiUrl = 'http://localhost:5190/api/Contact/favourites?page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sortOrder;

    //Act
    service.getAllFavContactsWithoutLetter(pageNumber,pageSize,sortOrder).subscribe((response) => {
      //Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle an empty contact list without letter', () => {
    //Arrange
    const sortOrder="asc";
    const pageNumber=1;
    const pageSize=1;
    const apiUrl = 'http://localhost:5190/api/Contact/favourites?page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sortOrder;

    const emptyResponse: ApiResponse<Contact[]> = {
      success: true,
      data: [],
      message: ''
    }
    //Act
    service.getAllFavContactsWithoutLetter(pageNumber,pageSize,sortOrder).subscribe((response) => {
      //Assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });

  it('should handle HTTP error gracefully', () => {
    //Arrange
    const sortOrder="asc";
    const pageNumber=1;
    const pageSize=1;
    const apiUrl = 'http://localhost:5190/api/Contact/favourites?page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sortOrder;

    const errorMessage = 'Failed to load contacts';
    //Act
    service.getAllFavContactsWithoutLetter(pageNumber,pageSize,sortOrder).subscribe(
      () => fail('expected an error, not contacts'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    //Respond with error
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });

  it('should fetch fav contact count with letter successfully', () => {
    //Arrange
    const letter="a";
    const sortOrder="asc";
    const apiUrl = 'http://localhost:5190/api/Contact/GetTotalCountOfFavContacts?letter='+letter+'&sortOrder='+sortOrder;
    const mockApiResponse = { data: 2 }; 
    //Act
    service.getAllFavContactsCountWithLetter(letter,sortOrder).subscribe((response) => {
      //Assert
      expect(response.data).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);

    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle an empty fav contact list', () => {
    //Arrange
    const letter="a";
    const sortOrder="asc";
    const apiUrl = 'http://localhost:5190/api/Contact/GetTotalCountOfFavContacts?letter='+letter+'&sortOrder='+sortOrder;
     const mockApiResponse = { data: 0 }; 
    const emptyResponse: ApiResponse<number> = {
      success: true,
      data: 0,
      message: ''
    }
    //Act
    service.getAllFavContactsCountWithLetter(letter,sortOrder).subscribe((response) => {
      //Assert
      expect(response.data).toBe(0);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });

  it('should handle HTTP error gracefully', () => {
    //Arrange
    const letter="a";
    const sortOrder="asc";
    const apiUrl = 'http://localhost:5190/api/Contact/GetTotalCountOfFavContacts?letter='+letter+'&sortOrder='+sortOrder;
     const errorMessage = 'Failed to load contacts';
    //Act
    service.getAllFavContactsCountWithLetter(letter,sortOrder).subscribe(
      () => fail('expected an error, not contacts'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    //Respond with error
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });
  it('should fetch fav contact count successfully', () => {
    //Arrange
    const apiUrl = 'http://localhost:5190/api/Contact/GetTotalCountOfFavContacts';
    const mockApiResponse = { data: 2 }; 
    //Act
    service.getAllFavContactsCount().subscribe((response) => {
      //Assert
      expect(response.data).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);

    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle an empty contact list', () => {
    //Arrange
    const apiUrl = 'http://localhost:5190/api/Contact/GetTotalCountOfFavContacts';
     const mockApiResponse = { data: 0 }; 
    const emptyResponse: ApiResponse<number> = {
      success: true,
      data: 0,
      message: ''
    }
    //Act
    service.getAllFavContactsCount().subscribe((response) => {
      //Assert
      expect(response.data).toBe(0);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });

  it('should handle HTTP error gracefully', () => {
    //Arrange
    const apiUrl = 'http://localhost:5190/api/Contact/GetTotalCountOfFavContacts';
     const errorMessage = 'Failed to load contacts';
    //Act
    service.getAllFavContactsCount().subscribe(
      () => fail('expected an error, not contacts'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    //Respond with error
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });
});