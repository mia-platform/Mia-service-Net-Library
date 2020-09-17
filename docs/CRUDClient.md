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
CRUD methods require you to map your class to a collection. To do this, you have to specify the collection name within the `Attribute` ``[CollectionName]``. 
For example:  

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


### Client methods
The client provides the following methods:

```csharp
Task<List<T>> Get<T>([Dictionary<string, string> query = null]);
Task<T> GetById<T>(string id, [Dictionary<string, string> query = null]);
Task<int> Count<T>([Dictionary<string, string> query = null]);
Task<HttpContent> Export<T>([Dictionary<string, string> query = null]);
Task<HttpContent> Post<T>(T document, [Dictionary<string, string> query = null]);
Task<HttpContent> PostBulk<T>(List<T> documents, [Dictionary<string, string> query = null]);
Task<HttpStatusCode> PostValidate<T>(T document, [Dictionary<string, string> query = null]);
Task<HttpContent> UpsertOne<T>(T document, [Dictionary<string, string> query = null]);
Task<HttpContent> Patch<T>(Dictionary<PatchCodingKey, Dictionary<string, object>> body, [Dictionary<string, string> query = null]);
Task<HttpContent> PatchById<T>(string id, Dictionary<PatchCodingKey, Dictionary<string, object>> body,[Dictionary<string, string> query = null]);
Task<HttpContent> PatchBulk<T>(PatchBulkBody body, [Dictionary<string, string> query = null]);
Task<HttpContent> Delete<T>([Dictionary<string, string> query = null]);
Task<HttpContent> DeleteById<T>(string id, [Dictionary<string, string> query = null]);
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
