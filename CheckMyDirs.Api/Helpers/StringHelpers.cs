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
    
    private static string GetStateRecord(string path, 
        IBaseFileState fileState,
        string specificState,
        int? previousVersion = null)
    {
        var substringToBeReplacedBy = $"{path}{Path.DirectorySeparatorChar}";
        
        var stateRecord = new StringBuilder()
            .Append(specificState + " ")
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