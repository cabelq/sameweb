import { Component, OnInit, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Organigrama } from '../../modelos/organigrama';

@Component({
    selector: 'app-page-header',
    templateUrl: './page-header.component.html',
    styleUrls: ['./page-header.component.scss']
})
export class PageHeaderComponent implements OnInit {
    @Input() heading: string;
    @Input() icon: string;
    @Input() dependencia: Organigrama;
    constructor() {}

    ngOnInit() {
       
    }
}
