using System.Text.RegularExpressions;
using CheckMyDirs.Api.Exceptions;

namespace CheckMyDirs.Api.Features;

public class PathValidationHandler
{
    private readonly PlatformID _currentPlatform = Environment.OSVersion.Platform;
    
    public string TryGetFullPath(string path)
    {
        if (path.Equals(string.Empty) || path is null)
        {
            throw new InvalidPathException("Path could not be null or empty.");
        }

        string validatedPath;

        switch (_currentPlatform)
        {
            case PlatformID.Unix:
            case PlatformID.MacOSX:
                validatedPath = GetUnixSpecificPath(path);
                break;

            case PlatformID.Win32NT:
            case PlatformID.Win32Windows:
            case PlatformID.Win32S:
            case PlatformID.WinCE:
                validatedPath = GetWindowsSpecificPath(path);
                break;

            default:
                throw new InvalidPathException("We're sorry, you are using unsupported operating system.");
        }

        // check if path leads to directory
        if (!File.GetAttributes(validatedPath).HasFlag(FileAttributes.Directory))
        {
            throw new InvalidPathException("This path leads to file, not directory.");
        }
        
        if (!Directory.Exists(validatedPath))
        {
            throw new InvalidPathException($"Directory \"{validatedPath}\" not found.");
        }

        return validatedPath;
    }
    
    private string GetUnixSpecificPath(string path)
    {
        // if root
        if (path.Equals("/"))
        {
            throw new InvalidPathException("You shouldn't version root directory. It could disrupt the Universe harmony. I'm stopping you.");
        }

        if (path.StartsWith("~/"))
        {
            return path.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
        }

        return path;
    }
    
    private string GetWindowsSpecificPath(string path)
    {
        // Seems to Windows withstands everything
        return path;
    }
}