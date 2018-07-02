import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Router } from '@angular/router';
import { StorageService } from '../services/storage.service';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private router: Router,private storageService: StorageService) {}

    canActivate() {
        
        if (this.storageService.estaAutenticado()){
        //if (localStorage.getItem('isLoggedin')) {
            return true;
        }

        this.router.navigate(['/login']);
        return false;
    }
}
