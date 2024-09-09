import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddContactRfComponent } from './add-contact-rf.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

describe('AddContactRfComponent', () => {
  let component: AddContactRfComponent;
  let fixture: ComponentFixture<AddContactRfComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,ReactiveFormsModule,RouterTestingModule],
      declarations: [AddContactRfComponent]
    });
    fixture = TestBed.createComponent(AddContactRfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
