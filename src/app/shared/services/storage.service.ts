import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Session } from "../modelos/session";
import { Usuario } from "../modelos/usuario";
import { Organigrama } from '../modelos/organigrama';


@Injectable()
export class StorageService {
  private localStorageService;
  private sessionActual: Session = null;

  constructor(private router: Router) {
    this.localStorageService = localStorage;
    this.sessionActual = this.cargarDatosSession();
  }

  setearSessionActual(session: Session): void {
    this.sessionActual = session;
    this.localStorageService.setItem('usuarioActual', JSON.stringify(session));
    this.localStorageService.setItem('isLoggedin', 'true');
  }

  setearOrganigramaDefault(organigrama: Organigrama): void {
    this.sessionActual.parametros.organigrama_default =  organigrama;
    this.localStorageService.setItem('usuarioActual', JSON.stringify(this.sessionActual));    
  }

  cargarDatosSession(): Session {
    var sessionStr = this.localStorageService.getItem('usuarioActual');
    return (sessionStr) ? <Session>JSON.parse(sessionStr) : null;
  }

  public obtenerSessionActual(): Session {
    return this.sessionActual;
  }

  private borrarSessionActual(): void {
    this.localStorageService.removeItem('usuarioActual');
    this.localStorageService.removeItem('isLoggedin');
    this.sessionActual = null;
  }

  /*public obtenerUsuarioActual(): Usuario {
    var session: Session = this.obtenerSessionActual();
    return (session && session.usuario) ? session.usuario : null;
  };*/


  estaAutenticado(): boolean {
    return (this.obtenerTokenActual() != null) ? true : false;
  };


  private obtenerTokenActual(): string {        
    var session = this.obtenerSessionActual();
    var tokenExpirado = null;
    if (session){tokenExpirado = this.tokenExpirado(session);}    
    return (session && session.token && !tokenExpirado) ? session.token : null;
  };

  private tokenExpirado(session: Session) {
    var expiration = new Date(session.expiracion.replace( /(\d{2})\/(\d{2})\/(\d{4})/, "$2/$1/$3"))
    return expiration < new Date();
  }


  logout(): void {
    this.borrarSessionActual();
    this.router.navigate(['/login']);
  }



  public setearExpediente(exp): void {
    this.localStorageService.setItem('expediente', JSON.stringify(exp));
  }

  public hayExpedienteActual(): boolean {
    return this.localStorageService.getItem('expediente') != undefined;
  }
  public obtenerExpedienteActual(): any {
    let exp = this.localStorageService.getItem('expediente');
    return (exp) ? <any>JSON.parse(exp) : null;
  }

  private borrarExpedienteActual(): void {
    this.localStorageService.removeItem('expediente');
    this.localStorageService

  }

}
