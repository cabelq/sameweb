import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RecepcionRoutingModule } from './recepcion-routing.module';
import { RecepcionComponent } from './recepcion.component';
import { PageHeaderModule } from './../../../shared';
import { FormsModule } from '@angular/forms';
import { ToasterModule, ToasterService } from 'angular2-toaster';



@NgModule({
    imports: [CommonModule, 
        RecepcionRoutingModule, 
        PageHeaderModule,
        FormsModule, 
        ToasterModule],
    declarations: [RecepcionComponent]
})
export class RecepcionModule {}