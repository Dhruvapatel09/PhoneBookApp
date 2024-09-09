import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditContactRfComponent } from './edit-contact-rf.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { DatePipe } from '@angular/common';

describe('EditContactRfComponent', () => {
  let component: EditContactRfComponent;
  let fixture: ComponentFixture<EditContactRfComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[ReactiveFormsModule,HttpClientTestingModule,RouterTestingModule],
      declarations: [EditContactRfComponent],
    });
    fixture = TestBed.createComponent(EditContactRfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
