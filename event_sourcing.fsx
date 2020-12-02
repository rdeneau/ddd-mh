// A Customer reserves specific seats at a specific screening (for simplicity, assume there exists only one screening for the time beeing).
// If available, the seats should be reserved.

type Seat = Seat of string

type Screening = Screening of string

type Command =
    | ReserveSeats of Seat list * Screening

type Event =
    | SeatsReserved of Seat list
    | ReservationFailed
    | NoSeatFoundInReservation

type Reservation =
    {
        seats: Seat list
        screening: Screening
    }
    with
        static member Empty = { seats = []; screening = Screening "empty" }

let check_seat_availability (seats: Seat list) =
    true

let make_reservation (screening: Screening) (seats: Seat list) : Reservation option =
    seats
    |> check_seat_availability
    |> function
        | true -> Some { seats = seats; screening = screening }
        | false -> None

// Tests
let Given (startState: Reservation) (events: Event list) : Reservation =
    (startState,events)
    ||> List.fold (
        fun state event ->
            match event with
            | SeatsReserved seats -> { state with seats = seats }
            | ReservationFailed -> state
            | NoSeatFoundInReservation -> { state with seats = [] }
        )

let When (command: Command) (state: Reservation) : Event list =
    match command with
    | ReserveSeats (seats, screening) ->
        if state = Reservation.Empty then
            make_reservation screening seats
            |> function
                | Some reservation -> (SeatsReserved seats) |> List.singleton
                | None -> ReservationFailed |> List.singleton
        else
            ReservationFailed |> List.singleton

let Then (expected: Event list) (actual: Event list) =
    actual = expected

let ``make reservation with seats should succeed`` =
    []
    |> Given Reservation.Empty
    |> When (ReserveSeats ([Seat "seat_1"], Screening "test"))
    |> Then [SeatsReserved [Seat "seat_1"]]
