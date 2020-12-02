// A Customer reserves specific seats at a specific screening (for simplicity, assume there exists only one screening for the time beeing).
// If available, the seats should be reserved.

type Seat = Seat of string

type Screening = Screening of string

type Command =
    | ReserveSeat of Seat list * Screening

type Event =
    | SeatReserved
    | ReservationFailed
    | NoSeatFoundInReservation

// given command , we get given reponse // command ReserveSeat, logic for reservation => result SeatReserved

let check_seat_availability (seats: Seat list) =
    match seats with
    | [] -> NoSeatFoundInReservation
    | _  -> SeatReserved

let make_reservation (command: Command) : Event =
    match command with
    | ReserveSeat (seats, _) ->
        seats |> check_seat_availability

// Tests
let ``make reservation with seats should succeed`` =
    ReserveSeat ([Seat "seat_1"], Screening "test")
    |> make_reservation
    |> fun event ->
        match event with
        | SeatReserved -> "ok"
        | NoSeatFoundInReservation -> "fail"
        | ReservationFailed -> "fail"

let ``make reservation without seats should fail`` =
    ReserveSeat ([], Screening "test")
    |> make_reservation
    |> fun event ->
        match event with
        | SeatReserved -> "fail"
        | NoSeatFoundInReservation -> "ok"
        | ReservationFailed -> "fail"
