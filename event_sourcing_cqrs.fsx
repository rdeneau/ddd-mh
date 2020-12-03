type Seat = Seat of string

type Screening = Screening of string

type Command =
    | ReserveSeats of Seat list * Screening

type Query =
    | AvailableSeatsForScreening of Screening

type WriteEvent =
    | SeatsReserved of Seat list

type ReadEvent =
    // | ReservationFailed
    // | NoSeatFoundInReservation
    | AvailableSeatsForScreeningAreGiven of Seat list

type ReservationSystem =
    ReservationSystem of Seat list
    with
        static member AllSeats =
            [Seat "1"; Seat "2"; Seat "3"]
            |> ReservationSystem

let (ReservationSystem state') = ReservationSystem.AllSeats
let mutable domainModel = state'
let mutable readModel = state'

let build_state events =
    (ReservationSystem.AllSeats,events)
    ||> List.fold (
        fun (ReservationSystem state) event ->
            match event with
            | SeatsReserved seats -> state |> List.filter (fun seat -> if seats |> List.contains (seat) then false else true) |> ReservationSystem
            // | ReservationFailed -> state
            // | NoSeatFoundInReservation -> { state with seats = [] }
        )


// Tests
let Given (events: WriteEvent list) = events

let Query (query:Query) (events: ReadEvent list) : string list =

    match query with
    | AvailableSeatsForScreening screening ->
        // TODO: handle later when screening is unknown
        readModel |> List.map (fun (Seat seat) -> seat)

let When (command: Command) (events: WriteEvent list) : ReadEvent list =
    match command with
    | ReserveSeats (seats, screening) ->
        domainModel <- build_state events |>  fun (ReservationSystem s) -> s
        readModel <- domainModel
        (AvailableSeatsForScreeningAreGiven seats) |> List.singleton

let Then (expected) (actual) =
    actual = expected

// let ``show the available seats for a screening`` =
//     []
//     |> Given
//     |> Query (AvailableSeatsForScreening (Screening "test"))
//     |> Then [AvailableSeatsForScreeningAreGiven (ReservationSystem.Example
//                                                 |> fun (ReservationSystem r) -> r |> Map.toList |> List.map snd |> List.head)]

let ``make reservation with seats should succeed`` =
    []
    |> Given
    |> When (ReserveSeats ([Seat "seat_1"], Screening "test"))
    |> Then ([AvailableSeatsForScreeningAreGiven [Seat "seat_1"]])

let ``command and query test`` =
    []
    |> Given
    |> When (ReserveSeats ([Seat "1"; Seat "2"], Screening "test"))
    |> Query (AvailableSeatsForScreening (Screening "test"))
    |> Then ["3"]
