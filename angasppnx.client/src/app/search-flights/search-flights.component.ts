import { Component, OnInit } from '@angular/core';
@Component({
  selector: 'app-search-flights',
  templateUrl: './search-flights.component.html',
  styleUrls: ['./search-flights.component.css']
})
export class SearchFlightsComponent implements OnInit {

  searchResult: FlightRm[] = [
    {
      airline: "ASP Airlines",
      remainingNumberOfSeats: 50,
      departure: { time: Date.now().toString(), place: "Adelaide" },
      arrival: { time: Date.now().toString(), place: "Melbourne" },
      price: "350",
    },
    {
      airline: "Angular-Qatar",
      remainingNumberOfSeats: 140,
      departure: { time: Date.now().toString(), place: "Dubai" },
      arrival: { time: Date.now().toString(), place: "Adelaide" },
      price: "800",
    },
    {
      airline: "PNX Airways",
      remainingNumberOfSeats: 6,
      departure: { time: Date.now().toString(), place: "North Adelaide" },
      arrival: { time: Date.now().toString(), place: "Richmond" },
      price: "65",
    },
  ]




  constructor() { }
  ngOnInit(): void {
  }
}

export interface FlightRm {
  airline: string;
  arrival: TimePlaceRm;
  departure: TimePlaceRm;
  price: string;
  remainingNumberOfSeats: number;
}

export interface TimePlaceRm {
  place: string,
  time: string
}
