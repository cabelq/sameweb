import {UsuarioParametros} from "./usuario-parametros";


export class Session {
    public token: string;
    public usuario: string;
    public expiracion: string;
    public nombre: string;
    public dependencia: string;
    public parametros: UsuarioParametros;
}
