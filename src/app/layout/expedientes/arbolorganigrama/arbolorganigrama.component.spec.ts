import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArbolorganigramaComponent } from './arbolorganigrama.component';

describe('ArbolorganigramaComponent', () => {
  let component: ArbolorganigramaComponent;
  let fixture: ComponentFixture<ArbolorganigramaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArbolorganigramaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArbolorganigramaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
