import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactListFavouritesComponent } from './contact-list-favourites.component';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

describe('ContactListFavouritesComponent', () => {
  let component: ContactListFavouritesComponent;
  let fixture: ComponentFixture<ContactListFavouritesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,FormsModule,RouterTestingModule],
      declarations: [ContactListFavouritesComponent]
    });
    fixture = TestBed.createComponent(ContactListFavouritesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
