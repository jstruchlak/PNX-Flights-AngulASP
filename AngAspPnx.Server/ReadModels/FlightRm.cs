﻿namespace AngAspPnx.Server.ReadModels
{
    public record FlightRm(
        Guid Id,
        string Airline,
        string Price,
        TimePlaceRm Departure,
        TimePlaceRm Arrival,
        int RemaingNumberOfSeats
        );
}
