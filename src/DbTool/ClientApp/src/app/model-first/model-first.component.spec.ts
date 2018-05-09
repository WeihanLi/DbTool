import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModelFirstComponent } from './model-first.component';

describe('ModelFirstComponent', () => {
  let component: ModelFirstComponent;
  let fixture: ComponentFixture<ModelFirstComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModelFirstComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModelFirstComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
