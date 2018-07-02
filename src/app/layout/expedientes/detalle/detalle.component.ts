import { Component, OnInit } from '@angular/core';
import { routerTransition } from '../../../router.animations';
import {  ToasterService, ToasterConfig } from 'angular2-toaster';

import { Organigrama } from '../../../shared/modelos/organigrama';
import { ExpedienteBody } from '../../../shared/modelos/expedientebody';

import { DatabaseService } from '../../../shared/services/database.service';
import { StorageService } from '../../../shared/services/storage.service';
import { GeneralService } from '../../../shared/services/general.service';


import * as jsPDF from 'jspdf';


@Component({
  selector: 'app-detalle',
  templateUrl: './detalle.component.html',
  styleUrls: ['./detalle.component.scss'],
  animations: [routerTransition()]
})
export class DetalleComponent implements OnInit {
  private toasterService: ToasterService;
  public organigrama: Organigrama;
  public expedientes: any[];
  public pases: any[];
  public respuestas: any[];

  private expedientebody: ExpedienteBody;
  public prefijo: string;
  public numero: string;
  public anio: number;



  public toasterconfig: ToasterConfig =
    new ToasterConfig({
      showCloseButton: false,
      tapToDismiss: false,
      timeout: 5000,
      positionClass: "toast-bottom-right"
    });

  constructor(private storageService: StorageService,
    private databaseService: DatabaseService,
    private generalService: GeneralService,
    toasterService: ToasterService) {
    this.toasterService = toasterService;
  }

  ngOnInit() {

    this.expedientebody = new ExpedienteBody(['', '']);

    if (this.storageService.hayExpedienteActual()) {
      let exp = this.storageService.obtenerExpedienteActual();
      this.anio = exp.anio;
      this.numero = exp.numero;
      this.prefijo = exp.prefijo;
      this.btnFiltrar()
    }

  }


  noPuedeFiltrar() {
    if (this.anio != undefined && this.numero != undefined && this.prefijo != undefined) {
      return false;
    } else {
      return true;
    }
  }

  esArchivo(dep: string) {
    if (dep.toLowerCase().includes('archivo')) {
      return true;
    } else {
      return false;
    }
  }

  tieneRespuestas() {
    if (this.respuestas.length > 0) {
      return true;
    } else {
      return false;
    }
  }

