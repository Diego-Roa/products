import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly tokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);
  
  constructor(private readonly http: HttpClient) { 
    const token = sessionStorage.getItem('access_token');
    this.tokenSubject.next(token);
  }

  getToken(): Observable<string | null> {
    return this.tokenSubject.asObservable();
  }

  setToken(token: string): void {
    sessionStorage.setItem('access_token', token);
    this.tokenSubject.next(token);
  }
}
