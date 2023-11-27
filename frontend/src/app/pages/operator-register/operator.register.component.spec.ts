import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OperatorRegisterComponent } from './operator.register.component';

describe('OperatorRegisterComponent', () => {
  let component: OperatorRegisterComponent;
  let fixture: ComponentFixture<OperatorRegisterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OperatorRegisterComponent]
    });
    fixture = TestBed.createComponent(OperatorRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
