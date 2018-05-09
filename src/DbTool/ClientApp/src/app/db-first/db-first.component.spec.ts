import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DbFirstComponent } from './db-first.component';

describe('DbFirstComponent', () => {
  let component: DbFirstComponent;
  let fixture: ComponentFixture<DbFirstComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DbFirstComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DbFirstComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
