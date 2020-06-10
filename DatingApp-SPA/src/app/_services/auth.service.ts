import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = 'http://10.0.0.23:5000/api/auth/';

  constructor(private http: HttpClient) { }

  login(model: any)
  {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user){
          localStorage.setItem('token', user.token);
        }
      })
    )
  } // End of login method

  register (model: any)
  {
    // Model will store the username and password
    return this.http.post(this.baseUrl + 'register', model);

  }
} // End of AuthService class
