import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { StorageService } from '../../../shared/services/storage.service';
import { Organigrama } from '../../../shared/modelos/organigrama';


@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
    pushRightClass: string = 'push-right';

    nombreUsuario:string;
    dependencia:Organigrama;
    dependencia_otras:Organigrama[]= [];
    //dependencia_otras_id:string[]= [];

    constructor(private translate: TranslateService, public router: Router,private storageService: StorageService) {

        this.translate.addLangs(['en', 'fr', 'ur', 'es', 'it', 'fa', 'de', 'zh-CHS']);
        this.translate.setDefaultLang('en');
        const browserLang = this.translate.getBrowserLang();
        this.translate.use(browserLang.match(/en|fr|ur|es|it|fa|de|zh-CHS/) ? browserLang : 'en');

        this.router.events.subscribe(val => {
            if (
                val instanceof NavigationEnd &&
                window.innerWidth <= 992 &&
                this.isToggled()
            ) {
                this.toggleSidebar();
            }
        }); 
        
        this.nombreUsuario = this.storageService.obtenerSessionActual().nombre;
        this.dependencia = this.storageService.obtenerSessionActual().parametros.organigrama_default;
        this.dependencia_otras = this.storageService.obtenerSessionActual().parametros.organigramas;
        //this.dependencia_otras_id = this.storageService.obtenerSessionActual().parametros.id_organigrama_otros.split("-");
    }

    ngOnInit() {}

    isToggled(): boolean {
        const dom: Element = document.querySelector('body');
        return dom.classList.contains(this.pushRightClass);
    }

    toggleSidebar() {
        const dom: any = document.querySelector('body');
        dom.classList.toggle(this.pushRightClass);
    }

    rltAndLtr() {
        const dom: any = document.querySelector('body');
        dom.classList.toggle('rtl');
    }

    onLoggedout() {
        localStorage.removeItem('isLoggedin');
    }

    changeLang(language: string) {
        this.translate.use(language);
    }

    onChangeOrganigrama(org:any) {        
        this.storageService.setearOrganigramaDefault(org);        
        this.dependencia = org;        
        this.router.navigate(['/blank-page']).then(()=>this.router.navigate(['/dashboard']));
        //this.router.navigate(['/dashboard']);
    }

}
