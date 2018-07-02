export class ExpedienteBody {
    public prefijo : String;
    public numero : String;
    public anio : number;
    public organigrama : number;
    public extracto : String;
    public iniciador : String;
    constructor( object: any){
        this.prefijo = (object.prefijo) ? object.prefijo : null;
        this.numero = (object.numero) ? object.numero : null;
        this.anio = (object.anio) ? object.anio : null;
        this.organigrama = (object.organigrama) ? object.organigrama : null;
        this.extracto = (object.extracto) ? object.extracto : null;
        this.iniciador = (object.iniciador) ? object.iniciador : null;
      }
}