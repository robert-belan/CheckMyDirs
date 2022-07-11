namespace CheckMyDirs.Api.Contracts;

public interface IBaseFileState
{
    string FullName { get; set; }
    string Checksum { get; set; }
}