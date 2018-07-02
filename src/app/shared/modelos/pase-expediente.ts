import { Organigrama }  from './organigrama';

export class PaseExpediente {
    public prefijo : String;
    public numero : String;
    public anio : number;
    public extension : String;
    public extracto : String;
    public iniciador : String;
    public fecha : String;
    public respuesta : String;
    public organigrama_origen:Organigrama;
    constructor(){
        this.prefijo = null;
        this.numero = null;
        this.anio = null;
        this.extension = null;
        this.extracto = null;
        this.respuesta = null;
        this.organigrama_origen = null;
      }    
}