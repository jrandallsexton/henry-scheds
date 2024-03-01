# henry-scheds for henry-meds

Small API for facilitating client-provider reservations (appointments)

## Overview

- A _provider_ can submit their schedule which generates a collection of _slots_ within the system
    - Each _slot_ is 15 minutes in duration
- A _client_ can obtain a list of available slots
    - A _slot_ can be reserved thereby generating an _appointment_ within the system
    - An _appointment_ will auto-expire if not confirmed within 15 minutes of creation
    - An _appointment_ cannot be created for a slot within 24 hours of the _slot_'s specified time
## Decisions
- Use of [MediatR](https://github.com/jbogard/MediatR) and [CQRS](https://martinfowler.com/bliki/CQRS.html) pattern for handling API actions
- SQL Server for persistence
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/) for ensuring integrity of commands and queries
- [Automapper](https://automapper.org/) for simple mapping of entities to DTOs
- [MediatR](https://github.com/jbogard/MediatR) handlers use dbContext directly instead of a repository (time constraints)
- [Hangfire](https://www.hangfire.io/) for the cron job that expires reservations not confirmed within 30 minutes
    - Cron job fires every minute
    - Reservations not confirmed within 30 minutes are passed off via fire-and-forget to another MediatR handler for expiry process
    - Should something happen with the handler, the cron job would send it for expiry the following minute
    - Automatically retries jobs (configurable)
    - Dashboard is available at [_/dashboard_](https://localhost:63632/dashboard)
- Unit Testing
    - [XUnit](https://xunit.net/) for framework
    - [FluentAssertions](https://fluentassertions.com/) for test assertions
    - [SQL Server In-Memory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/) for dbContext setups
## Missing
- Log statements are constructed for [structured logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0), therefore you will see warnings about formatting
- Structured logging provider not implemented
- Integration testing which would use [TestContainers](https://testcontainers.com/) to spin up the environment within Docker and perform actual HTTP requests against endpoints
- [Transactional Outbox Pattern](https://microservices.io/patterns/data/transactional-outbox.html) for integration events (generally via some sort of event bus)
- No repositories exist; direct usage of dbContext within MediatR handlers
- No authentication or authorization
- Unit testing is minimal; only a couple of classes implemented to show general testing setups (TDD not utilized)

## Running the Service
- Docker Compose file is provided; set the startup project to it

## Closing Thoughts
- Many of the classes within the Application namespace are empty - placed there to show more about my thought process and how the pattern I chose would look over-time
- As of recent, I have really begun to rethink the usage of the ExceptionHandlingMiddleware and want to change it - but this is a tried & true pattern that I know works. Drawback?  Exceptions are expensive and there are better ways of returning the correct HTTP result to the API client
- Instead of _Provider_ and _Client_ entities, it really should have been more _User-centric_ and allowed the application to obtain required IDs for commands/queries to be determined via HTTP Context
- Exercise was stated to be completed within 2-3 hours; this was done in roughly 4-5 hours
- Most of the code (except for _domain-specific_ items) was recycled from previous projects

## Diagram
- Diagram created using [Mermaid](https://mermaid.js.org/)