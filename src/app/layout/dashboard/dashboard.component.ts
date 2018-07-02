import { Component, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { routerTransition } from '../../router.animations';
import { DatabaseService } from '../../shared/services/database.service'
import { StorageService } from '../../shared/services/storage.service'
import { PagerService } from '../../shared/services/pager.service'
import { Router } from '@angular/router';

import { Estadistica } from '../../shared/modelos/estadistica';
import { Organigrama } from '../../shared/modelos/organigrama';
import { ExpedienteBody } from '../../shared/modelos/expedientebody';
import { GeneralService } from '../../shared';


@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
    animations: [routerTransition()]
})
export class DashboardComponent implements OnInit {
    //public organigrama:string;
    public alerts: Array<any> = [];
    public sliders: Array<any> = [];
    public estadistica: Estadistica = new Estadistica();
    public expedientes: any[];
    public expedientessinrecibir: any[];
    public expedientessinrecepcion: any[];
    public organigrama: Organigrama;

    // pager object
    pager: any = {};

    // paged items
    pagedItems: any[];

    constructor(private storageService: StorageService,
        private databaseService: DatabaseService,
        private router: Router,
        private generalService: GeneralService,
        private pagerService: PagerService) {
        this.organigrama = storageService.obtenerSessionActual().parametros.organigrama_default;
        this.alerts.push(
            {
                id: 1,
                type: 'success',
                message: `Lorem ipsum dolor sit amet, consectetur adipisicing elit.
                Voluptates est animi quibusdam praesentium quam, et perspiciatis,
                consectetur velit culpa molestias dignissimos
                voluptatum veritatis quod aliquam! Rerum placeat necessitatibus, vitae dolorum`
            },
            {
                id: 2,
                type: 'warning',
                message: `Lorem ipsum dolor sit amet, consectetur adipisicing elit.
                Voluptates est animi quibusdam praesentium quam, et perspiciatis,
                consectetur velit culpa molestias dignissimos
                voluptatum veritatis quod aliquam! Rerum placeat necessitatibus, vitae dolorum`
            }
        );
    }

    ngOnInit() {

        this.databaseService.getEstadisticas(this.storageService.obtenerSessionActual().parametros.organigrama_default.id).subscribe(
            data => this.estadistica = data,
            error => this.generalService.mostrarError(error,'error')//this.error = JSON.parse(error._body)
        )

        this.databaseService.getExpedientesEnOficinaMasAntiguos(this.storageService.obtenerSessionActual().parametros.organigrama_default.id).subscribe(
            data => {
                this.expedientes = data.Table;

                // initialize to page 1
                this.setPage(1);
            },
            error => this.generalService.mostrarError(error,'error')
        )

        this.databaseService.getExpedientesSinRecibir(this.storageService.obtenerSessionActual().parametros.organigrama_default.id).subscribe(
            data => this.expedientessinrecibir = data.Table,
            error => this.generalService.mostrarError(error,'error')
        )
        this.databaseService.getExpedientesSinRecepcion(this.storageService.obtenerSessionActual().parametros.organigrama_default.id).subscribe(
            data => this.expedientessinrecepcion = data.Table,
            error => this.generalService.mostrarError(error,'error')
        )
        //this.organigrama = this.storageService.obtenerSessionActual().parametros.organigrama_default;
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

    verExpediente(exp) {
        debugger;
        let expedientebody = new ExpedienteBody(['', '']);
        expedientebody.anio = exp.anio;
        expedientebody.prefijo = exp.prefijo;
        expedientebody.numero = exp.numero;
        this.storageService.setearExpediente(expedientebody);
        this.router.navigate(['/detalle']);
    }

    public claseExpOfi(): string {
        switch (true) {
            case this.estadistica.expedientes_en_oficina < 10: {
                return 'success';
            }
            case this.estadistica.expedientes_en_oficina < 10 && this.estadistica.expedientes_en_oficina < 20: {
                return 'warning';
            }
            case this.estadistica.expedientes_en_oficina > 20: {
                return 'danger'
            }
            default: return 'success'
        }

    }

    public claseExpSinRecOtraOfi(): string {
        switch (true) {
            case this.estadistica.exp_sin_rec_en_otra_ofi < 10: {
                return 'success';
            }
            case this.estadistica.exp_sin_rec_en_otra_ofi < 10 && this.estadistica.exp_sin_rec_en_otra_ofi < 20: {
                return 'warning';
            }
            case this.estadistica.exp_sin_rec_en_otra_ofi > 20: {
                return 'danger'
            }
            default: return 'success'
        }

    }
    public claseExpSinRecMiOfi(): string {
        switch (true) {
            case this.estadistica.exp_sin_rec_en_mi_ofi < 10: {
                return 'success';
            }
            case this.estadistica.exp_sin_rec_en_mi_ofi < 10 && this.estadistica.exp_sin_rec_en_mi_ofi < 20: {
                return 'warning';
            }
            case this.estadistica.exp_sin_rec_en_mi_ofi > 20: {
                return 'danger'
            }
            default: return 'success'
        }

    }

    public claseExpSolOtraOfi(): string {
        switch (true) {
            case this.estadistica.exp_sol_por_otra_ofi < 10: {
                return 'success';
            }
            case this.estadistica.exp_sol_por_otra_ofi < 10 && this.estadistica.exp_sol_por_otra_ofi < 20: {
                return 'warning';
            }
            case this.estadistica.exp_sol_por_otra_ofi > 20: {
                return 'danger'
            }
            default: return 'success'
        }

    }


    public closeAlert(alert: any) {
        const index: number = this.alerts.indexOf(alert);
        this.alerts.splice(index, 1);
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
