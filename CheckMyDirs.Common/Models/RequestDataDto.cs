using System.ComponentModel.DataAnnotations;

namespace CheckMyDirs.Common.Models;

public class RequestDataDto
{
    [Required(ErrorMessage = "Please, give me some path.")]
    public string Path { get; set; } = string.Empty;
}