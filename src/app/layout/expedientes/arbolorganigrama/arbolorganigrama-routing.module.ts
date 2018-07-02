import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ArbolorganigramaComponent } from './arbolorganigrama.component';

const routes: Routes = [
    {
        path: '', component: ArbolorganigramaComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ArbolorganigramaRoutingModule {
}
