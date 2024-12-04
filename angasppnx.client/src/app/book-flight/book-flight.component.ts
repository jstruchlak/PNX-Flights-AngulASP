import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FlightService } from '../api/services/flight.service';
import { FlightRm } from '../api/models/flight-rm';
import { AuthService } from '../auth/auth.service';
import { FormBuilder, Validators } from '@angular/forms';
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
    number: [1, Validators.compose([Validators.required, Validators.min(1), Validators.max(254)])],
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

  private handleError = (err: any) => {

    if (err.status == 404) {
      alert("Flight not found!")
      this.router.navigate(['/search-flights'])
    }

    console.log("Response Error. Status: ", err.status)
    console.log("Response Error. Status Text: ", err.statusText)
    console.log(err)
  }

  book() {

    if (this.form.invalid)
      return;


    console.log(`Booking ${this.form.get('number')?.value} passengers for the flight: ${this.flight.id}`)

    const booking: BookDto = {
      flightId: this.flight.id,
      passengerEmail: this.authService.currentUser?.email,
      numberOfSeats: this.form.get('number')?.value ?? undefined

    }

    this.flightService.bookFlight({ body: booking })
      .subscribe({
        next: () => this.router.navigate(['/my-booking']),
        error: err => {
          console.error('Booking error:', err);
          this.handleError(err); // Ensure this.handleError is implemented to show user-friendly messages.
        }
      });


  }

  get number() {
    return this.form.controls.number
  }

}
