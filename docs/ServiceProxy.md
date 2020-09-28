## Service proxy
### Service options
Before getting your service proxy, you need to inject the appropriate `InitServiceOptions` instance:
You have to specify:
+ the service port, default is `3000`
+ the service protocol, either `Protocol.Http` or `Protocol.Https`, default is `Http`
+ default proxy headers 
+ path prefix

For example: 
```csharp
var initOptions = new InitServiceOptions(8080, Protocol.Http, new Dictionary<string, string> {{"token", "foobar"}}, "api");
 ```

### Getting a proxy
To get a service proxy, you can use the following methods from class `ServiceClientFactory`:

+ `GetDirectServiceProxy`, to directly communicate with a specific microservice
```csharp
var proxy = ServiceClientFactory.GetDirectServiceProxy("my-microservice", initOptions);
``` 

+ `GetServiceProxy`, to communicate with a specific custom service passing through the Microservice Gateway:
```csharp
var proxy = ServiceClientFactory.GetServiceProxy(initOptions);
``` 

### Proxy methods
The proxy provides the following methods to make HTTP requests:


```csharp
Task<HttpResponseMessage> Get(string path, [string queryString = ""], [string body = ""], [ServiceOptions options = null]);
Task<HttpResponseMessage> Post(string path, [string queryString = ""], [string body = ""], [ServiceOptions options = null]);
Task<HttpResponseMessage> Put(string path, [string queryString = ""], [string body = ""], [ServiceOptions options = null]);
Task<HttpResponseMessage> Patch(string path, [string queryString = ""], [string body = ""], [ServiceOptions options = null]);
Task<HttpResponseMessage> Delete(string path, [string queryString = ""], [string body = ""], [ServiceOptions options = null]);
```
 
