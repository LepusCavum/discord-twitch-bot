### Automated Tests

1. LoggingExtensions can configure application logging [LoggingExtension_ConfiguresApplicationLogging]
2. Development environment applies application logging defaults  [LoggingExtension_ConfiguresDevelopmentEnvLogging]
3. Production environment applies application logging defaults [LoggingExtension_ConfiguresProductionEnvLogging]
4. StartupService logs startup events through the application's logging infrastructure. [StartupService_LogsStartupInformation]
5. Unhandled startup exceptions produce an error log entry through the application's logging infrastructure. 