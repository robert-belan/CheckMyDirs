using System.Security.Cryptography;
using CheckMyDirs.Api.Helpers;
using CheckMyDirs.Api.Models;

namespace CheckMyDirs.Api.Features;

public sealed class CurrentFilesStatesHandler
{
    private string _path = string.Empty;
    private FileInfo[]? _files = Array.Empty<FileInfo>();
    private List<CurrentFileStateType> _filesWithChecksum = new();
    

    public List<CurrentFileStateType> GetCurrentFilesStates(string path)
    {
        _path = path;
        
        PopulateFiles();
        GetFilesChecksums();

        return _filesWithChecksum;
    }
    
    private void PopulateFiles()
    {
        _files = new DirectoryInfo(_path).GetFiles("*.*", SearchOption.AllDirectories);
    }

    private void GetFilesChecksums()
    {
        using var sha1 = SHA1.Create();
        
        foreach (var file in _files!)
        {
            // Get SHA1 checksum value
            // using var fileStream = file.Open(FileMode.Open);
            using var fileStream = file.OpenRead();
            
            var hash = sha1.ComputeHash(fileStream);
            
            // Ignore ".pseudogit" file
            if (file.FullName.Equals(PathHelpers.GetDotPseudogit(_path)))
            {
                continue;
            }
            
            // Populate element
            var currentFileState = new CurrentFileStateType
            {
                FullName = file.FullName,
                Checksum = Convert.ToHexString(hash)
            };

            _filesWithChecksum.Add(currentFileState);
        }
    }
}