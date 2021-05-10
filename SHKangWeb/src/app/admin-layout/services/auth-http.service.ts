import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';

const API_USERS_URL = `${environment.apiUrl}/users`;

@Injectable({
    providedIn: 'root',
})
export class AuthHTTPService {
    constructor(private http: HttpClient) { }

    // public methods
    login(email: string, password: string): Observable<any> {
        return this.http.post<any>(API_USERS_URL, { email, password });
    }

    // CREATE =>  POST: add a new user to the server
    createUser(user: any): Observable<any> {
        return this.http.post<any>(API_USERS_URL, user);
    }

    // Your server should check email => If email exists send link to the user and return true | If email doesn't exist return false
    forgotPassword(email: string): Observable<boolean> {
        return this.http.post<boolean>(`${API_USERS_URL}/forgot-password`, {
            email,
        });
    }

    getUserByToken(token): Observable<any> {
        const httpHeaders = new HttpHeaders({
            Authorization: `Bearer ${token}`,
        });
        return this.http.get<any>(`${API_USERS_URL}`, {
            headers: httpHeaders,
        });
    }
}
