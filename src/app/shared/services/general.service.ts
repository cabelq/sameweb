import { Injectable } from '@angular/core';
import { ToasterService } from 'angular2-toaster';
import { Router } from '@angular/router';
import * as html2canvas from "html2canvas";

@Injectable()
export class GeneralService {

    constructor(private toasterService: ToasterService, private router: Router) {

    }

    public rellenarCeros(number, width) {
        var numberOutput = Math.abs(number); /* Valor absoluto del número */
        var length = number.toString().length; /* Largo del número */
        var zero = "0"; /* String de cero */

        if (width <= length) {
            if (number < 0) {
                return ("-" + numberOutput.toString());
            } else {
                return numberOutput.toString();
            }
        } else {
            if (number < 0) {
                return ("-" + (zero.repeat(width - length)) + numberOutput.toString());
            } else {
                return ((zero.repeat(width - length)) + numberOutput.toString());
            }
        }
    }
    public mostrarError(error: any, tipo: string) {
        debugger;
        if (error.status == 401) {
            this.toasterService.pop(tipo, '', JSON.parse(error._body).mensaje);
            this.router.navigate(['/login']);

        } else {
            if (JSON.parse(error._body).mensaje != undefined) {
                this.toasterService.pop(tipo, '', JSON.parse(error._body).mensaje);
            } else {
                this.toasterService.pop(tipo, '', JSON.parse(error._body));
            }
        }

    }
    private getDataUri(url,width,height, callback) {
        var imagen = new Image();
    
        imagen.onload = function () {
            var canvas = document.createElement('canvas');
            canvas.width = width; // or 'width' if you want a special/scaled size
            canvas.height = height; // or 'height' if you want a special/scaled size
    
            canvas.getContext('2d').drawImage(imagen, 0, 0);
    
            // Get raw image data
            //callback(canvas.toDataURL('image/png').replace(/^data:image\/(png|jpg);base64,/, ''));
    
            // ... or get as Data URI
            callback(canvas.toDataURL('image/png'));
        };
    
        imagen.src = url;
    }
    
    public cabeceraReporte(doc: any):any {
        doc.setFont("helvetica");
        //doc.setFontType("bold");
        doc.setFontSize(9);
        doc.text(120, 20, 'MUNICIPALIDAD DE LUJAN');
        doc.roundedRect(20 ,10 ,160,20,2,2,'S');

        var img = new Image;
img.onload = function() {
    doc.addImage(this, 10, 10);
    //doc.save("test.pdf");
    return doc;
};
img.crossOrigin = "";  // for demo as we are at different origin than image
img.src = "/assets/images/Same_color.png";  





        /*this.getDataUri('assets/images/Same_color.png',120,30, function(dataUri) {
            // Do whatever you'd like with the Data URI! 
            console.log(dataUri);           
            doc.addImage(dataUri, 'png', 20, 10);
        });*/
        /*let canvas = document.createElement('canvas');
        document.body.appendChild(canvas);
        canvas.width = 40;
        canvas.height =30;

       //add the images
        let base_image = new Image();
        base_image.src = 'assets/images/Same_color.png';
        let context = canvas.getContext('2d');
        context.drawImage(base_image,0,0,120,30);

        //now grab the one image data for jspdf
        //let imgData = canvas.toDataURL('image/png');
        var imgData = 'data:image/png;base64,'+ btoa('assets/images/Same_color.png');
    console.log(imgData);
    doc.setFontSize(40);
    doc.text(30, 20, 'Hello world!');
    doc.addImage(imgData, 'JPEG', 15, 40, 180, 160);
        console.log(imgData);
        //and lose the canvas when you're done
        document.body.removeChild(canvas);


        //let imgData = 'data:image/png;base64,'+ btoa('assets/images/logo.png');        
        doc.addImage(imgData, 'PNG', 15, 15, 120, 30);
*/
    }
}
