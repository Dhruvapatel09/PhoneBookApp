import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { AuthService } from 'src/app/services/auth.service';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-contact-list-favourites',
  templateUrl: './contact-list-favourites.component.html',
  styleUrls: ['./contact-list-favourites.component.css']
})
export class ContactListFavouritesComponent {
  contacts: any[] | undefined | null;
  contacts1: Contact[] = [];
  contactId: number | undefined;
  loading: boolean = false;
  pageNumber: number = 1;
  pageSize: number = 2;
  totalItems: number = 0;
  totalPages: number = 0;
  uniqueFirstLetters: string[] = [];
  // sortOrder: string = "asc";
  alphabet: string[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split('');
  letter: string  = "";
  constructor(private contactService: ContactService, private router: Router) { }
  sort='asc';
  ngOnInit(): void {
    this.loadFavContactsCount();
    this.loadAllContacts();
  }
  loadFavContactsCount(): void {
    if (this.letter) {
      this.getAllFavContactsCountWithLetter(this.letter);

    } else {
      this.getAllFavContactsCountWithoutLetter();
    }
  }
  getAllFavContactsCountWithLetter(letter: string): void {
    this.contactService.getAllFavContactsCountWithLetter(letter,this.sort).subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          console.log(response.data);
          this.totalItems = response.data;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
          this.getAllFavContactsWithLetter(letter);
        } else {
          console.error('Failed to fetch contacts count', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts count.', error);
        this.loading = false;
      }
    });

  }
  getAllFavContactsCountWithoutLetter(): void {
    this.contactService.getAllFavContactsCount().subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          console.log(response.data);
          this.totalItems = response.data;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
          this.getAllFavContactsWithPagination();
        } else {
          console.error('Failed to fetch contacts count', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts count.', error);
        this.loading = false;
      }
    });

  }
  loadContacts(): void {
    this.loading = true;
    if (this.letter) {
      this.getAllFavContactsWithLetter(this.letter);
    } else {
      this.getAllFavContactsWithPagination();
    }
  }
  loadAllContacts(): void {
    this.loading = true;
    this.contactService.getAllFavContactsWithLetter(this.pageNumber,this.pageSize,this.letter,this.sort).subscribe({
      next: (response: ApiResponse<Contact[]>) => {
        if (response.success) {
          console.log(response.data);
          this.contacts1 = response.data;
          this.updateUniqueFirstLetters();
        } else {
          console.error('Failed to fetch contacts', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts.', error);
        this.loading = false;
      }
    });
  }
  getUniqueFirstLetters(): string[] {
    // Extract first letters from contact names and filter unique letters
    const firstLetters = Array.from(new Set(this.contacts1.map(contact => contact.firstName.charAt(0).toUpperCase())));
    return firstLetters.sort(); // Sort alphabetically
}
  updateUniqueFirstLetters(): void {
    this.uniqueFirstLetters = this.getUniqueFirstLetters();
}
  getAllFavContactsWithLetter(letter: string): void {
    this.loading = true;
    this.contactService.getAllFavContactsWithLetter(this.pageNumber, this.pageSize, letter,this.sort).subscribe({
      next: (response: ApiResponse<Contact[]>) => {
        if (response.success) {
          console.log(response.data);
          this.contacts = response.data;
        } else {
          console.error('Failed to fetch contacts', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts.', error);
        this.loading = false;
      }
    });
  }
  getAllFavContactsWithPagination(): void {
    if (this.pageNumber > this.totalPages) {
      console.log('Requested page does not exist.');
      return;
    }

    this.contactService.getAllFavContactsWithoutLetter(this.pageNumber, this.pageSize,this.sort).subscribe({
      next: (response: ApiResponse<Contact[]>) => {
        if (response.success) {
          console.log(response.data);
          this.contacts = response.data;
        } else {
          console.error('Failed to fetch contacts', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts.', error);
        this.loading = false;
      }
    });
  }

  filterByLetter(letter: string ): void {
    this.letter = letter;
    this.pageNumber = 1;
    this.loadFavContactsCount();
  }
  changePageSize(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageNumber = 1;
    this.totalPages = Math.ceil(this.totalItems / this.pageSize);
    this.loadContacts();
  }

  changePage(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.loadContacts();
  }

  getContactImage(contact: Contact): string {
    if (contact.imageByte) {
      return 'data:image/jpeg;base64,' + contact.imageByte;
    } else {
      return 'assets/DefaultImage.jpg'; // Path to your default image
    }
  }

  confirmDelete(id: number): void {
    if (confirm('Are you sure?')) {
      this.contactId = id;
      this.deleteContact();
    }
  }

  deleteContact(): void {
    this.contactService.deleteContactById(this.contactId).subscribe({
      next: (response) => {
        if (response.success) {
          // Decrement totalItems by 1 after successful deletion
          this.totalItems--;
          // Recalculate totalPages based on updated totalItems
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
          if (this.pageNumber > this.totalPages) {
            this.pageNumber = this.totalPages;
          }
          this.loadContacts();
          this.loadAllContacts();
        } else {
          alert(response.message);
        }
      },
      error: (err) => {
        alert(err.error.message);
      },
      complete: () => {
        console.log('completed');
      }
    })
    this.router.navigate(['/contacts-favourites']);
  }
  sortAsc()
  {
    this.sort = 'asc'
    this.pageNumber = 1;
    this.loadContacts();
  }
 
  sortDesc()
  {
    this.sort = 'desc'
    this.pageNumber = 1;
    this.loadContacts();
 
  }
}
