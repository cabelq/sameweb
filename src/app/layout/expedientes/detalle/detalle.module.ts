import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DetalleRoutingModule } from './detalle-routing.module';
import { DetalleComponent } from './detalle.component';
import { PageHeaderModule } from './../../../shared';
import { FormsModule } from '@angular/forms';
import { ToasterModule } from 'angular2-toaster';



@NgModule({
    imports: [CommonModule, 
        DetalleRoutingModule, 
        PageHeaderModule,
        FormsModule,
        ToasterModule 
        ],
    declarations: [DetalleComponent]
})
export class DetalleModule {}