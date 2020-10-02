## Release steps

### Changelog update
When you want to release a new version, update the `CHANGELOG.md` file with the proper [format](https://keepachangelog.com/en/1.0.0/).

### Version increment
Increment the release version by updating property `Version` in `MiaServiceDotNetLibrary/MiaServiceDotNetLibrary.csproj` file.
For example:
```diff
    <PropertyGroup>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <PackageId>MiaServiceDotNetLibrary</PackageId>
 -       <Version>1.4.2</Version>
 +       <Version>2.0.0</Version>
        <Company>MiaPlatform</Company>
    </PropertyGroup>
```

### Version tag
Create a tag for the new version with `git tag -a [new_version] -m [message]`. For example:
```bash
git tag -a v.2.0.0 -m "Release 2.0.0"
```

### NuGet Package
To create a NuGet package, run `dotnet pack`.
