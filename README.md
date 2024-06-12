# <img src="assets/NSS-128x128.png" align="left" />Nefarius.Utilities.GitHubUpdater

[![Build status](https://ci.appveyor.com/api/projects/status/pfpifkvaiahsnfr6?svg=true)](https://ci.appveyor.com/project/nefarius/nefarius-utilities-githubupdater) ![Requirements](https://img.shields.io/badge/Requires-.NET%20Standard%202.0-blue.svg) [![Nuget](https://img.shields.io/nuget/v/Nefarius.Utilities.GitHubUpdater)](https://www.nuget.org/packages/Nefarius.Utilities.GitHubUpdater/) [![Nuget](https://img.shields.io/nuget/dt/Nefarius.Utilities.GitHubUpdater)](https://www.nuget.org/packages/Nefarius.Utilities.GitHubUpdater/)

Utility classes to perform application update checks using GitHub repositories.

## Example

Put a snippet similar to the following in your application startup sequence:

```csharp
#if !DEBUG
        if (new UpdateChecker("nefarius", "Identinator").IsUpdateAvailable)
        {
            await this.ShowMessageAsync("Update available",
                "A newer version of the Identinator is available, I'll now take you to the download site!");
            Process.Start(new ProcessStartInfo("https://github.com/nefarius/Identinator/releases/latest")
                { UseShellExecute = true });
        }
#endif
```
