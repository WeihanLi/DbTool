import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeFirstComponent } from './code-first.component';

describe('CodeFirstComponent', () => {
  let component: CodeFirstComponent;
  let fixture: ComponentFixture<CodeFirstComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CodeFirstComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CodeFirstComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
