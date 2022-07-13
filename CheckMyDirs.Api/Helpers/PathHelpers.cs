using System.Text;
using CheckMyDirs.Api.Constants;
using CheckMyDirs.Api.Exceptions;
using CheckMyDirs.Api.Contracts;

namespace CheckMyDirs.Api.Helpers;

public static class PathHelpers
{
    public static string GetParent(string path)
    {
        var parent = Directory.GetParent(path);
        if (parent is null)
        {
            throw new InvalidPathException(
                $"Parent directory of \"{path}\" not exists. \"I'm Root\".");
        }
        
        return parent.ToString();
    }
    
    public static string GetDotPseudogit(string path)
    {
        // Originaly
        // return Path.Combine(GetParent(path), ExtensionTypes.DotPseudogit);

        // Newly ".pseudogit" resides in folder being checked
        return Path.Combine(path, ExtensionTypes.DotPseudogit);
    }

    public static async Task<string[]> GetDotPseudogitFilesLogs()
    {
        var dotPseudogitFilesLogPath = GetDotPseudogitFileLogPath();

        string[] locations = {};
        if (!string.IsNullOrEmpty(dotPseudogitFilesLogPath))
        {
            locations = await File.ReadAllLinesAsync(dotPseudogitFilesLogPath);
            return locations;
        }
        
        return locations;
    }

    public static string GetDotPseudogitFileLogPath()
    {
        var location = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
            LogLocationsTypes.DotPseudogitFilesLocationsLog);

        return !File.Exists(location) ? string.Empty : location;
    }
}