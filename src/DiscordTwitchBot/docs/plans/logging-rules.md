# Logging Rules

## Rules

1. Logs represent meaningful events and state changes, not routine execution traces.
2. All application logs use structured properties rather than interpolated message text.
3. Never log tokens, credentials, authorization headers, connection strings, raw secrets, or complete external payloads.
4. Exceptions are included with the exception logging overload when an exception is available.
5. External integrations log connection state transitions, authentication failures, reconnect attempts, and terminal failures.
6. Recovery logs include the affected integration, reason, attempt number when available, and the next action.
7. Health and status logs are emitted only when the state changes or when a failure requires attention.
8. Message-level logs use a correlation identifier and do not include message content by default.
9. Repeated reconnect and failure events must not create an unbounded log storm. Add rate limiting or aggregate counts when the retry behavior is implemented.


## Category Filters

Baseline filters:

| Environment | Minimum application level | Microsoft | System | Third-party libraries |
| --- | --- | --- | --- | --- |
| Development | `Debug` | `Warning` | `Warning` | `Warning` |
| Production | `Information` | `Warning` | `Warning` | `Warning` |

## Event Contract

Every recurring event type should define a stable event name or `EventId` before implementation. The following properties are required where applicable:

| Event | Level | Required properties |
| --- | --- | --- |
| Application started | `Information` | `ApplicationName`, `Version`, `Environment` |
| Application stopping/stopped | `Information` | `ApplicationName`, `Environment` |
| Configuration accepted | `Information` | `ConfigurationSections`, with secret values excluded |
| Configuration rejected | `Critical` | `ConfigurationSections`, `FailureReason` |
| Integration connected | `Information` | `Integration`, `Endpoint` without secrets |
| Integration disconnected | `Warning` | `Integration`, `Reason`, `WillRetry` |
| Integration reconnect scheduled | `Warning` | `Integration`, `Attempt`, `Delay`, `Reason` |
| Integration authentication failed | `Error` | `Integration`, `FailureReason` |
| Message received | `Debug` | `MessageId`, `Integration`, `ReceivedAt` |
| Message filtered | `Debug` | `MessageId`, `FilterReason` |
| Relay delivery succeeded | `Information` | `MessageId`, `Destination`, `DeliveredAt` |
| Relay delivery failed | `Error` | `MessageId`, `Destination`, `Attempt`, `WillRetry`, `FailureReason` |

`MessageId` should be generated or preserved at the application boundary and carried through filtering, formatting, queueing, and delivery. Do not use message text as the correlation identifier.

## Implementation Checklist

- [ ] Correct the global minimum-level behavior so Development can emit `Debug` logs.
- [ ] Use named `EventId` values or a documented stable event-name convention.
- [ ] Add structured properties to startup and shutdown logs.
- [ ] Add integration lifecycle and recovery logs when adapters are implemented.
- [ ] Add message correlation at the Twitch-to-relay boundary.
- [ ] Add tests that verify effective filtering, not only registered filter rules.
- [ ] Verify secrets and raw payloads are excluded from logs.
- [ ] Revisit retry log volume after reconnect behavior is implemented.