  tienePases() {
    if (this.pases.length > 0) {
      return true;
    } else {
      return false;
    }
  }
  imprimirExpediente() {
    
    var doc = new jsPDF();
    //doc = this.generalService.cabeceraReporte(doc);
    doc.setFont("helvetica");
    //doc.setFontType("bold"); 
    doc.setFontSize(9); 
    doc.text(156, 20, 'MUNICIPALIDAD DE LUJAN');
    doc.setFontSize(7); 
    doc.text(175, 27, this.formatDate(new Date(),2));
    doc.text(175, 292,  this.storageService.obtenerSessionActual().nombre );
    doc.setFontSize(9); 
    doc.text(85, 27, 'INFORME DE EXPEDIENTE');
    doc.roundedRect(13, 10, 190, 20, 2, 2, 'S');

    var img = new Image;
    let me = this;
    img.onload = function () {
      doc.addImage(this, 15, 15);
      doc.save("expediente_" + me.expedientes[0].numero.toString() + "_" + me.expedientes[0].anio.toString()  +  ".pdf");
    };
    img.crossOrigin = "";  // for demo as we are at different origin than image
    img.src = "/assets/images/Same_color.png";

    /* doc.setFont("helvetica");
     doc.setFontType("bold");
     doc.setFontSize(9);
     doc.text(20, 20, 'Cabecera');*/
    //doc.text(20, 30, 'This is client-side Javascript, pumping out a PDF.');
    let fila = 30;
    for (let exp of this.expedientes) {
      fila = fila + 10;
      doc.roundedRect(13, 35, 190, 34, 2, 2, 'S');

      doc.text(15, fila, 'Prejijo ');
      doc.text(35, fila, exp.prefijo);
      doc.text(55, fila, 'Número ');
      doc.text(75, fila, exp.numero);
      doc.text(95, fila, 'Año ');
      doc.text(115, fila, exp.anio.toString());
      if (this.esArchivo(exp.Desc_Organigrama)) {
        doc.text(135, fila, 'Está en Archivo ');
      } else {
        doc.text(135, fila, 'NO Está en Archivo ');
      }
      fila = fila + 8;
      doc.text(15, fila, 'Extracto ');
      doc.text(35, fila, exp.estracto);

      fila = fila + 8;
      doc.text(15, fila, 'Tipo ');
      doc.text(35, fila, exp.Codigo_Tipo);
      doc.text(55, fila, exp.Desc_Tipo);

      doc.text(105, fila, 'Fecha  ');
      doc.text(125, fila, exp.fecha_ingreso);

      fila = fila + 8;
      doc.text(15, fila, 'Organigrama ');
      doc.text(35, fila, exp.id_organigrama);
      doc.text(55, fila, exp.Desc_Organigrama);


      ////////

      fila = fila + 10;


      doc.text(15, fila, 'Iniciador ');
      doc.text(35, fila, exp.iniciador);
      doc.text(135, fila, 'Tipo /Nro Doc ');
      doc.text(155, fila, exp.Tipo_Doc);
      doc.text(175, fila, exp.Nro_Doc.toString());

      fila = fila + 8;
      doc.text(15, fila, 'Direccion: ');
      doc.text(35, fila, exp.Calle);
      doc.text(75, fila, exp.Altura.toString());
      doc.text(90, fila, 'Localidad');
      doc.text(105, fila, exp.Localidad);
      doc.text(135, fila, 'CP');
      doc.text(150, fila, exp.CP.toString());

      fila = fila + 8;
      doc.text(15, fila, 'Telefono ');
      doc.text(25, fila, exp.TE.toString());

      doc.text(85, fila, 'Email  ');
      doc.text(95, fila, exp.Mail);
      doc.roundedRect(13, fila - 20, 190, 26, 2, 2, 'S');

      //doc.roundedRect(18, 35, 180, fila - 20, 2, 2, 'S');
    }
    fila = fila + 15;

    doc.setFontType("bold");
    doc.text(11, fila, 'Pases del Expediente ');

    doc.setFontSize(7);
    fila = fila + 5;
    doc.text(11, fila, 'Id Pase ');
    doc.text(21, fila, 'Id Remito ');
    doc.text(36, fila, 'Fecha Pase ');
    doc.text(53, fila, 'Origen');
    doc.text(100, fila, 'Destino');
    doc.text(150, fila, 'Fecha Rec.');
    doc.text(165, fila, 'Resp Recepcion');
    doc.setFontType("normal");
    for (let pase of this.pases) {
      fila = fila + 5;
      //doc.roundedRect(18, 35, 180, fila, 2, 2, 'S');
      doc.text(11, fila, pase.ID_Pase.toString());
      doc.text(21, fila, pase.Nro_Remito.toString());
 
      doc.text(36, fila, this.formatDate(pase.Fecha_Pase, 1));
      if (pase.ID_Origen.toString().length == 1){
        doc.text(50, fila, '00' + pase.ID_Origen.toString());  
      }else if (pase.ID_Origen.toString().length == 2){
        doc.text(50, fila, '0'+ pase.ID_Origen.toString());  
      }else{
        doc.text(50, fila, pase.ID_Origen.toString());
      }
      
      doc.text(55, fila, pase.Desc_Origen.substring(0, 37));
      
      if (pase.ID_Destino.toString().length == 1){
        doc.text(100, fila, '00' + pase.ID_Destino.toString());  
      }else if (pase.ID_Destino.toString().length == 2){
        doc.text(100, fila, '0'+ pase.ID_Destino.toString());  
      }else{
        doc.text(100, fila, pase.ID_Destino.toString());
      }      
      doc.text(105, fila, pase.Desc_Destino.substring(0, 37));
      if (pase.Fecha_Recepcion != null) {
        doc.text(150, fila, this.formatDate(pase.Fecha_Recepcion, 1));
      }
      if (pase.Responsable_Recepcion != null) {
        doc.text(165, fila, pase.Responsable_Recepcion.substring(0, 25));
      }
    }

    fila = fila + 15;

    doc.setFontType("bold");
    doc.text(11, fila, 'Respuestas del Expediente ');

    doc.setFontSize(7);
    fila = fila + 5;
    doc.text(11, fila, 'Id Rta ');
    doc.text(21, fila, 'Tipo Rta ');
    doc.text(36, fila, 'Fecha Rta ');
    doc.text(53, fila, 'Descripcion');
    doc.text(90, fila, 'Usuario');
    doc.text(115, fila, 'Texto');
    doc.setFontType("normal");
    for (let respuesta of this.respuestas) {
      fila = fila + 5;
      //doc.roundedRect(18, 35, 180, fila, 2, 2, 'S');
      doc.text(11, fila, respuesta.Rta_ID.toString());
      doc.text(21, fila, respuesta.Tipo_Rta.toString());
 
      doc.text(36, fila, this.formatDate(respuesta.Fecha_Rta, 1));
      
      doc.text(55, fila, respuesta.Descripcion);
      
      doc.text(90, fila, respuesta.Usuario.substring(0, 37));
      doc.text(115, fila, respuesta.Texto);
    }

    //doc.save('testCanvas.pdf');

    //console.log(this.expedientes[index]);
    //}

    //var doc = new jsPDF();
    //doc.text(20, 20, 'Hello world!');
    //doc.text(20, 30, 'This is client-side Javascript, pumping out a PDF.');
    //doc.addPage();
    //doc.text(20, 20, 'http://www.coding4developers.com/');

    // Save the PDF



  }

