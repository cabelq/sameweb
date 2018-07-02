import { Component, OnInit } from '@angular/core';
import { routerTransition } from '../../../router.animations';


import { DatabaseService } from '../../../shared/services/database.service';
import { StorageService } from '../../../shared/services/storage.service';
import { PagerService } from '../../../shared/services/pager.service';
import { GeneralService } from '../../../shared/services/general.service';

import { ToasterService, ToasterConfig } from 'angular2-toaster';


//import { ArbolorganigramaComponent} from '../arbolorganigrama/arbolorganigrama.component';

import { Organigrama } from '../../../shared/modelos/organigrama';
import { ExpedienteBody } from '../../../shared/modelos/expedientebody';
import { Pase } from '../../../shared/modelos/pase';
import { PaseExpediente } from '../../../shared/modelos/pase-expediente';

import * as jsPDF from 'jspdf';

@Component({
  selector: 'app-nuevopase',
  templateUrl: './nuevopase.component.html',
  styleUrls: ['./nuevopase.component.scss'],
  animations: [routerTransition()]
})
export class NuevopaseComponent implements OnInit {

  private toasterService: ToasterService;

  public organigrama: Organigrama;
  //public expedientessel: any[] = [];
  public expedientes: any[];
  public pase: Pase;

  public remito_nro: number = null;
  // pager object
  pager: any = {};

  // paged items
  pagedItems: any[];

