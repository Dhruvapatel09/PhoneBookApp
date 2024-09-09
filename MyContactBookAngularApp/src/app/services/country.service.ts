import { Injectable } from '@angular/core';
import { Country } from '../models/country.model';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CountryService {

  private apiUrl = 'http://localhost:5190/api/Country';
  constructor(private http: HttpClient) { }
  getAllCountry(): Observable<ApiResponse<Country[]>> {
    return this.http.get<ApiResponse<Country[]>>(this.apiUrl+'/GetAll')
  }
  getCountryById(countryId: number|undefined): Observable<ApiResponse<Country>> {
    return this.http.get<ApiResponse<Country>>(this.apiUrl + '/GetCountryById/' + countryId);
  }
  
}
