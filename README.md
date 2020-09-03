# Mia service .NET Library
A library that allows you to define Mia-Platform custom services in .NET Core easily.

## Purpose
This library aims to provide an easy way to integrate the Platform.
It is able to integrate either the CRUD and other REST services.

## Usage
### Service proxy
#### Service options
Before getting your service proxy, you need to inject the appropriate `InitServiceOptions` instance:
 +  ```csharp
    var initOptions = new InitServiceOptions(8080, Protocol.Http);
    ```

#### Getting a proxy
To get a service proxy, you can use the following methods from class `ServiceClientFactory`:

+ `GetDirectServiceProxy`, to directly communicate with a specific microservice
    ```csharp
    var proxy = ServiceClientFactory.GetDirectServiceProxy("my-microservice", initOptions);
    ``` 

+ `GetServiceProxy`, to use microservice gateway:
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

 +  ```csharp
        var crudClient = ServiceClientFactory.GetCrudServiceClient("http://localhost:300O", "my-secret", 3);
    ```
 #### Calling an API
You can use CRUD client's methods to call the API endpoint you defined.
 #### Collection mapping
 To map your class to a collection, you have to specify the collection name within the `Attribute` ``[CollectionName]``.   
  ```csharp
        [CollectionName("users")]
        public class User
        {
            public int Id { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Status { get; set; }
    
            public User() {}
    
            public User(int id, string firstname, string lastname, string status)
            {
                Id = id;
                Firstname = firstname;
                Lastname = lastname;
                Status = status;
            }
        }
    ``` 
