import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { ApiResponse } from '../models/ApiResponse{T}';
import { State } from '../models/state.model';

@Injectable({
  providedIn: 'root'
})
export class StateService {
  private apiUrl = 'http://localhost:5190/api/State';
  constructor(private http: HttpClient) { }
  getAllState(): Observable<ApiResponse<State[]>> {
    return this.http.get<ApiResponse<State[]>>(this.apiUrl+'/GetStates')
  }
  fetchStatetByCountryId(countryId: number): Observable<ApiResponse<State[]>> {
    return this.http.get<ApiResponse<State[]>>(this.apiUrl + '/GetAllStateByCountryId/' + countryId);
  }
}
