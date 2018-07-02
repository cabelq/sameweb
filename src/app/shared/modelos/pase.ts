import { Organigrama }  from './organigrama';
import { PaseExpediente }  from './pase-expediente';

export class Pase {
    public organigrama:Organigrama;    
    public expedientes : PaseExpediente[];
    constructor(){
        this.organigrama = null;        
        this.expedientes = [];
      }
}