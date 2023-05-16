import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SftoolsloginComponent } from './sftoolslogin.component';

describe('SftoolsloginComponent', () => {
  let component: SftoolsloginComponent;
  let fixture: ComponentFixture<SftoolsloginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SftoolsloginComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SftoolsloginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
