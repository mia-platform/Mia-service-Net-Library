### Access platform headers
In order to access platform headers, you can get an instance of class `MiaHeadersPropagator` from 
`HttpContext.Items` with key `"MiaHeadersPropagator"`:

```csharp
var miaHeadersPropagator = (MiaHeadersPropagator) HttpContext.Items["MiaHeadersPropagator"];
```

`MiaHeadersPropagator` instance exposes the following methods:
```csharp
string GetUserId();
string GetGroups();
string GetClientType();
bool IsFromBackOffice();
```
