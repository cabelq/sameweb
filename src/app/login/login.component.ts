import { Component, AfterViewInit } from '@angular/core';
import { FormControl, FormGroup, FormArray, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { routerTransition } from '../router.animations';


import { AutorizacionService } from "../shared/services/autorizacion.service";
import { StorageService } from "../shared/services/storage.service";

import { Login } from "../shared/modelos/login";
import { Session } from "../shared/modelos/session";
import { GeneralService } from '../shared';
import { ToasterConfig } from 'angular2-toaster';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    animations: [routerTransition()]
})
export class LoginComponent implements AfterViewInit {

    login: Login;
    public toasterconfig: ToasterConfig =
        new ToasterConfig({
            showCloseButton: false,
            tapToDismiss: false,
            timeout: 5000,
            positionClass: "toast-bottom-right"
        });


    form = new FormGroup({
        usuario: new FormControl(),
        password: new FormControl(),
    });

    constructor(public router: Router,
        private autenticacionService: AutorizacionService,
        private storageService: StorageService,
        private generalService:GeneralService
    ) {
        this.login = new Login(['', '']);
    }

    ngAfterViewInit(): void {
        //this.form.get('name').valueChanges.subscribe(data => this.usrNameChanges = data);
        //this.form.get('name').statusChanges.subscribe(data => this.usrNameStatus = data);
    }

    onLoggedin() {             
        this.login.usuario = this.form.value.usuario;
        this.login.password =this.form.value.password;        
        this.autenticacionService.login(new Login(this.login)).subscribe(
            data => this.correctLogin(data),
            error => this.generalService.mostrarError(error,'error')//this.error = JSON.parse(error._body)
        )

    }

    private correctLogin(data: Session) {
        debugger;
        this.storageService.setearSessionActual(data);
        this.router.navigate(['/dashboard']);
    }

}
