using CheckMyDirs.Api.Contracts;

namespace CheckMyDirs.Api.Models;

public class CurrentFileStateType : IBaseFileState
{
    public string FullName { get; set; } = string.Empty;
    public string Checksum { get; set; } = string.Empty;
}