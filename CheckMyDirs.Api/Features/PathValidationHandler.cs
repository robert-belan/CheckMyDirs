using System.Text.RegularExpressions;
using CheckMyDirs.Api.Exceptions;

namespace CheckMyDirs.Api.Features;

public class PathValidationHandler
{
    private readonly PlatformID _currentPlatform = Environment.OSVersion.Platform;
    private readonly string _separator = Path.DirectorySeparatorChar.ToString();
    
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

    private string GetUnixSpecificPath(string input)
    {
        // if windows specific path provided
        // Warning: the path validation is more complicated and should be handled
        //  see: https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file
        
        // TODO: Incorrect regex - Need to be replaced !!!
        var windowsPathRegexPattern = "^[A-Za-z]:\\(?:[^\\/:*?\"<>|\r\n]+\\)*[^\\/:*?\"<>|\r\n]*$";
        
        var reg = new Regex(windowsPathRegexPattern, RegexOptions.IgnoreCase);
        if (reg.IsMatch(input))
        {
            throw new InvalidPathException("Using windows specific path on Unix-like system.");
        }

        if (input.Equals("/"))
        {
            throw new InvalidPathException("You shouldn't version root directory. It could disrupt the Universe harmony. I'm stopping you.");
        }

        if (input.EndsWith($"..{_separator}"))
        {
            return Directory.GetParent(input).ToString();
        }
        
        if (input.EndsWith($".{_separator}"))
        {
            return Directory.GetCurrentDirectory();
        }
        
        if (input.EndsWith(_separator) & !input.EndsWith($"{_separator}{_separator}" ))
        {
            input = input.TrimEnd(Path.DirectorySeparatorChar);
        }
        
        if (input.StartsWith("~/"))
        {
            return input.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
        }
    
        // if relative value provided
        if (!input.StartsWith(_separator) | input.StartsWith("./"))
        {
            return Path.Combine(Directory.GetCurrentDirectory(), input);
        }
    
        return input;
    }
    
    private string GetWindowsSpecificPath(string input)
    {
        // Warning: the path validation is more complicated and should be handled
        //  see: https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file
        return input;
    }
}