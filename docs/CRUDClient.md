## CRUD client

### Getting a CRUD client
To get a CRUD client, you can use the following methods from class `ServiceClientFactory`:
You have to specify:
+ the API endpoint
+ (optional) the API secret
+ (optional) the API version

 ```csharp
var crudClient = ServiceClientFactory.GetCrudServiceClient("http://localhost:300O", "my-secret", 2);
 ```

 ### Collection mapping
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

 ### Building a query
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