  btnFiltrar() {
    this.expedientebody = new ExpedienteBody(['', '']);
    this.expedientebody.prefijo = this.prefijo;
    this.expedientebody.anio = this.anio;
    this.expedientebody.numero = this.generalService.rellenarCeros(this.numero, 6);
    this.expedientes = [];
    this.pases = [];
    this.respuestas = [];
    /*    if (!this.numero == undefined) {
          if (this.numero.toString().length > 0) {
            this.generalService.rellenarCeros(this.numero, 6)
          }
        }*/
    //this.expedientebody.organigrama = this.storageService.obtenerSessionActual().parametros.organigrama_default.id;

    this.databaseService.getExpediente(new ExpedienteBody(this.expedientebody)).subscribe(
      data => {
        if (data.Table.length > 0) {
          this.expedientes = data.Table;

        }
        else {
          this.toasterService.pop('error', 'Filtro', 'SIN DATOS PARA MOSTRAR');
        }
      },
      error => this.generalService.mostrarError(error, 'error'),
      () => {
        this.databaseService.getPases(new ExpedienteBody(this.expedientebody)).subscribe(
          data => {
            if (data.Table.length > 0) {
              this.pases = data.Table;
            }
            else {
              this.toasterService.pop('error', 'Filtro', 'SIN PASES PARA MOSTRAR');
            }
          },
          error => this.generalService.mostrarError(error, 'error')
        );
        this.databaseService.getRespuestas(new ExpedienteBody(this.expedientebody)).subscribe(
          data => {
            if (data.Table.length > 0) {
              this.respuestas = data.Table;
            }
            else {
              //this.toasterService.pop('error', 'Filtro', 'SIN RESPUESTAS PARA MOSTRAR');
            }
          },
          error => this.generalService.mostrarError(error, 'error')
        );

      }
    )
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
      return currs_date + "/" + currs_month + "/" + curr_year;
    }
    else if (format == 2)// MM/dd/yyyy HH:mm:ss
    {
      return currs_date + "/" + currs_month + "/" + curr_year + " " + currs_hr + ":" + currs_min + ":" + curr_sc;
    }
  }
}
