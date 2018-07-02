
import {map} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers,ResponseContentType } from "@angular/http";
import { Observable } from "rxjs";
import { Estadistica } from '../modelos/estadistica';
import { Session } from '../modelos/session';
import { StorageService } from '../services/storage.service';
import { ExpedienteBody } from '../modelos/expedientebody';
import { Pase } from '../modelos/pase';

import 'rxjs/Rx';
import { environment } from '../../../environments/environment';

@Injectable()
export class DatabaseService {
  private sessionActual: Session = null;

  constructor(private http: Http, private storageService: StorageService) { }

  getEstadisticas(idDep:number): Observable<Estadistica> {  
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    return this.http.get(environment.urlApiServer + 'expedientes/estadistica/' + idDep,opts ).pipe(map(res => res.json()));
  }

  getOrganigrama(): Observable<any> {  
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    return this.http.get(environment.urlApiServer + 'organigrama',opts ).pipe(map(res => res.json()));
  }

  getTiposUbicaciones(): Observable<any> {  
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    return this.http.get(environment.urlApiServer + 'tiposubicaciones',opts ).pipe(map(res => res.json()));
  }

  getExpedientesEnOficinaMasAntiguos(idDep:number): Observable<any> {    
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    return this.http.get(environment.urlApiServer + 'expedientes/antiguos/' + idDep,opts ).pipe(map(res => res.json()));
  }

  getExpedientes(exp:ExpedienteBody): Observable<any> {    
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    return this.http.post(environment.urlApiServer + 'expedientes/filtro', exp, {headers: headers}  ).pipe(map(res => res.json()));
  }
  getExpediente(exp:ExpedienteBody): Observable<any> {    
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    return this.http.post(environment.urlApiServer + 'expedientes/expediente', exp, {headers: headers}  ).pipe(map(res => res.json()));
  }

  imprimirExpediente(exp:ExpedienteBody): Observable<any> {    
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    return this.http.post(environment.urlApiServer + 'expedientes/imprimirexpediente', exp, {responseType: ResponseContentType.Blob,headers: headers}  ).pipe(map(res => res.json()));
  }

  getPases(exp:ExpedienteBody): Observable<any> {    
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    return this.http.post(environment.urlApiServer + 'expedientes/pases', exp, {headers: headers}  ).pipe(map(res => res.json()));
  }
  getRespuestas(exp:ExpedienteBody): Observable<any> {    
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    return this.http.post(environment.urlApiServer + 'expedientes/respuestas', exp, {headers: headers}  ).pipe(map(res => res.json()));
  }
  getExpedientesSinRecibir(idDep:number): Observable<any> {    
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    return this.http.get(environment.urlApiServer + 'expedientes/sinrecibir/' + idDep,opts ).pipe(map(res => res.json()));
  }

  getExpedientesSinRecepcion(idDep:number): Observable<any> {    
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    return this.http.get(environment.urlApiServer + 'expedientes/sinrecepcion/' + idDep,opts ).pipe(map(res => res.json()));
  }

  insertPase(pase:Pase): Observable<any> {    
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;    
    return this.http.post(environment.urlApiServer + 'expedientes/insertPase', pase, {headers: headers}  ).pipe(map(res => res.json()));
  }

  imprimirRemito(remito:number): Observable<any> {    
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;
    opts.responseType = ResponseContentType.Blob;    
    return this.http.get(environment.urlApiServer + 'expedientes/imprimirRemito', opts);
  }

  recepcionPase(pase:number): Observable<any> {    
    this.sessionActual = this.storageService.obtenerSessionActual();  
    let headers = new Headers();
    headers.append('Token', this.sessionActual.token);
    let opts = new RequestOptions();
    opts.headers = headers;    
    return this.http.get(environment.urlApiServer + 'expedientes/recepcionPase/' + pase, {headers: headers}  ).pipe(map(res => res.json()));
  }


}
