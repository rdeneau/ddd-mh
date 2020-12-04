type Seat = Seat of string
type ReservationStatus =
    |Reserved
    |Available

type Screening = Screening of string

type Command =
    | ReserveSeats of
        {|
            Title: string
            Seats: Seat list
            CustomerId: string
            ScreeningId: string
        |}
    | CustomerPays

type Query =
    | AvailableMovies 
    // of
    //     {|
    //         ScreeningId: string
    //         Title: string
    //         Seats: Seat list
    //     |} list
    | ScreeningInformation
    // of
    //     {|
    //         ScreeningId: string
    //         Title: string
    //         Seats: Seat list
    //     |}
    | Price of string
    | Ticket

type Event =
    | ReservationSystemOpened
    | SeatsReserved of Map<Seat,ReservationStatus>
    | ReservationFailed
    | ReservationCompleted

type EventStore = Event list

type ReservationSystem =
    ReservationSystem of Map<Seat,ReservationStatus>
    with
        static member AllSeats =
            [Seat "1",Available; Seat "2",Available; Seat "3",Available]
            |> Map.ofList
            |> ReservationSystem

let mutable readModelMovies: string list = []

let mutable eventStore: EventStore = []

let build_state events =
    (ReservationSystem Map.empty, events)
    ||> List.fold (
        fun (ReservationSystem state) event ->
            match event with
            | ReservationSystemOpened ->
                do
                    readModelMovies <- ReservationSystem.AllSeats |> fun (ReservationSystem r) -> r |> Map.toList |> List.map (fun (Seat s,_) -> s)
                ReservationSystem.AllSeats
            | SeatsReserved seats -> (state |> Map.toList)@(seats |> Map.toList) |> Map.ofList |> ReservationSystem 
            | ReservationCompleted -> ReservationSystem state
            | ReservationFailed -> ReservationSystem state
        )

let command_handler command =
    match command with
    | ReserveSeats info ->
        let state = build_state eventStore |>  fun (ReservationSystem s) -> s
        let seats = info.Seats
        seats
        |> List.choose state.TryFind
        |> List.forall ((=) Available)
        |> function
            |true -> 
                do  
                    eventStore <- SeatsReserved (seats |> List.map (fun seat -> seat,Reserved) |> Map.ofList)::eventStore
                SeatsReserved (seats |> List.map (fun seat -> seat,Reserved) |> Map.ofList)
            |false -> 
                do  
                    eventStore <- ReservationFailed::eventStore
                ReservationFailed
        // |> List.filter (fun seat -> if seats |> List.contains (seat) then false else true)
        // readModel <- domainModel
        // (AvailableSeatsForScreeningAreGiven seats) |> List.singleton
    | CustomerPays -> 
        do  
            eventStore <- ReservationCompleted::eventStore
        ReservationCompleted

let query_handler (query: Query) =
    match query with
    | Ticket -> "Ticket"
    | AvailableMovies -> readModelMovies |> String.concat ";"
    | Price _ -> "10€"
    | ScreeningInformation -> readModelMovies |> String.concat ";"

command_handler CustomerPays
command_handler (ReserveSeats {|CustomerId="";ScreeningId="";Seats=[Seat "1"];Title = "lalala"|})
eventStore
readModelMovies
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

// let ``make reservation with seats should succeed`` =
//     []
//     |> Given
//     |> When (ReserveSeats ([Seat "seat_1"], Screening "test"))
//     |> Then ([AvailableSeatsForScreeningAreGiven [Seat "seat_1"]])

let ``reserve seat gives price and expiration`` =
    []
    |> Given
    |> When (ReserveSeats ([Seat "1"; Seat "2"], Screening "test"))
    |> Query (AvailableSeatsForScreening (Screening "test"))
    |> Then ["3"]

let ``customer pays and get its ticket`` = "todo"
