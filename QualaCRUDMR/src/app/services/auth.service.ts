import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ResponseHttp, Token } from '../interfaces/response-access';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly apiUrl: string = environment.apiUrl + "/login";
  private readonly tokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);
  
  constructor(private readonly http: HttpClient) { 
    const token = sessionStorage.getItem('access_token');
    this.tokenSubject.next(token);
  }

  public login(request: any){
    return this.http.post<ResponseHttp<Token>>(`${this.apiUrl}`, request);
  }

  getToken(): Observable<string | null> {
    return this.tokenSubject.asObservable();
  }

  setToken(token: string): void {
    sessionStorage.setItem('access_token', token);
    this.tokenSubject.next(token);
  }
}
