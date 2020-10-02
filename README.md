# Mia service .NET Library
A library that allows you to define Mia-Platform custom services in .NET Core easily.

## Purpose
This library aims to provide an easy way to integrate your custom microservices with [Mia-Platform](https://mia-platform.eu).
You can use it is to integrate with your defined CRUD and other custom services that run is your DevOps Console Project.

## Install

**Package Manager**

```bash
Install-Package MiaServiceDotNetLibrary -Version 1.0.0
```

**.Net CLI**

```bash
dotnet add package MiaServiceDotNetLibrary --version 1.0.0
```

**PackageReference**
```xml
<PackageReference Include="MiaServiceDotNetLibrary" Version="1.0.0" />
```

## Usage documentation
* [<b>Service proxy</b></a>](./docs/ServiceProxy.md)
* [<b>CRUD client</b>](./docs/CRUDClient.md)
* [<b>Decorators</b>](./docs/Decorators.md)
* [<b>Access platform headers</b>](./docs/MIAHeaders.md)
* [<b>Logging</b>](./docs/Logging.md)
* [<b>Release steps</b>](./docs/Release.md)
