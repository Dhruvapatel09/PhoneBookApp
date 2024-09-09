import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgotpasswordsuccessComponent } from './forgotpasswordsuccess.component';

describe('ForgotpasswordsuccessComponent', () => {
  let component: ForgotpasswordsuccessComponent;
  let fixture: ComponentFixture<ForgotpasswordsuccessComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ForgotpasswordsuccessComponent]
    });
    fixture = TestBed.createComponent(ForgotpasswordsuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
