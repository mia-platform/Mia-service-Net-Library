# Mia service .NET Library
A library that allows you to define Mia-Platform custom services in .NET Core easily.

## Purpose
This library aims to provide an easy way to integrate your custom microservices with [Mia-Platform](https://mia-platform.eu).
You can use it is to integrate with your defined CRUD and other custom services that run is your DevOps Console Project.

## Usage
### Service proxy
#### Service options
Before getting your service proxy, you need to inject the appropriate `InitServiceOptions` instance:

```csharp
var initOptions = new InitServiceOptions(8080, Protocol.Http);
 ```

#### Getting a proxy
To get a service proxy, you can use the following methods from class `ServiceClientFactory`:

+ `GetDirectServiceProxy`, to directly communicate with a specific microservice
```csharp
var proxy = ServiceClientFactory.GetDirectServiceProxy("my-microservice", initOptions);
``` 

+ `GetServiceProxy`, to communicate with a specific custom service passing through the Microservice Gateway:
```csharp
var proxy = ServiceClientFactory.GetServiceProxy(initOptions);
``` 
### CRUD client

#### Getting a CRUD client
To get a CRUD client, you can use the following methods from class `ServiceClientFactory`:
You have to specify:
+ the API endpoint
+ (optional) the API secret
+ (optional) the API version

 ```csharp
var crudClient = ServiceClientFactory.GetCrudServiceClient("http://localhost:300O", "my-secret", 2);
 ```

 #### Collection mapping
 Some CRUD methods require you to map your class to a collection. To do this, you have to specify the collection name within the `Attribute` ``[CollectionName]``.   

```csharp
[CollectionName("users")]
public class User
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    
    public User() {}
    
    public User(int id, string firstname, string lastname, string status)
    {
        Id = id;
        Firstname = firstname;
        Lastname = lastname;
    }
}
``` 

 #### Building a query
 You can use class `CrudQueryBuilder` to build your HTTP query.
 It provides specific methods to set the standard Mia CRUD query parameters and the method `Param` to set custom query parameters.
 To set the value of the Mongo query parameter, you can also use a specific class (`MongoQueryBuilder`).

```csharp
var query = new CrudQueryBuilder()
    .State(State.Draft)
    .CreatorId("foo")
    .CreatedAt("bar")
    .MongoQuery(new MongoQueryBuilder().NotIn("foo", new List<string>{"bar", "baz"}))
    .Param("foo", "bar")
    .Build();

var result = await crudClient.Get<User>(query);
``` 

### Decorators

#### PRE decorators
##### Request
An instance of class `PreDecoratorRequest` can be used as input to the decorator handler.

The properties of the `PreDecoratorRequest` instance can be used to access the original request:

+ `Method` - the original request method
+ `Path` - the path of the original request
+ `Headers` - the headers of the original request
+ `Query` - the querystring of the original request
+ `Body` - the body of the original request

Moreover, the `PreDecoratorRequest` instance exposes an interface to modify the original request,
 which will come forward to the target service. This interface is accessible using the instance method 
 `ChangeOriginalRequest` which returns a builder for `PreDecoratorRequestProxy` object. This object wraps 
 a copy of the original request and provides the following methods to modify that copy:

+ `Method(string method)` - modify the method of the original request
+ `Path(string path)` - modify the path of the original request
+ `Headers(IDictionary<string, string> headers)` - modify the headers of the original request
+ `Query (IDictionary<string, string> query)` - modify the querystring of the original request
+ `Body(ExpandoObject body)` - modify the body of the original request
+ `Change()` - return the resulting `PreDecoratorRequest` instance.

To leave the original request unchanged, you can instead use `LeaveOriginalRequestUnmodified` method.

##### Response
Both the result of `ChangeOriginalRequest` building operation and the one of `LaveOriginalRequestUnmodified` call can be passed to static method
 `DecoratorResponseFactory.MakePreDecoratorResponse(PreDecoratorRequest preDecoratorRequest)`.
This method returns an instance of `DecoratorResponse`, which represents the response that should be returned.

##### Abort chain
To abort the decorator chain, you can obtain the related `DecoratorResponse` instance by calling the method
 `DecoratorResponseFactory.AbortChain(int finalStatusCode, [Optional]IDictionary<string, string> finalHeaders, [Optional]ExpandoObject finalBody)`.


#### POST decorators
##### Request
The properties of the `PostDecoratorRequest` instance can be used to access the original request:

The utility functions exposed by the `PostDecoratorRequest` instance can be used to access both the original request and the original response:

+ `Request.Method` - returns the original request method
+ `Request.Path` - returns the path of the original request
+ `Request.Headers` - returns the headers of the original request
+ `Request.Query` - returns the querystring of the original request
+ `Request.Body` - returns the body of the original request
+ `Response.StatusCode` - returns the body of the original response
+ `Request.Headers` - returns the headers of the original response
+ `Request.Body` - returns the status code of the original response

Moreover, the `PostDecoratorRequest` instance exposes an interface to modify the original request,
 which will come forward to the target service. This interface is accessible using the instance method 
 `ChangeOriginalResponse` which returns a builder for `PostDecoratorRequestProxy` object. This object wraps 
 a copy of the original request and provides the following methods to modify that copy:
 
+ `Headers(IDictionary<string, string> headers)` - modify the headers of the original response
+ `Body(ExpandoObject body)` - change the body of the original response
+ `StatusCode(int statusCode)` - change the status code of the original response
+ `Change()` - return the resulting `PostDecoratorRequest` instance.

To leave the original response unchanged, the `LeaveOriginalResponseUnmodified` function can be used instead.

##### Response
Both the result of `ChangeOriginalResponse` building operation and the one of `LeaveOriginalResponseUnmodified` call can be passed to static method
 `DecoratorResponseFactory.makePostDecoratorResponse(PostDecoratorRequest postDecoratorRequest)`.
This method returns an instance of `DecoratorResponse`, which represents the response that should be returned.

##### Abort chain
To abort the decorator chain, you can obtain the related `DecoratorResponse` instance by calling the method
 `DecoratorResponseFactory.AbortChain(int finalStatusCode, [Optional]IDictionary<string, string> finalHeaders, [Optional]ExpandoObject finalBody)`.

