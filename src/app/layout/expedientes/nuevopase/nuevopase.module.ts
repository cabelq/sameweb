import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NuevopaseRoutingModule } from './nuevopase-routing.module';
import { NuevopaseComponent } from './nuevopase.component';
import { PageHeaderModule } from './../../../shared';

import { FormsModule } from '@angular/forms';
import { ToasterModule} from 'angular2-toaster';

//import { ArbolorganigramaModule } from '../arbolorganigrama/arbolorganigrama.module';
import { ArbolorganigramaComponent } from './../arbolorganigrama/arbolorganigrama.component';
import { TreeModule } from 'ng2-tree';
import { ArchwizardModule } from 'angular-archwizard'; 
import { CKEditorModule } from 'ng2-ckeditor';

@NgModule({
    imports: [CommonModule, 
        NuevopaseRoutingModule, 
        PageHeaderModule,
        FormsModule, 
        ToasterModule,ArchwizardModule, 
        TreeModule,
        CKEditorModule
    ],
    declarations: [NuevopaseComponent,ArbolorganigramaComponent]
})
export class NuevopaseModule {}