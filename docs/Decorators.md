
## Decorators
This section explains how to define and handle your [Decorator](https://docs.mia-platform.eu/docs/development_suite/api-console/api-design/crud_advanced) routes.

### PRE decorators
#### Request
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

#### Response
Both the result of `ChangeOriginalRequest` building operation and the one of `LeaveOriginalRequestUnmodified` call can be passed to static method
 `DecoratorResponseFactory.MakePreDecoratorResponse(PreDecoratorRequest preDecoratorRequest)`.
This method returns an instance of `DecoratorResponse`, which represents the response that should be returned.

#### Abort chain
To abort the decorator chain, you can obtain the related `DecoratorResponse` instance by calling the method
 `DecoratorResponseFactory.AbortChain(int finalStatusCode, [Optional]IDictionary<string, string> finalHeaders, [Optional]ExpandoObject finalBody)`.


### POST decorators
#### Request
An instance of class `PostDecoratorRequest` can be used as input to the decorator handler.

The properties of the `PostDecoratorRequest` instance can be used to access both the original request and the original response:

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

#### Response
Both the result of `ChangeOriginalResponse` building operation and the one of `LeaveOriginalResponseUnmodified` call can be passed to static method
 `DecoratorResponseFactory.MakePostDecoratorResponse(PostDecoratorRequest postDecoratorRequest)`.
This method returns an instance of `DecoratorResponse`, which represents the response that should be returned.

#### Abort chain
To abort the decorator chain, you can obtain the related `DecoratorResponse` instance by calling the method
 `DecoratorResponseFactory.AbortChain(int finalStatusCode, [Optional]IDictionary<string, string> finalHeaders, [Optional]ExpandoObject finalBody)`.

