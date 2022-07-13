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

        #region Files Processiong Limitation
        // Limit functionality due to unimplemented results paging - huge number of files could take serious time
        // TODO: Delete files processing limitation after paging implementation
        if (_files.Length > 1000)
        {
            throw new InvalidOperationException(
                "Desired directory contains too many files to process (greater than 1000). Due to unimplemented result paging there is a safety limit.");
        }
        #endregion
    }

    private void GetFilesChecksums()
    {
        using var sha1 = SHA1.Create();
        
        foreach (var file in _files!)
        {
            // Get SHA1 checksum value
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