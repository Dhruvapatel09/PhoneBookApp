import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactListPaginationComponent } from './contact-list-pagination.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';

describe('ContactListPaginationComponent', () => {
  let component: ContactListPaginationComponent;
  let fixture: ComponentFixture<ContactListPaginationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
    imports: [RouterTestingModule,HttpClientTestingModule,FormsModule],
    declarations: [ContactListPaginationComponent]
    });
    fixture = TestBed.createComponent(ContactListPaginationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
