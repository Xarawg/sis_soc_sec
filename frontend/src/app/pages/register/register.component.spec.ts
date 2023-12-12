import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminRegisterComponent } from './register.component';

describe('OperatorRegisterComponent', () => {
  let component: AdminRegisterComponent;
  let fixture: ComponentFixture<AdminRegisterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminRegisterComponent]
    });
    fixture = TestBed.createComponent(AdminRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
