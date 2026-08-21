## Tasks

1. Create the strongly typed ApplicationOptions class. 
2. Bind ApplicationOptions from IConfiguration. 
3. Move configuration registration into AddBotServices(). 
4. Inject IOptions<ApplicationOptions> into StartupService.
5. Replace direct IConfiguration usage with IOptions<ApplicationOptions>.  
6. Define and implement validation rules for required ApplicationOptions values.
7. Add configuration validation logging.

---

## Test Plan

### Automated Tests

1. ApplicationOptions binds successfully from configuration.
- provide an in-memory configuration containing the Application section, register the options, resolve the options, and assert that the resulting ApplicationOptions contains the expected value. [ApplicationOptions_BindsNameFromConfiguration]
2. StartupService can be resolved through DI with IOptions<ApplicationOptions> registered. [StartupService_ShouldResolve_WithApplicationOptions]
- verify that the configured value actually reaches StartupService, rather than merely proving construction succeeds.
3. Options validation executes during application startup rather than first usage. 
- The test needs to establish: configuration is invalid, host is configured with options validation, host startup is attempted, startup fails because of options validation, validation happens without StartupService reaching normal execution [Host_StartFails_WhenAppNameIsEmpty],[Host_StartFails_WhenAppNameIsMissing]
4. The existing Application:Name configuration binds to ApplicationOptions.Name.
5. Valid ApplicationOptions pass validation.
- test should test the validation rule itself, rather than host startup.
6. Missing or empty required ApplicationOptions values fail validation.
- test should test the validation rule itself, rather than host startup. test should not merely assert that "something throws." We want to know that the Options validation mechanism identifies the invalid value.
- ApplicationOptions.Name must be provided and must not be null, empty, or whitespace.
7. Configuration validation failures produce an error-level log containing diagnostic information about the invalid configuration.
- We need to establish at least: an invalid configuration causes a validation failure, a log entry is produced, the log has an appropriate severity, the log identifies the configuration failure