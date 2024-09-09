import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { User } from '../models/user';
import { ApiResponse } from '../models/ApiResponse{T}';
import { PasswordRecovery } from '../models/passwordrecovery';
import { EditUser } from '../models/edituser.model';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock:HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[AuthService]
    });
    service = TestBed.inject(AuthService);
    httpMock=TestBed.inject(HttpTestingController);
  });
  afterEach(()=>{
    httpMock.verify();
    
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  // Register User
  it('should register user successfully',()=>{
    //arrange
    const registerUser:User={
      "userId": 1,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      "password": "Di5;reP9]]A,0@c\\%V*g?Do>A/<5I?yBkWM2`dCWQ.s'!%U.+syh,0 P8sb-XmUqD",
      "confirmPassword": "string",
      fileName: null,
      imageByte: ''
    };
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:"User register successfully.",
      data:""
    }
    //act
    service.signUp(registerUser).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5190/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed user register',()=>{
    //arrange
    const registerUser:User={
      "userId": 0,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      "password": "Di5;reP9]]A,0@c\\%V*g?Do>A/<5I?yBkWM2`dCWQ.s'!%U.+syh,0 P8sb-XmUqD",
      "confirmPassword": "string",
      fileName: null,
      imageByte: ''
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"User already exists.",
      data:""
    }
    //act
    service.signUp(registerUser).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5190/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while register user',()=>{
    //arrange
    const registerUser:User={
      "userId": 0,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      "password": "Di5;reP9]]A,0@c\\%V*g?Do>A/<5I?yBkWM2`dCWQ.s'!%U.+syh,0 P8sb-XmUqD",
      "confirmPassword": "string",
      fileName: null,
      imageByte: ''
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.signUp(registerUser).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5190/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush({},mockHttpError);

  });
  //Forget Password
  it('should update password successfully', () => {
    //arrange
    const username = '';
    const password = '';
    const confirmPassword = '';

    const mockSuccessResponse: ApiResponse<string> = {
      success: true,
      message: "Password successfully change.",
      data: ""
    }
    //act
    service.forgetpassword(username,password,confirmPassword).subscribe(response => {
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req = httpMock.expectOne('http://localhost:5190/api/Auth/ForgetPassword');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed user forget password', () => {
    //arrange
    const username = '';
    const password = '';
    const confirmPassword = '';

    const mockErrorResponse: ApiResponse<string> = {
      success: true,
      message: "User already exists.",
      data: ""
    }
    //act
    service.forgetpassword(username,password,confirmPassword).subscribe(response => {
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req = httpMock.expectOne('http://localhost:5190/api/Auth/ForgetPassword');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while forget password', () => {
    //arrange
   const username = '';
    const password = '';
    const confirmPassword = '';
    const mockHttpError = {
      statusText: "Internal Server Error",
      status: 500
    }
    //act
    service.forgetpassword(username,password,confirmPassword).subscribe({
      next: () => fail('should have failed with the 500 error'),
      error: (error) => {
        //assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req = httpMock.expectOne('http://localhost:5190/api/Auth/ForgetPassword');
    expect(req.request.method).toBe('POST');
    req.flush({}, mockHttpError);

  });
//Password Discovery
it('should change password successfully', () => {
  //arrange
  const updatedPassword: PasswordRecovery = {
    username:"username",
    password:"",
    confirmPassword:"",
  };
  const mockSuccessResponse: ApiResponse<string> = {
    success: true,
    message: "Password successfully change.",
    data: ""
  }
  //act
  service.passwordDiscovery(updatedPassword).subscribe(response => {
    //assert
    expect(response).toBe(mockSuccessResponse);
  });
  const req = httpMock.expectOne('http://localhost:5190/api/Auth/ForgetPassword');
  expect(req.request.method).toBe('POST');
  req.flush(mockSuccessResponse);

});
it('should handle failed user change password', () => {
  //arrange
  const updatedPassword: PasswordRecovery = {
    username:"username",
    password:"",
    confirmPassword:"",
  };
  const mockErrorResponse: ApiResponse<string> = {
    success: true,
    message: "User already exists.",
    data: ""
  }
  //act
  service.passwordDiscovery(updatedPassword).subscribe(response => {
    //assert
    expect(response).toBe(mockErrorResponse);
  });
  const req = httpMock.expectOne('http://localhost:5190/api/Auth/ForgetPassword');
  expect(req.request.method).toBe('POST');
  req.flush(mockErrorResponse);

});
it('should handle Http error while change password', () => {
  //arrange
  const updatedPassword: PasswordRecovery = {
    username:"username",
    password:"",
    confirmPassword:"",
  };
  const mockHttpError = {
    statusText: "Internal Server Error",
    status: 500
  }
  //act
  service.passwordDiscovery(updatedPassword).subscribe({
    next: () => fail('should have failed with the 500 error'),
    error: (error) => {
      //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
    }
  });
  const req = httpMock.expectOne('http://localhost:5190/api/Auth/ForgetPassword');
  expect(req.request.method).toBe('POST');
  req.flush({}, mockHttpError);

});
//FetchUsrByLoginId
it('should fetch user by id successfully', () =>{
  //Arrange
  const loginId="loginId";
  const mockSuccessResponse:ApiResponse<EditUser>={
    success:true,
    data:{} as EditUser,
    message:''
  };
  //Act
  service.fetchUserByloginId(loginId).subscribe(response=>{
    //Assert
    expect(response.success).toBeTrue();
    expect(response.message).toBe('');
    expect(response.data).toEqual(mockSuccessResponse.data);
  });

  const req=httpMock.expectOne('http://localhost:5190/api/Auth/GetUserById/'+loginId);
  expect(req.request.method).toBe('GET');
  req.flush(mockSuccessResponse);

});

it('should handle failed contact retrival', () =>{
  //Arrange
  const loginId="loginId";
  const mockErrorResponse:ApiResponse<EditUser>={
    success:false,
    data:{} as EditUser,
    message:'No record found!'
  };
  //Act
  service.fetchUserByloginId(loginId).subscribe(response=>{
    //Assert
    expect(response).toEqual(mockErrorResponse);
    expect(response.message).toBe('No record found!');
    expect(response.success).toBeFalse();
  });

  const req=httpMock.expectOne('http://localhost:5190/api/Auth/GetUserById/'+loginId);
  expect(req.request.method).toBe('GET');
  req.flush(mockErrorResponse);

});

it('should handle HTTP error', () =>{
  //Arrange
  const loginId="loginId";
  const mockHttpError={
    status:500,
    statusText:'Internal Server Error'
  };
  //Act
  service.fetchUserByloginId(loginId).subscribe({
    next: () => fail('should have failed with 500 error'),
    error: (error) => {
      //Assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error')
    }
  });

  const req=httpMock.expectOne('http://localhost:5190/api/Auth/GetUserById/'+loginId);
  expect(req.request.method).toBe('GET');
  req.flush({}, mockHttpError);   

});
//edit user
it('should update a category successfully', () => {
  //Arrange
  const updatedContact: EditUser = {
    userId:1,
    firstName: "Test",
    lastName: "Test",
    email: "Test@gmail.com",
    contactNumber: "1234567891",
    loginId:"LoginId",
    password:"",
    confirmPassword:"",
    file:"",
    fileName:null
  };
  const mockSuccessResponse: ApiResponse<EditUser> = {
    success: true,
    data: {} as EditUser,
    message: 'Contact updated successfully.'
  };

  //Act
  service.editUser(updatedContact).subscribe(
    response => {
      //Assert
      expect(response).toEqual(mockSuccessResponse);
    });
  const req = httpMock.expectOne('http://localhost:5190/api/Auth/Edit');
  expect(req.request.method).toBe('PUT');
  req.flush(mockSuccessResponse);

});
it('should handle failed category update', () => {
  //Arrange
  const updatedContact: EditUser = {
    userId:1,
    firstName: "Test",
    lastName: "Test",
    email: "Test@gmail.com",
    contactNumber: "1234567891",
    loginId:"LoginId",
    password:"",
    confirmPassword:"",
    file:"",
    fileName:null
  };
  const mockErrorResponse: ApiResponse<EditUser> = {
    success: true,
    message: "User already exists.",
    data: {} as EditUser
  };
  //Act
  service.editUser(updatedContact).subscribe(
    response => {
      //Assert
      expect(response).toEqual(mockErrorResponse);
    });
  const req = httpMock.expectOne('http://localhost:5190/api/Auth/Edit');
  expect(req.request.method).toBe('PUT');
  req.flush(mockErrorResponse);

});

it('should handle error response for update', () => {
  //Arrange
  const updatedContact: EditUser = {
    userId:1,
    firstName: "Test",
    lastName: "Test",
    email: "Test@gmail.com",
    contactNumber: "1234567891",
    loginId:"LoginId",
    password:"",
    confirmPassword:"",
    file:"",
    fileName:null
  };
  const mockHttpError = {
    statusText: "Internal Server Error",
    status: 500
  };
  //Act
  service.editUser(updatedContact).subscribe({
    next: () => fail('should have failed with 500 error'),
    error: (error) => {
      //Assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error')
    }

  });
  const req = httpMock.expectOne('http://localhost:5190/api/Auth/Edit');
  expect(req.request.method).toBe('PUT');
  req.flush({}, mockHttpError);

});
});
