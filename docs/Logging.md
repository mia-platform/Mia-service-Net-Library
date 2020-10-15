# Logging
You can log a message to see in DevOps console. The library uses the [log4net logging system](https://logging.apache.org/log4net/).

There are two types of logging:

 * Request Logging: logs every request related message (incoming, completed and custom ones) with the standard Mia-Platform logging format.
 * Message Logging: logs a message, along with its custom properties if there are any.

```
// customObject attributes will be added to the log:
Logger.Trace(HttpContext.Request, "message with object", customObject);
// alternatively, without custom attributes:
Logger.Trace(HttpContext.Request, "message without object");
// alternatively, if it is not related to a specific request:
Logger.Trace("message without object and reqId");
```

The logger class supports the main logging levels: *Trace*, *Debug*, *Info*, *Warn*, *Error* and *Fatal*.

The library will generate two logs for each request, representing the incoming request and the request completion. Incoming request logs with the *Trace* level, completed request with the *Info* level. Both these logs provide useful information for later analysis or debugging.  
To enable this logging functionality you should add the following line in the `Configure` function of `Startup.cs` file of your microservice:

```csharp
app.UseRequestResponseLoggingMiddleware(new MiddlewareOptions {excludedPrefixes = new List<string>() { "/-/" }});
```

The property `excludedPrefixes` is used to avoid logging incoming and completed request's logs for routes that should not show that kind of logs.  
In particular, calling status routes should not generate any request related logs and, since all of them begin with the "/-/" prefix, we have excluded them using that property.

## Example

```csharp
    [HttpGet]
        public string Get()
        {
            Logger.Trace("trace level message");
            Logger.Trace("trace level message with object", new CustomProps("foo"));
            Logger.Trace(HttpContext.Request, "trace level request message with no object");
            Logger.Trace(HttpContext.Request, "trace level request message with object", new CustomProps("foo"));

            Logger.Debug(HttpContext.Request, "debug level request message with no object");
            Logger.Info(HttpContext.Request, "info level request message with no object");
            Logger.Warn(HttpContext.Request, "warn level request message with no object");
            Logger.Error(HttpContext.Request, "error level request message with no object");
            Logger.Fatal(HttpContext.Request, "fatal level request message with no object");
            return $"Hello World!";
        }
}
```

The CustomProps class is implemented like the following:

```csharp
public class CustomProps
    {
        public string CustomProp { get; set; }

        public CustomProps(string customProp)
        {
            CustomProp = customProp;
        }
    }
```

It is also very important to include in all examples and templates a `log4net.config` file like the following one:

```xml
<log4net>
    <root>
      <level value="ALL" />
      <appender-ref ref="console" />
    </root>
    <appender name="console" type="log4net.Appender.ConsoleAppender">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%message%newline" />
      </layout>
    </appender>
  </log4net>
```

The log4net configuration can be loaded using assembly-level attributes (the following line should be added in the `AssemblyInfo.cs` file):

```csharp
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
```

To better understand how log4net configuration works visit [Apache log4net Manual - Configuration](https://logging.apache.org/log4net/release/manual/configuration.html).

For further details about logs can you see the [guidelines for logs](https://docs.mia-platform.eu/development_suite/monitoring-dashboard/dev_ops_guide/log/).
