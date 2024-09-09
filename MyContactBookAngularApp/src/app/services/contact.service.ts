import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Contact } from '../models/contact.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { addContact } from '../models/addContact';
@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private apiUrl = 'http://localhost:5190/api/Contact';
  constructor(private http: HttpClient) { }
  getAllContacts(): Observable<ApiResponse<Contact[]>> {
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + '/GetAllContacts')
  }
  addContact(contact: addContact): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl + '/Create', contact);
  }
  editContact(contact: Contact): Observable<ApiResponse<Contact>> {
    return this.http.put<ApiResponse<Contact>>(this.apiUrl + '/ModifyContact', contact);
  }
  getContactById(phoneId: number | undefined): Observable<ApiResponse<Contact>> {
    return this.http.get<ApiResponse<Contact>>(this.apiUrl + '/GetContactById/' + phoneId);
  }
  deleteContactById(phoneId: number | undefined): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(this.apiUrl + '/Remove/' + phoneId);
  }
 
  getAllContactsCount(letter:string,search:string) : Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+'/GetContactsCount?letter='+letter+'&searchQuery='+search);
  }
 
  getAllContactsWithPagination(pageNumber: number,pageSize:number,letter:string,sort:string,search:string) : Observable<ApiResponse<Contact[]>>{
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl+'/GetAllContactsByPagination?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize+'&searchQuery='+search+'&sortOrder='+sort);
  }

  getAllFavContactsWithLetter(pageNumber: number, pageSize: number,letter: string,sortOrder:string): Observable<ApiResponse<Contact[]>> {
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + '/favourites?letter=' + letter  + '&page=' + pageNumber + '&pageSize=' + pageSize+'&sortOrder='+sortOrder );

  }
  getAllFavContactsWithoutLetter(pageNumber: number, pageSize: number,sortOrder:string): Observable<ApiResponse<Contact[]>> {
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl + '/favourites' + '?page=' + pageNumber + '&pageSize=' + pageSize+'&sortOrder='+sortOrder );

  }
  getAllFavContactsCountWithLetter(letter: string,sortOrder:string): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalCountOfFavContacts?letter=' + letter+'&sortOrder='+sortOrder);

  }
  getAllFavContactsCount(): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalCountOfFavContacts');
  }

  
}