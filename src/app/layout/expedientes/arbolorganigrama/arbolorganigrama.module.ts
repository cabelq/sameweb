import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
    
import { ArbolorganigramaRoutingModule } from './arbolorganigrama-routing.module';
import { PageHeaderModule } from './../../../shared';
import { FormsModule } from '@angular/forms';
import { ToasterModule } from 'angular2-toaster';
import { TreeModule } from 'ng2-tree';
import { TreeViewModule } from '@progress/kendo-angular-treeview';
  

@NgModule({
    imports: [CommonModule, 
        ArbolorganigramaRoutingModule, 
        PageHeaderModule,
        FormsModule,
        ToasterModule,
        TreeModule, TreeViewModule],
    declarations: []
})
export class ArbolorganigramaModule {}