# Logging
You can log a message to see in DevOps console. The library uses the [log4net logging system](https://logging.apache.org/log4net/).

In order to log messages you have to call this method.
`LoggingUtility.LogMessage(HttpRequest req, LogLevels logLevel, object userObject, string message)`

Each `logLevel` level produces a log with the homonym level.

By default the library will generate a log for each request, representing both the incoming request and the request completion, logs are created with the *info* level and already provide useful information for later analysis or debugging. If you need more, you can add your logs.

## Example 
```
[HttpPost]
[Route("hello")]
public string LoggingTest([FromBody] Hello hello)
{
    // Log at 'Trace' level with custom object 'hello'
    LoggingUtility.LogMessage(HttpContext.Request, LogLevels.Trace, hello, "custom message");
    if (hello.Message.Contains(".NET Core"))
    {
        // Log at 'Info' level
        LoggingUtility.LogMessage(HttpContext.Request, LogLevels.Info, null, "custom message 2");
    }
    else
    {
        // Log at 'Warn' level with custom object 'hello'
        LoggingUtility.LogMessage(HttpContext.Request, LogLevels.Warn, null, "custom message 3");
    }
    return "Goodbye, world";
}
```
For further details about logs can you see the [guidelines for logs](https://docs.mia-platform.eu/development_suite/monitoring-dashboard/dev_ops_guide/log/).