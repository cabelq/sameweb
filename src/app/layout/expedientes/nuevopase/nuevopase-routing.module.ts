import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NuevopaseComponent } from './nuevopase.component';

const routes: Routes = [
    {
        path: '', component: NuevopaseComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class NuevopaseRoutingModule {
}
