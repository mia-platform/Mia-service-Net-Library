# Logging
You can log a message to see in DevOps console. The library uses the [log4net logging system](https://logging.apache.org/log4net/).

There are two types of logging:
 * Request Logging: logs every incoming request in the typical format.
 * Message Logging: logs a message, along with its custom properties if there are any.
 
```
// customObject attributes will be added to the log.
Logger.Trace(HttpContext.Request, "message with object", customObject);
// or
Logger.Trace(HttpContext.Request, "message without object");
```

The logger class supports the main logging levels: *Trace*, *Debug*, *Info*, *Warn*, *Error* and *Fatal*.


By default the library will generate a log for each request, representing both the incoming request and the request completion, logs are created with the *info* level and already provide useful information for later analysis or debugging. If you need more, you can add your logs.

## Example 
```
[HttpPost]
[Route("hello")]
public string LoggingTest([FromBody] Hello hello)
{
    Logger.Trace(HttpContext.Request, "message with object", hello);
    if (hello.Message.Contains(".NET Core"))
    {
        Logger.Info(HttpContext.Request, "foo");
    }
    else
    {
        Logger.Warn(HttpContext.Request, "bar");
    }
    return "Goodbye, world!";
} 
```
For further details about logs can you see the [guidelines for logs](https://docs.mia-platform.eu/development_suite/monitoring-dashboard/dev_ops_guide/log/).