import { Component, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { routerTransition } from '../../../router.animations';
import { Router } from '@angular/router';


import { DatabaseService } from '../../../shared/services/database.service';
import { StorageService } from '../../../shared/services/storage.service';
import { PagerService } from '../../../shared/services/pager.service';
import { GeneralService } from '../../../shared/services/general.service';

import { ToasterService, ToasterConfig } from 'angular2-toaster';

//import { ArbolorganigramaComponent} from '../arbolorganigrama/arbolorganigrama.component';

import { Organigrama } from '../../../shared/modelos/organigrama';
import { ExpedienteBody } from '../../../shared/modelos/expedientebody';


@Component({
    selector: 'app-recepcion',
    templateUrl: './recepcion.component.html',
    styleUrls: ['./recepcion.component.scss'],
    animations: [routerTransition()]
})
export class RecepcionComponent implements OnInit {
    private toasterService: ToasterService;


    public organigrama: Organigrama;
    public expedientes: any[];
    // pager object
    pager: any = {};

    // paged items
    pagedItems: any[];

    public prefijo: string;
    public numero: string;
    public anio: number;
    public extracto: string;
    private expedientebody: ExpedienteBody;

    public toasterconfig: ToasterConfig =
        new ToasterConfig({
            showCloseButton: false,
            tapToDismiss: false,
            timeout: 5000,
            positionClass: "toast-bottom-right"
        });


    constructor(private storageService: StorageService,
        private databaseService: DatabaseService,
        private pagerService: PagerService,
        private generalService: GeneralService,
        private router: Router,
        toasterService: ToasterService) {
        this.toasterService = toasterService;
        this.organigrama = storageService.obtenerSessionActual().parametros.organigrama_default;
    }

    ngOnInit() {
        this.expedientebody = new ExpedienteBody(['', '']);
        this.expedientebody.organigrama = this.storageService.obtenerSessionActual().parametros.organigrama_default.id;

        this.databaseService.getExpedientesSinRecibir(this.storageService.obtenerSessionActual().parametros.organigrama_default.id).subscribe(
            data => {

                if (data.Table.length > 0) {
                    this.expedientes = data.Table;
                }
                else {
                    this.expedientes = [];
                }

                this.setPage(1);
            },
            error => this.generalService.mostrarError(error,'error')
        )

    }



    recepcionPase(pase: number) {
        debugger;
        this.databaseService.recepcionPase(pase).subscribe(
            data => {
                this.toasterService.pop('info', 'Info', 'Recepción satisfactoria');
                this.router.navigate(['/dashboard']);
            },
            error => this.generalService.mostrarError(error,'error')
        )
    }


    sinRecibir(fecha: string) {

    }
    btnFiltrar() {
        debugger;
        this.expedientebody = new ExpedienteBody(['', '']);
        this.expedientebody.prefijo = this.prefijo;
        this.expedientebody.anio = this.anio;
        this.expedientebody.numero = this.numero;
        if (!this.numero == undefined) {
            if (this.numero.toString().length > 0) {
                this.generalService.rellenarCeros(this.numero, 6)
            }
        }
        //this.expedientebody.numero = ((this.numero.toString().length > 0) ? this.generalService.rellenarCeros(this.numero,6) : this.numero);
        this.expedientebody.extracto = this.extracto;
        this.expedientebody.organigrama = this.storageService.obtenerSessionActual().parametros.organigrama_default.id;
        this.databaseService.getExpedientesSinRecibir(this.storageService.obtenerSessionActual().parametros.organigrama_default.id).subscribe(
            data => {
                if (data.Table.length > 0) {
                    this.expedientes = data.Table;
                }
                else {
                    this.toasterService.pop('error', 'Filtro', 'SIN DATOS PARA MOSTRAR');
                    //this.expedientes = [];
                }
                this.setPage(1);
            },
            error => this.generalService.mostrarError(error,'error')
        )

        
    }
    setPage(page: number) {
        if (page < 1 || page > this.pager.totalPages) {
            return;
        }

        // get pager object from service
        this.pager = this.pagerService.getPager(this.expedientes.length, page);

        // get current page of items
        this.pagedItems = this.expedientes.slice(this.pager.startIndex, this.pager.endIndex + 1);
    }
    public formatDate(fecha, format) {
        var currs_month: string;
        var currs_date: string;
        var currs_hr: string;
        var currs_min: string;

        let dateObj = new Date(fecha);
        var monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Dicembre"];
        var curr_date = dateObj.getDate();
        var curr_month = dateObj.getMonth();

        curr_month = curr_month + 1;
        var curr_year = dateObj.getFullYear();
        var curr_min = dateObj.getMinutes();
        var curr_hr = dateObj.getHours();
        var curr_sc = dateObj.getSeconds();
        currs_month = curr_month.toString();
        if (curr_month.toString().length == 1)
            currs_month = '0' + curr_month;
        currs_date = curr_date.toString();
        if (curr_date.toString().length == 1)
            currs_date = '0' + curr_date;
        currs_hr = curr_hr.toString();
        if (curr_hr.toString().length == 1)
            currs_hr = '0' + curr_hr;
        currs_min = curr_min.toString();
        if (curr_min.toString().length == 1)
            currs_min = '0' + curr_min;

        if (format == 1)//dd/mm/yyyy
        {
            return curr_date + "/" + curr_month + "/" + curr_year;
        }
        else if (format == 2)// MM/dd/yyyy HH:mm:ss
        {
            return currs_date + "/" + currs_month + "/" + curr_year + " " + currs_hr + ":" + currs_min + ":" + curr_sc;
        }
    }
}
