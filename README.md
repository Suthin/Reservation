## Scenario

Application has two kinds of users, **providers** and **clients**. Providers have a schedule where they are available to see clients. Clients want to book time, in advance, on that schedule.

**Providers**

- Have an id.
- Have a schedule
    - On Friday the 13th of August, I want to work between 8am and 3pm.

**Clients**

- Have an id.
- Want to reserve a 15m time ‘slot’ from a providers schedule.
    - Reservations expire after 30 mins if not confirmed.
    - Reservations must be made at least 24 hours in advance.
- Want to be able to confirm a reservation.

## Task

Build a API (e.g. RESTful) that:

- Allows providers to submit times they’d like to work on the schedule.
- Allows clients to list available slots.
- Allows clients to reserve an available slot.
- Allows clients to confirm their reservation.

--------------------------------------------------------
## Technology Stack

1. C#.net Web API
2. MS SQL

**How to configure and run the application**

1.  First we have to create new Reservation database with the following tables(Required script kept in Database folder(/Reservation/Reservation/Database/SQLDatabaseScript.sql).
    1.1 Client
    1.2 Provider
    1.3 ProviderSlot
    1.4 ClientSlot

2.  Load Some Clients and Provider seed data in Clients and Provider table (Sample script is kept in Database folder Reservation/Reservation/Database/SampleSeedData.sql)
3.  Update the database connection string in appsettings.json file 

     "ConnectionStrings": {
        "DefaultConnection": "Server=POINT;Database=Reservation;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
      }
4. Now we are good with all configuration now we can run the application. 










