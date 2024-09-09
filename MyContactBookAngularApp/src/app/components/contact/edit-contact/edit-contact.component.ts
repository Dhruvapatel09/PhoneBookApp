import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-edit-contact',
  templateUrl: './edit-contact.component.html',
  styleUrls: ['./edit-contact.component.css']
})
export class EditContactComponent {
  phoneId: number | undefined;
  imageUrl: string | ArrayBuffer | null = null;
  @ViewChild('imageInput') imageInput!: ElementRef;
  contact: Contact = {
    phoneId: 0,
    firstName: '',
    email: '',
    phoneNumber: '',
    lastName: '',
    company: '',
    image: '',
    imageByte: '',
    stateId: 0,
    countryId: 0,
    gender: '',
    birthdate:'',
    favourites: false,
    country: {
      countryId: 0,
      countryName: ''
    },
    state: {
      stateId: 0,
      stateName: '',
      countryId: 0
    }
  };
  countries: Country[] = [];
  states: State[] = [];
  countrySelected: boolean = false;
  imageByte: string = '';
  loading: boolean = false;

  constructor(
    private countryService: CountryService,
    private contactService: ContactService,
    private stateService: StateService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.phoneId = params['phoneId'];
      this.loadContact(this.phoneId);
      this.loadCountries();
    });
  }

  loadCountries(): void {
    this.countryService.getAllCountry().subscribe({
      next: (response: ApiResponse<Country[]>) => {
        if (response.success) {
          this.countries = response.data;
        } else {
          console.error('Failed to fetch countries', response.message);
        }
      },
      error: (error) => {
        console.error('Error fetching countries:', error);
      }
    });
  }

  onSelectCountry(countryId: number): void {
    this.states = [];
    if (this.countrySelected) {
      // Only reset the state if the country has been initially set
      this.contact.stateId = 0; // Reset the stateId to 0 or any default value indicating no state selected
    } else {
      this.countrySelected = true; // Set the flag to true after the initial country selection
    }
    this.stateService.fetchStatetByCountryId(countryId).subscribe({
      next: (response: ApiResponse<State[]>) => {
        if (response.success) {
          this.states = response.data;
        } else {
          console.error('Failed to fetch states', response.message);
        }
      },
      error: (error) => {
        console.error('Error fetching states:', error);
      }
    });
  }

  loadContact(phoneId: number | undefined): void {
    
    this.contactService.getContactById(phoneId).subscribe({
      next: (response) => {
        if (response.success) {
          this.contact = response.data;
          this.contact.birthdate = this.formatDate(new Date(this.contact.birthdate));
          this.onSelectCountry(this.contact.countryId); // Load states for the country
          this.imageUrl = 'data:image/jpeg;base64,' + this.contact.imageByte;
          this.imageByte = this.contact.imageByte;
          this.imageInput.nativeElement.value = this.contact.image;
        } else {
          console.error("Failed to fetch contact", response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        alert(err.error.message);
        this.loading = false;
      }, 
      complete:() =>{
        this.loading = false;
        console.log('completed');
      }

    });
  }

  onSubmit(myForm: NgForm): void {
    if (myForm.valid) {
      this.loading = true;
      console.log(myForm.value);
      if (this.imageUrl === null) {
        // If file has been removed, clear the imageByte and fileName in the contact object
        this.contact.imageByte = '';
        this.contact.image = '';
      }
      this.contactService.editContact(this.contact).subscribe({
        next: (response) => {
          if (response.success) {
            console.log('Contact updated successfully:', response);
            this.router.navigate(['/contacts-pagination']);
            myForm.resetForm();
          } else {
            alert(response.message);
          }
          this.loading = false;
        },
        error: (err) => {
          console.error(err.error.message);
          this.loading = false;
          alert(err.error.message);
        },
        complete:() =>{
          this.loading = false;
          console.log('completed');
        }
 
      });
    }
  }
  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const fileType = file.type;
      if (fileType === 'image/jpeg' || fileType === 'image/png' || fileType === 'image/jpg') {
        const reader = new FileReader();
        reader.onload = () => {
          this.contact.imageByte = (reader.result as string).split(',')[1];
          this.contact.image = file.name;
          this.imageUrl = reader.result;
        };

        reader.readAsDataURL(file);
      } else {
        this.imageUrl = null;
        this.contact.imageByte = '';
        this.contact.image = '';
        this.imageInput.nativeElement.value = '';
        // Alert user about invalid file format
        alert('Invalid file format! Please upload an image in JPG, JPEG, or PNG format.');

      }
    }
  }

  removeFile() {
    this.imageUrl = null;
    this.contact.imageByte = '';
    this.contact.image = '';
    this.imageInput.nativeElement.value = '';
  }
  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }
}