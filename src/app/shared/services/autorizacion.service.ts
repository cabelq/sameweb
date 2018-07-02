
import {map} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs";
import { Login } from "../modelos/login";
import { Session } from "../modelos/session";
import 'rxjs/Rx';
import { environment } from '../../../environments/environment';

@Injectable()
export class AutorizacionService {
  //private basePath = 'http://localhost:51010/api/';
  constructor(private http: Http) { }

  login(loginObj: Login): Observable<Session> {
    
    return this.http.post(environment.urlApiServer + 'login', loginObj).pipe(map(this.extractData));
  }

  private extractData(res: Response) {
    //debugger;
    let body = res.json();
    return body;
  }
}
