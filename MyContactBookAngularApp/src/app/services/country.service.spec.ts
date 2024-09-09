import { TestBed } from '@angular/core/testing';

import { CountryService } from './country.service';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Country } from '../models/country.model';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('CountryService', () => {
  let service: CountryService;
  let httpMock: HttpTestingController;
  const mockApiResponse: ApiResponse<Country[]> = {
    success: true,
    data: [
      {
        countryId: 1,
        countryName: "Country 1",
      },
      {
        countryId: 2,
        countryName: "Country 2",
      }
    ],
    message: ''
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(CountryService);
    httpMock = TestBed.inject(HttpTestingController);
  });
  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all country successfully', () => {
    //Arrange
    const apiUrl = 'http://localhost:5190/api/Country/GetAll';

    //Act
    service.getAllCountry().subscribe((response) => {
      //Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle an empty country list', () => {
    //Arrange
    const apiUrl = 'http://localhost:5190/api/Country/GetAll';

    const emptyResponse: ApiResponse<Country[]> = {
      success: true,
      data: [],
      message: ''
    }
    //Act
    service.getAllCountry().subscribe((response) => {
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
    const apiUrl = 'http://localhost:5190/api/Country/GetAll';
    const errorMessage = 'Failed to load contacts';
    //Act
    service.getAllCountry().subscribe(
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

  it('should fetch country by id successfully', () =>{
    //Arrange
    const countryId=1;
    const mockSuccessResponse:ApiResponse<Country>={
      success:true,
      data:{
        countryId: 1,
        countryName: "Country 1",
      },
      message:''
    };
    //Act
    service.getCountryById(countryId).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toEqual(mockSuccessResponse.data);
      expect(response.data.countryId).toEqual(countryId);
    });

    const req=httpMock.expectOne('http://localhost:5190/api/Country/GetCountryById/'+countryId);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);

  });

  it('should handle failed country retrival', () =>{
    //Arrange
    const countryId=1;
    const mockErrorResponse:ApiResponse<Country>={
      success:false,
      data:{} as Country,
      message:'No record found!'
    };
    //Act
    service.getCountryById(countryId).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toBe('No record found!');
      expect(response.success).toBeFalse();
    });

    const req=httpMock.expectOne('http://localhost:5190/api/Country/GetCountryById/'+countryId);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);

  });

  it('should handle HTTP error', () =>{
    //Arrange
    const countryId=1;
    const mockHttpError={
      status:500,
      statusText:'Internal Server Error'
    };
    //Act
    service.getCountryById(countryId).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error')
      }
    });

    const req=httpMock.expectOne('http://localhost:5190/api/Country/GetCountryById/'+countryId);
    expect(req.request.method).toBe('GET');
    req.flush({}, mockHttpError);   

  });
});