  //public organigramasel: any;
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
    toasterService: ToasterService) {
    this.toasterService = toasterService;
    this.organigrama = storageService.obtenerSessionActual().parametros.organigrama_default;

  }

  ngOnInit() {
    this.pase = new Pase;
    this.expedientebody = new ExpedienteBody(['', '']);
    this.expedientebody.organigrama = this.storageService.obtenerSessionActual().parametros.organigrama_default.id;

    this.databaseService.getExpedientes(new ExpedienteBody(this.expedientebody)).subscribe(
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

    this.databaseService.getExpedientesEnOficinaMasAntiguos(this.storageService.obtenerSessionActual().parametros.organigrama_default.id).subscribe(
      data => {
        this.expedientes = data.Table;

        // initialize to page 1
        this.setPage(1);
      },
      error => this.generalService.mostrarError(error,'error')
    )

  }
  btnFiltrar() {
    this.expedientebody = new ExpedienteBody(['', '']);
    this.expedientebody.prefijo = this.prefijo;
    this.expedientebody.anio = this.anio;
    this.expedientebody.numero = this.numero;
    if (!this.numero == undefined) {
      if (this.numero.toString().length > 0) {
        this.generalService.rellenarCeros(this.numero, 6)
      }
    }
    this.expedientebody.extracto = this.extracto;
    this.expedientebody.organigrama = this.storageService.obtenerSessionActual().parametros.organigrama_default.id;

    this.databaseService.getExpedientes(new ExpedienteBody(this.expedientebody)).subscribe(
      data => {
        if (data.Table.length > 0) {
          this.expedientes = data.Table;
        }
        else {
          this.toasterService.pop('error', 'Filtro', 'SIN DATOS PARA MOSTRAR');
        }
        this.setPage(1);
      },
      error => this.generalService.mostrarError(error,'error')
    )
  }
  sinRecibir(fecha: string) {
    //debugger;
  }
  setPage(page: number) {
    if (page < 1 || page > this.pager.totalPages) {
      return;
    }

    // get pager object from service
    this.pager = this.pagerService.getPager(this.expedientes.length, page, 4);

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


  tieneExpedientesSeleccionados() {
    if (this.pase.expedientes == undefined) {
      return false;
    } else {
      if (this.pase.expedientes.length > 0) {
        return true;
      } else {
        return false;
      }
    }
  }

  tieneDestinoSeleccionado() {
    if (this.pase.organigrama == undefined) {
      return false;
    } else {
      return true;
    }
  }


  tieneRespuestas() {
    for (let exp of this.pase.expedientes){
      if (exp.respuesta == undefined){
        return false;
      }else{
        if (exp.respuesta.trim().length ==0){
          return false;
        }
      }
    }
    return true;
  }


  onSeleccionOrganigrama(nodo: any) {
    let org: Organigrama = new Organigrama;
    org.id = nodo.id;
    org.descripcion = nodo.value;
    this.pase.organigrama = org;
    //this.organigramasel = nodo;
    //agreed ? this.agreed++ : this.disagreed++;
  }

  confirmarPase(){
    this.databaseService.insertPase(this.pase).subscribe(
      data => {        
        this.remito_nro  =data.remito;
      },
      error => this.generalService.mostrarError(error,'error')
    )
    
  }
  
  /*imprimirRemito(){
    this.databaseService.imprimirRemito(this.remito_nro).subscribe(
      data => {      
        
      },
      error => this.generalService.mostrarError(error,'error')
    )
    
  }*/

  imprimirRemito() {
    
    var doc = new jsPDF();
    //doc = this.generalService.cabeceraReporte(doc);
    doc.setFont("helvetica");
    //doc.setFontType("bold"); 
    doc.setFontSize(9); 
    doc.text(156, 20, 'MUNICIPALIDAD DE LUJAN');
    doc.setFontSize(7); 
    doc.text(175, 27, 'Original');
    
    doc.setFontSize(9); 
    doc.text(85, 20, 'REMITO Nro ' + this.remito_nro );
    doc.roundedRect(13, 10, 190, 35, 2, 2, 'S');

    doc.roundedRect(13, 210, 190, 35, 2, 2, 'S');
    doc.text(15, 215, 'Fecha: ');
    doc.text(22, 215, this.formatDate(new Date(),2));
    doc.text(15, 225, 'Emisor: ');
    doc.text(15, 225,  this.storageService.obtenerSessionActual().parametros.organigrama_default.responsable);
    //doc.text(15, 225,  this.storageService.obtenerSessionActual().nombre );
    doc.text(80, 225,  '_________________________________________________' );
    doc.text(80, 227,  'Firma y Aclaracion Responsable' );

    var img = new Image;
    let me = this;
    img.onload = function () {
      doc.addImage(this, 15, 15);
      doc.save("remito_" + me.remito_nro +  ".pdf");
    };
    img.crossOrigin = "";  // for demo as we are at different origin than image
    img.src = "/assets/images/Same_color.png";

    /* doc.setFont("helvetica");
     doc.setFontType("bold");
     doc.setFontSize(9);
     doc.text(20, 20, 'Cabecera');*/
    //doc.text(20, 30, 'This is client-side Javascript, pumping out a PDF.');
    let fila = 30;
    doc.setFontType("bold");    
    doc.text(15, fila, 'Oficina Origen ');
    doc.setFontType("normal");    
    doc.text(45, fila, this.storageService.obtenerSessionActual().dependencia);
    fila = fila + 10;
    doc.setFontType("bold");    
    doc.text(15, fila, 'Oficina Destino ');
    doc.setFontType("normal");    
    doc.text(45, fila, this.pase.organigrama.id.toString() + ' ' +  this.pase.organigrama.descripcion );
    doc.setFontSize(6);
    fila = fila + 5;
    //doc.text(11, fila, 'Id Pase ');
    doc.text(21, fila, 'Expediente ');
    doc.text(86, fila, 'Detalle de Expediente ');

    for (let exp of this.pase.expedientes) {
      fila = fila + 5;
      doc.text(15, fila, exp.prefijo.toString());
      doc.text(22, fila, exp.numero.toString()); 
      doc.text(30, fila, exp.anio.toString());
      doc.text(35, fila, exp.extension);      
      doc.text(45, fila, this.formatDate(exp.fecha,2));      
      doc.text(60, fila, exp.extracto.substring(0, 70));      
      doc.text(150, fila, exp.iniciador.substring(0, 25));      
    }

  }


  btnBorrar(e) {    
    let exp: PaseExpediente = new PaseExpediente;
    exp.anio = e.anio;
    exp.numero = e.numero;
    exp.prefijo = e.prefijo;
    exp.extension = e.extension;
    exp.extracto = e.estracto;
    this.pase.expedientes.splice(
      this.pase.expedientes.findIndex( exped => exped.numero == exp.numero &&  exped.anio == exp.anio ), 1
    );
    
    
    //this.expedientessel.push(e);
  }
 
  btnPasar(e) {    
    let exp: PaseExpediente = new PaseExpediente;
    exp.anio = e.anio;
    exp.numero = e.numero;
    exp.prefijo = e.prefijo;
    exp.extension = e.extension;
    exp.iniciador = e.iniciador;
    exp.fecha = e.fecha_ingreso;
    exp.extracto = e.estracto;
    exp.organigrama_origen = new Organigrama();
    exp.organigrama_origen.id = e.id_organigrama;
    const resultado = this.pase.expedientes.find( exped => exped.numero == exp.numero &&  exped.anio == exp.anio );
    if (resultado == undefined){
      this.pase.expedientes.push(exp);
    }
    
    //this.expedientessel.push(e);
  }
 
  convertHtmlToRtf(html) {
    if (!(typeof html === "string" && html)) {
      return null;
    }

    var tmpRichText, hasHyperlinks;
    var richText = html;

    // Singleton tags
    richText = richText.replace(/<(?:hr)(?:\s+[^>]*)?\s*[\/]?>/ig, "{\\pard \\brdrb \\brdrs \\brdrw10 \\brsp20 \\par}\n{\\pard\\par}\n");
    richText = richText.replace(/<(?:br)(?:\s+[^>]*)?\s*[\/]?>/ig, "{\\pard\\par}\n");

    // Empty tags
    richText = richText.replace(/<(?:p|div|section|article)(?:\s+[^>]*)?\s*[\/]>/ig, "{\\pard\\par}\n");
    richText = richText.replace(/<(?:[^>]+)\/>/g, "");

    // Hyperlinks
    richText = richText.replace(
      /<a(?:\s+[^>]*)?(?:\s+href=(["'])(?:javascript:void\(0?\);?|#|return false;?|void\(0?\);?|)\1)(?:\s+[^>]*)?>/ig,
      "{{{\n");
    tmpRichText = richText;
    richText = richText.replace(
      /<a(?:\s+[^>]*)?(?:\s+href=(["'])(.+)\1)(?:\s+[^>]*)?>/ig,
      "{\\field{\\*\\fldinst{HYPERLINK\n \"$2\"\n}}{\\fldrslt{\\ul\\cf1\n");
    hasHyperlinks = richText !== tmpRichText;
    richText = richText.replace(/<a(?:\s+[^>]*)?>/ig, "{{{\n");
    richText = richText.replace(/<\/a(?:\s+[^>]*)?>/ig, "\n}}}");

    // Start tags
    richText = richText.replace(/<(?:b|strong)(?:\s+[^>]*)?>/ig, "{\\b\n");
    richText = richText.replace(/<(?:i|em)(?:\s+[^>]*)?>/ig, "{\\i\n");
    richText = richText.replace(/<(?:u|ins)(?:\s+[^>]*)?>/ig, "{\\ul\n");
    richText = richText.replace(/<(?:strike|del)(?:\s+[^>]*)?>/ig, "{\\strike\n");
    richText = richText.replace(/<sup(?:\s+[^>]*)?>/ig, "{\\super\n");
    richText = richText.replace(/<sub(?:\s+[^>]*)?>/ig, "{\\sub\n");
    richText = richText.replace(/<(?:p|div|section|article)(?:\s+[^>]*)?>/ig, "{\\pard\n");

    // End tags
    richText = richText.replace(/<\/(?:p|div|section|article)(?:\s+[^>]*)?>/ig, "\n\\par}\n");
    richText = richText.replace(/<\/(?:b|strong|i|em|u|ins|strike|del|sup|sub)(?:\s+[^>]*)?>/ig, "\n}");

    // Strip any other remaining HTML tags [but leave their contents]
    richText = richText.replace(/<(?:[^>]+)>/g, "");

    // Prefix and suffix the rich text with the necessary syntax
    richText =
      "{\\rtf1\\ansi\n" + (hasHyperlinks ? "{\\colortbl\n;\n\\red0\\green0\\blue255;\n}\n" : "") + richText +
      "\n}";

    return richText;
  }
}
