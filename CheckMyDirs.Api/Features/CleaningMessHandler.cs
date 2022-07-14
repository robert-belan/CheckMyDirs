using CheckMyDirs.Api.Constants;
using CheckMyDirs.Api.Helpers;
using CheckMyDirs.Common.Models;

namespace CheckMyDirs.Api.Features;

public class CleaningMessHandler
{
    /// <remarks>
        /// See https://starwars.fandom.com/wiki/Order_66
    /// </remarks>
    public async Task ExecuteOrder66()
    {
        var locations = await PathHelpers.GetDotPseudogitFilesLogs();

        // Delete all .pseudogit files ever created
        foreach (var location in locations)
        {
            File.Delete(location);
        }

        // Delete log itself
        var logLocation = PathHelpers.GetDotPseudogitFileLogPath();
        if (!string.IsNullOrEmpty(logLocation))
        {
            File.Delete(logLocation);
        };
            
    }
}