import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';

import { LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponent } from './layout.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { HeaderComponent } from './components/header/header.component';
import { RecepcionComponent } from './components/expedientes/recepcion/recepcion.component';
//import { ArbolorganigramaComponent } from './components/expedientes/arbolorganigrama/arbolorganigrama.component';
//import { ArbolorganigramaComponent } from './expedientes/arbolorganigrama/arbolorganigrama.component';
//import { NuevopaseComponent } from './expedientes/nuevopase/nuevopase.component';

import {ToasterModule, ToasterService} from 'angular2-toaster';
import { TreeModule } from 'ng2-tree';

import { FormsModule } from '@angular/forms';

//import { SeleccionComponent } from './expedientes/nuevopase/seleccion/seleccion.component';


@NgModule({
    imports: [
        CommonModule,
        LayoutRoutingModule,
        TranslateModule,
        FormsModule,
        ToasterModule,
        TreeModule,
        NgbDropdownModule.forRoot()
    ],
    declarations: [LayoutComponent, 
        SidebarComponent, 
        HeaderComponent, 
        RecepcionComponent
        //, ArbolorganigramaComponent,
        //NuevopaseComponent //, SeleccionComponent
    ]
})
export class LayoutModule {}
