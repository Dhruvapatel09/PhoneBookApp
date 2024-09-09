import { TestBed } from '@angular/core/testing';

import { StateService } from './state.service';
import { State } from '../models/state.model';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';

describe('StateService', () => {
  let service: StateService;
  let httpMock: HttpTestingController;
  const mockApiResponse: ApiResponse<State[]> = {
    success: true,
    data: [
      {
        stateId:1,
        countryId: 1,
        stateName: "State 1",
      },
      {
        stateId:2,
        countryId: 2,
        stateName: "State 2",
      }
    ],
    message: ''
  }
  const mockSuccessResponse: ApiResponse<string> = {
    success: true,
    message: "Contact saved successfully.",
    data: ""
  };
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(StateService);
    httpMock = TestBed.inject(HttpTestingController);
  });
  afterEach(() => {
    httpMock.verify();
  });
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all state successfully', () => {
    //Arrange
    const apiUrl = 'http://localhost:5190/api/State/GetStates';

    //Act
    service.getAllState().subscribe((response) => {
      //Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle an empty state list', () => {
    //Arrange
    const apiUrl = 'http://localhost:5190/api/State/GetStates';

    const emptyResponse: ApiResponse<State[]> = {
      success: true,
      data: [],
      message: ''
    }
    //Act
    service.getAllState().subscribe((response) => {
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
    const apiUrl = 'http://localhost:5190/api/State/GetStates';
    const errorMessage = 'Failed to load state';
    //Act
    service.getAllState().subscribe(
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
  it('should fetch state by country id successfully', () =>{
    //Arrange
    const countryId=1;
    const mockSuccessResponse:ApiResponse<State[]>={
      success:true,
      data:{} as State[],
      message:''
    };
    //Act
    service.fetchStatetByCountryId(countryId).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toEqual(mockSuccessResponse.data);
    });

    const req=httpMock.expectOne('http://localhost:5190/api/State/GetAllStateByCountryId/'+countryId);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);

  });

  it('should handle failed country retrival', () =>{
    //Arrange
    const countryId=1;
    const mockErrorResponse:ApiResponse<State[]>={
      success:false,
      data:{} as State[],
      message:'No record found!'
    };
    //Act
    service.fetchStatetByCountryId(countryId).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toBe('No record found!');
      expect(response.success).toBeFalse();
    });

    const req=httpMock.expectOne('http://localhost:5190/api/State/GetAllStateByCountryId/'+countryId);
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
    service.fetchStatetByCountryId(countryId).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error')
      }
    });

    const req=httpMock.expectOne('http://localhost:5190/api/State/GetAllStateByCountryId/'+countryId);
    expect(req.request.method).toBe('GET');
    req.flush({}, mockHttpError);   

  });
});
