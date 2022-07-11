namespace CheckMyDirs.Common.Models;

public class FinalReportType
{
    public List<string> Records { get; set; } = new();
    public string? Message { get; set; }
    public string? Error { get; set; }
}