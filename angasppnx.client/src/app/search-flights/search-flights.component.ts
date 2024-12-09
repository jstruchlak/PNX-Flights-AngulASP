import { Component, OnInit } from '@angular/core';
import { FlightService } from '../api/services';
import { FlightRm } from '../api/models';
import { FormBuilder } from '@angular/forms'

@Component({
  selector: 'app-search-flights',
  templateUrl: './search-flights.component.html',
  styleUrls: ['./search-flights.component.css']
})
export class SearchFlightsComponent implements OnInit {


  searchResult: FlightRm[] = []

  constructor(private flightService: FlightService,
    private fb: FormBuilder) { }

  searchForm = this.fb.group({
    from: [''],
    destination: [''],
    fromDate: [''],
    toDate: [''],
    numberOfPassengers: [1]
  })

  ngOnInit(): void {
    this.search();
  }

  search() {
    const searchParams = {
      from: this.searchForm.value.from ?? undefined,
      destination: this.searchForm.value.destination ?? undefined,
      fromDate: this.searchForm.value.fromDate ?? undefined,
      toDate: this.searchForm.value.toDate ?? undefined,
      numberOfPassengers: this.searchForm.value.numberOfPassengers ?? undefined
    };

    this.flightService.searchFlight(searchParams)
      .subscribe(response => this.searchResult = response, this.handleError);
  }


  private handleError(err: any) {
    console.log("Response Error. Status: ", err.status)
    console.log("Response Error. Status Text: ", err.statusText)
    console.log(err)
  }

}
