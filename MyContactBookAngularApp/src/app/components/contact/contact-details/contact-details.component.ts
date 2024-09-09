import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Contact } from 'src/app/models/contact.model';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-contact-details',
  templateUrl: './contact-details.component.html',
  styleUrls: ['./contact-details.component.css']
})
export class ContactDetailsComponent {
  phoneId:number|undefined;
  contact:Contact={
    phoneId:0,
    firstName:'',
    lastName:'',
    phoneNumber:'',
    company:'',
    email:'',
    countryId:0,
    stateId:0,
    image:'',
    imageByte:'',
    favourites:true,
    gender:'',
    birthdate:'',
    country:{
      countryId:0,
      countryName:''
    },
    state:{
      stateId:0,
      stateName:'',
      countryId:0,
    }
  };
  constructor(private categoryService:ContactService,private route:ActivatedRoute){}
  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.phoneId=params['phoneId'];
      this.loadContactDetails(this.phoneId);
    });
    
  }
  
  loadContactDetails(phoneId:number|undefined):void{
  this.categoryService.getContactById(phoneId).subscribe({
    next:(response)=>{
      if(response.success){
        this.contact=response.data;
        console.log("category",this.contact);
        console.log(response.data);
      }else{
        console.error('Failed to fetch Contact: ',response.message)
      }
    },
    error:(err)=>{
      alert(err.error.message);
    },
    complete:()=>{
      console.log('Completed');
    }
  })
  }
  }
  