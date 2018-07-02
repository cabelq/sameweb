import { Component, AfterViewInit,ViewChild,Output,EventEmitter } from '@angular/core';
import { DatabaseService } from '../../../shared/services/database.service';
import { StorageService } from '../../../shared/services/storage.service';
import { ToasterService, ToasterConfig } from 'angular2-toaster';
import { TreeModel,NodeEvent,NodeMenuItemAction } from 'ng2-tree';
import { GeneralService } from '../../../shared';
 
@Component({
  selector: 'app-arbolorganigrama',
  templateUrl: './arbolorganigrama.component.html',
  styleUrls: ['./arbolorganigrama.component.scss']
})
export class ArbolorganigramaComponent implements AfterViewInit {

  @Output() onSeleccion = new EventEmitter<any>();
  public organigrama: any[];
  @ViewChild('treeComponent') arbolComponent;
  private toasterService: ToasterService;
  public toasterconfig: ToasterConfig =
    new ToasterConfig({
      showCloseButton: false,
      tapToDismiss: false,
      timeout: 5000,
      positionClass: "toast-bottom-right"
    });

  public arbolModelo: TreeModel; 
  public data: any[] = [
    {
        text: 'Furniture', items: [
            { text: 'Tables & Chairs' },
            { text: 'Sofas' },
            { text: 'Occasional Furniture' }
        ]
    },
    {
        text: 'Decor', items: [
            { text: 'Bed Linen' },
            { text: 'Curtains & Blinds' },
            { text: 'Carpets' }
        ]
    }
];
  
  constructor(private storageService: StorageService,
    private databaseService: DatabaseService,
    private generalService:GeneralService,
    toasterService: ToasterService
  ) {}

  ngAfterViewInit() {
    let miarbolModelo : TreeModel = {
      value: 'Organigrama',
      settings: {
        'static': true,
        'rightMenu': true,
        'leftMenu': true,
        'cssClasses': {
          'expanded': 'fa fa-caret-down fa-lg',
          'collapsed': 'fa fa-caret-right fa-lg',
          'leaf': 'fa fa-lg ',
          'empty': 'fa fa-caret-right disabled'
        },
        'isCollapsedOnInit':true,
        'templates': {
          'node': '<i class="fa fa-folder-o fa-lg"></i>',
          'leaf': '<i class="fa fa-file-o fa-lg " style="color:red"></i>',
          'leftMenu': '<i class="fa fa-navicon fa-lg"></i>'
        },
        'menuItems': [
            { action: NodeMenuItemAction.Custom, name: 'Foo', cssClass: 'fa fa-arrow-right' },
            { action: NodeMenuItemAction.Custom, name: 'Bar', cssClass: 'fa fa-arrow-right' },
            { action: NodeMenuItemAction.Custom, name: 'Baz', cssClass: 'fa fa-arrow-right'}
          ]
        },
      id:0,
      children: []
    };
    this.databaseService.getOrganigrama().subscribe(
      data => {    
        //let padre : TreeController;
        
        for (let item of data.Table) {
          //this.padre = this.arbolComponent.getControllerByNodeId(0);
          let nodo: TreeModel = {
            value: item.ORG_Codigo + ' ' + item.ORG_Descripcion,
            id: item.ORG_Id_Organigrama /*,
            children: []*/
          };

          let hoja: TreeModel = {
            value: item.ORG_Codigo + ' ' + item.ORG_Descripcion,
            id: item.ORG_Id_Organigrama 
          };


          if (item.ORG_Nivel == 0) {
              
            //padre = this.arbolComponent.getController();    
            miarbolModelo.children.push(nodo);
            //padre.addChild(nodo);   
          } else {
           // debugger;
           //if (item.ORG_Nivel == 1) {
            // debugger;
            let padre = this.buscarArbol(miarbolModelo,item.ORG_Enlace_P_H);//this.arbolComponent.getController();//this.arbolComponent.getControllerByNodeId(1);
            if (padre != undefined){              
              if (padre.children == undefined ){
                let hijos = {               
                  children: []
                };
                hijos.children.push(nodo);
                Object.assign(padre,hijos);
  
              }else{
                padre.children.push(nodo);  
              }
            }
            
          }
          
        }
      },
      error => this.generalService.mostrarError(error,'error'),
      ()=>{
        this.arbolModelo = miarbolModelo;
      }
    )
  }
  public logEvent(e: NodeEvent): void {
    console.log(e.node.children);
    if (e.node.children == undefined){
      this.arbolComponent.getController().collapse();
      this.onSeleccion.emit(e.node.node);  
    }else{
      this.toasterService.pop('success', 'Info', 'Item no seleccionable');
    }
  }

  private buscarArbol(arbol:any, id:number) {
    for (let item of arbol.children){     
      if (item.id == id){
        return item;
      }else {      
        if (item.children != undefined){
          if (item.children.length > 0){
            let encontre =  this.buscarArbol(item, id);
            if (encontre != undefined){
              return encontre;
            }
          }  
        }  
      }
    }    
  }

}
