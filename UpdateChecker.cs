using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;

namespace Nefarius.Utilities.GitHubUpdater;

/// <summary>
///     Checks for updates via GitHub API.
/// </summary>
public class UpdateChecker
{
    /// <summary>
    ///     Creates a new instance using the provided GitHub username and repository.
    /// </summary>
    /// <param name="username">The name of the user or organization owning the repository to check against.</param>
    /// <param name="repository">The repository to query.</param>
    public UpdateChecker(string username, string repository)
    {
        Username = username;
        Repository = repository;
    }
    
    /// <summary>
    ///     The name of the user or organization owning the repository to check against.
    /// </summary>
    public string Username { get; }

    /// <summary>
    ///     The repository to query.
    /// </summary>
    public string Repository { get; }

    /// <summary>
    ///     Gets the control application version.
    /// </summary>
    public static Version AssemblyVersion => Assembly.GetEntryAssembly().GetName().Version;

    /// <summary>
    ///     Gets the releases API URI.
    /// </summary>
    public Uri ReleasesUri => new($"https://api.github.com/repos/{Username}/{Repository}/releases");

    /// <summary>
    ///     True if tag on latest GitHub release is newer than own assembly version, false otherwise.
    /// </summary>
    public bool IsUpdateAvailable
    {
        get
        {
            try
            {
                // Query for releases/tags and store information
                using var client = new WebClient();

                // Required or result is HTTP-403
                client.Headers["User-Agent"] =
                    "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                    "(compatible; MSIE 6.0; Windows NT 5.1; " +
                    ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                // Get body
                var response = client.DownloadString(ReleasesUri);

                // Get JSON objects
                var result = JsonConvert.DeserializeObject<IList<Root>>(response);

                // Top release is latest of interest
                var latest = result.FirstOrDefault();

                // No release found to compare to, bail out
                if (latest == null)
                    return false;

                var tag = new string(latest.TagName.Skip(1).ToArray());

                // Expected format e.g. "v1.2.3" so strip first character
                var version = Version.Parse(tag);

                var isOutdated = version.CompareTo(AssemblyVersion) > 0;

                return isOutdated;
            }
            catch
            {
                // May happen on network issues, ignore
                return false;
            }
        }
    }
}