import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FlightService } from '../api/services/flight.service';
import { FlightRm } from '../api/models/flight-rm';
import { AuthService } from '../auth/auth.service';
import { FormBuilder } from '@angular/forms';
import { BookDto } from '../api/models/book-dto';

@Component({
  selector: 'app-book-flight',
  templateUrl: './book-flight.component.html',
  styleUrls: ['./book-flight.component.css'],
})
export class BookFlightComponent implements OnInit {
  flightId: string = 'not loaded';
  flight: FlightRm = {};
  form = this.fb.group({
    number: [1],
  });

  constructor(
    private route: ActivatedRoute,
    private flightService: FlightService,
    private router: Router,
    private authService: AuthService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    if (!this.authService.currentUser) {
      this.router.navigate(['/register-passenger']);
      return;
    }

    this.route.paramMap.subscribe((p) => {
      const flightId = p.get('flightId');
      if (!flightId) {
        console.error('Flight ID is missing.');
        this.router.navigate(['/search-flights']);
        return;
      }
      this.findFlight(flightId);
    });
  }

  private findFlight(flightId: string): void {
    this.flightId = flightId;

    this.flightService.findFlight({ id: this.flightId }).subscribe(
      (flight) => (this.flight = flight),
      (err) => this.handleError(err)
    );
  }

  private handleError(err: any): void {
    if (err.status === 404) {
      alert('Flight Not Found');
      this.router.navigate(['/search-flights']);
    }

    console.error('Response Error:', {
      status: err.status,
      statusText: err.statusText,
      error: err,
    });
  }

  book(): void {
    const numberOfPassengers = this.form.get('number')?.value;

    if (!numberOfPassengers || typeof numberOfPassengers !== 'number') {
      console.error('Invalid number of passengers.');
      return;
    }

    console.log(
      `Booking ${numberOfPassengers} passengers for the flight ${this.flightId}`
    );

    const booking: BookDto = {
      flightId: this.flight.id,
      passengerEmail: this.authService.currentUser?.email,
      numberOfSeats: this.form.get('number')?.value ?? undefined
    }

    this.flightService.bookFlight({ body: booking })
      .subscribe(_ => this.router.navigate(['/my-booking']),
        this.handleError);
  }
}
