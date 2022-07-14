using System.Text;
using CheckMyDirs.Api.Constants;
using CheckMyDirs.Api.Contracts;

namespace CheckMyDirs.Api.Helpers;

public static class StringHelpers
{
    public static string GetModified(string path, IBaseFileState fileState, int? previousVersion)
    {
        return GetStateRecord(path, fileState, ReportStatusTypes.Modified, previousVersion);
    }

    public static string GetAdded(string path, IBaseFileState fileState)
    {
        return GetStateRecord(path, fileState, ReportStatusTypes.Added);
    }
    
    public static string GetDeleted(string path, IBaseFileState fileState)
    {
        return GetStateRecord(path, fileState, ReportStatusTypes.Deleted);
    }
    

    /// <remarks>
    ///     This func is used in loop, that's why Stringbuilders everywhere.
    /// </remarks>
    private static string GetStateRecord(string path, 
        IBaseFileState fileState,
        string specificState,
        int? previousVersion = null)
    {
        var substringToBeReplacedBy = new StringBuilder()
            .Append(path)
            .Append(Path.DirectorySeparatorChar)
            .ToString();

        var stateRecord = new StringBuilder()
            .Append(specificState)
            .Append(' ')
            .Append(fileState.FullName.Replace(substringToBeReplacedBy, string.Empty));

        if (specificState.Equals(ReportStatusTypes.Modified))
        {
            stateRecord.Append(" (in version ")
                .Append(previousVersion)
                .Append(')');
        }
        
        return stateRecord.ToString();
    }
}