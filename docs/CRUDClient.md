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

##### GET methods
- To get a list of documents of type `T`:
```csharp
Task<List<T>> Get<T>([Dictionary<string, string> query = null]);
 ```
- To get a document of type `T` by id:
```csharp
Task<T> GetById<T>(string id, [Dictionary<string, string> query = null]);
```
- To count documents of type `T`:
```csharp
Task<int> Count<T>([Dictionary<string, string> query = null]);
```
- To export documents of type `T`:
```csharp
Task<HttpContent> Export<T>([Dictionary<string, string> query = null]);
```

##### POST methods
- To insert a document of type `T`:
```csharp
Task<HttpContent> Post<T>(T document, [Dictionary<string, string> query = null]);
```
- To insert multiple documents of type `T`:
```csharp
Task<HttpContent> PostBulk<T>(List<T> documents, [Dictionary<string, string> query = null]);
```
- To check if it's valid to post a document of type `T`:
```csharp
Task<HttpStatusCode> PostValidate<T>(T document, [Dictionary<string, string> query = null]);
```
- To insert (or update if already present) a document of type `T`:
```csharp
Task<HttpContent> UpsertOne<T>(T document, [Dictionary<string, string> query = null]);
```

##### PATCH methods

- To patch a document of type `T`:
```csharp
Task<HttpContent> Patch<T>(Dictionary<PatchCodingKey, Dictionary<string, object>> body, [Dictionary<string, string> query = null]);
```
- To patch a document of type `T` by id:
```csharp
Task<HttpContent> PatchById<T>(string id, Dictionary<PatchCodingKey, Dictionary<string, object>> body,[Dictionary<string, string> query = null]);
```
- To patch multiple documents of type `T`:
```csharp
 Task<HttpContent> PatchBulk<T>(IList<PatchItemSection> body, Dictionary<string, string> query = null);
```
Class `PatchCodingKey` contains static methods to get patch keys. For example, `PatchCodingKey.Set` returns a `PatchCodingKey` instance with `Value` = `"$set"`.

Class `PatchItemSection` has two `Dictionary` props: `PatchFilterSection` and `PatchUpdateSection`. When serialized,
 it produces a JSON object that looks like this:
```json
{
    filter: {...},
    update: {...}
}
```

##### DELETE methods
- To delete documents of type `T`:
```csharp
Task<HttpContent> Delete<T>([Dictionary<string, string> query = null]);
```
- To delete a document of type `T` by id:
```csharp
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
