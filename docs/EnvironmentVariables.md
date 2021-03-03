# Environment Variables

The _MiaServiceNetLibrary_ allows the user to:

* specify the configurations schema of the _Environment Variables_ with a specific class
* validate the schema with a default validator or by implementing a custom validator
* easily retrieve the _Environment Variables_ using the .NET Dependency Injection

## The _MiaEnvsConfigurations_ abstract class

The _MiaServiceNetLibrary_ has an abstract class called _MiaEnvsConfigurations_ that contains:

* the Mia-Platform specific environment variables
* a validation method called _Validate_ that does a default validation by using the .NET _System.ComponentModel.DataAnnotations_ attributes (see more on [official documentation](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netcore-3.1))

To add your custom environment variables you must _extend_ the _MiaEnvsConfigurations_ abstract class with your custom class.

### Custom _Environment Variables_

To extend the _MiaEnvsConfigurations_ abstract class you just need to create your own class like the following example:

```csharp
using System.ComponentModel.DataAnnotations;

class MyCustomEnvironmentVariablesClass
{
  // Required string with min length 10
  [Required]
  [MinLength(10)]
  public string MyRequiredString { get; set; }

  // Mandatory string with max length 15
  [MaxLength(15)]
  public string MyMandatoryString { get; set; }

  // Mandatory int with value ranging from 1 to 10
  [Range(1, 10)]
  public int MyNumber { get; set; }
}
```

With the example above you already have:

* the Mia-Platform required Environment Variables
* your custom Environment Variables
* the default Validation that uses the _System.ComponentModel.DataAnnotations_ .NET  library (see more on [official documentation](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netcore-3.1))

:warning: The **Required** attribute of the _System.ComponentModel.DataAnnotations_ .NET library does not work very well with _non string_ properties, we suggest you to adopt specific validation on those properties.

### Custom validation of the properties

The _MiaEnvsConfigurations_ abstract class has a default _Validate_ method; since it is a _virtual_ method you can easily override it, like in the following example (the continuation of the previous one):

```csharp
class MyCustomEnvironmentVariablesClass
{
  ...
  ...
  ...

  public override void Validate()
  {
    // We recommend to always include this line of code in order to use the standard
    // validation implemented by the library.
    base.Validate();

    if (MyMandatoryString != null && MyNumber == 7)
    {
      throw ValidationException($"MyNumber cannot be {MyNumber} if MyMandatoryString is not null");
    }
  }
}
```

In this way you can add all the logic you need to validate your Environment Variables.

:warning: **We highly suggest to always keep the `base.Validate()` call into your custom Validate function**, otherwise the Mia-Platform default Environment Variables validation will be skipped and your service could not work.
