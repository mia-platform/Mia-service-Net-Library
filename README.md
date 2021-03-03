[![Build Status][github-actions-svg]][github-actions]
![Nuget](https://img.shields.io/nuget/v/MiaServiceDotNetLibrary)

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
* [**Service proxy**](./docs/ServiceProxy.md)
* [**CRUD client**](./docs/CRUDClient.md)
* [**Decorators**](./docs/Decorators.md)
* [**Access platform headers**](./docs/MIAHeaders.md)
* [**Logging**](./docs/Logging.md)
* [**Environment variables**](./docs/EnvironmentVariables.md)
* [**Release steps**](./docs/Release.md)


[github-actions]: https://github.com/mia-platform/Mia-service-Net-Library/actions
[github-actions-svg]: https://github.com/mia-platform/Mia-service-Net-Library/workflows/.NET%20Core/badge.svg
