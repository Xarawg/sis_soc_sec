import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OperatorStartComponent } from './operator.start.component';

describe('OperatorStartComponent', () => {
  let component: OperatorStartComponent;
  let fixture: ComponentFixture<OperatorStartComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OperatorStartComponent]
    });
    fixture = TestBed.createComponent(OperatorStartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
