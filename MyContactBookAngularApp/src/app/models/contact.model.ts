import { Country } from "./country.model";
import { State } from "./state.model";

export interface Contact{
    phoneId:number;
    firstName:string;
    lastName:string;
    email:string;
    phoneNumber:string;
    company:string;
    image:string;
    imageByte:string,
    gender:string;
    favourites:boolean;
    countryId:number;
    stateId:number;
    birthdate:string;
    country: Country,
    state: State
}