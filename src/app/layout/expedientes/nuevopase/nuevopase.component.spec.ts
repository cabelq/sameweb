import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NuevopaseComponent } from './nuevopase.component';

describe('NuevopaseComponent', () => {
  let component: NuevopaseComponent;
  let fixture: ComponentFixture<NuevopaseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NuevopaseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NuevopaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
