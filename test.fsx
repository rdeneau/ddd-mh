type WeekDay =
    | Monday
    | Tuesday
    | Wednessday
    | Thursday
    | Friday
    | Saturday
    | Sunday

type Hour = Hour of int
type Minute = Minute of int
type Second = Second of int

type Time = Time of Hour * Minute * Second

type DayAndTime = DayAndTime of WeekDay * Time

type TimeInterval = TimeInterval of DayAndTime * DayAndTime

type MovieTitle = MovieTitle of string

type ScreeningTime =
    {
        startTime: Time
        endTime: Time
    }

type AvailableMovies = TimeInterval -> MovieTitle * ScreeningTime list

type ReservationSystem = unit
type ChooseParticularScreening = MovieTitle * ScreeningTime -> ReservationSystem

type ScreeningRoom = ScreeningRoom of string
type Seat = Seat of string

type ScreeningRoomAndSeatAvailability = ReservationSystem -> (ScreeningRoom * Seat list) list
