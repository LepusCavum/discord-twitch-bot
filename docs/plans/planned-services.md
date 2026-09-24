#Planned Services

| Service | Lifetime | Why |
| -------- | -------- | -------- |
| StartupService | Singleton | One instance for the application's lifetime |
| Discord Client | Singleton | Single gateway connection |
| Twitch Client | Singleton | Single IRC/EventSub connection |
| Message Formatter | Singleton or Transient? | Stateless |
| Message Filter | Singleton or Transient? | Stateless |
| Relay Service | Singleton | Coordinates long-lived services |
| Configuration | Singleton | Managed by the host |
| Logger | Built-in | Managed by .NET |