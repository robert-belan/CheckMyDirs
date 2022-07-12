using CheckMyDirs.Api.Contracts;

namespace CheckMyDirs.Api.Models;

public class PreviousFileStateType : IBaseFileState
{
    public string FullName { get; set; } = string.Empty;
    public string Checksum { get; set; } = string.Empty;
    public int? Version { get; set; }
}