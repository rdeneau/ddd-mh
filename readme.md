# Exercises done during Domain Models in Practise Nov 2020

By [Marco Heimeshoff](https://skillsmatter.com/legacy_profile/marco-heimeshoff) [@Heimeshoff](https://twitter.com/Heimeshoff) for [Skillsmatter](https://skillsmatter.com/courses/737-domain-models-in-practice)

🏷️ architecture event-sourcing domain-driven-design event-driven-design semantic-code domain-modelling

## Mob programming setup

- Language & IDE : F# & VSCode + Ionide (*)
- Test runner: FSCheck-Expecto
- Method of sharing code: vscode live share

> (*) ☝️ **Note** : command shortcut `fsi.SendSelection` is usually <kbd>Alt</kbd>+<kbd>Enter</kbd> but here it's <kbd>Alt</kbd>+<kbd>Insert</kbd>

## Implementing Value Objects

### VO properties

- The low hanging fruit of DDD
- Can be compared with others using property equality - equals() and hashCode()
- Measures, quantifies, or describes a thing in the domain
- Is completely replaceable when the measurement or description changes
- Models a conceptual whole by composing related attributes
- Has Side-Effect-Free Behavior
- Can be maintained immutable

### Scenario: Online Reservation

1. The user selects the day and the time when he/she would like to see the movie.
2. The system lists movies available in the given time interval - title and screening times.
3. The user chooses a particular screening.
4. The system gives information regarding screening room and available seats.
5. The user chooses seats, and gives the name of the person doing the reservation (name and surname).
6. The system gives back the total amount to pay and reservation expiration time.
