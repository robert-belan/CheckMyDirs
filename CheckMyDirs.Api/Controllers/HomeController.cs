using System.ComponentModel.DataAnnotations;
using CheckMyDirs.Api.Features;
using CheckMyDirs.Common;
using CheckMyDirs.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckMyDirs.Api.Controllers;

[ApiController]
[Route("/")]
public class HomeController : Controller
{
    private ProcessingHandler _processingHandler;
    
    public HomeController(ProcessingHandler processingHandler)
    {
        _processingHandler = processingHandler;
    }
    
    [HttpPost]
    public async Task<FinalReportType> GetPath([FromBody, Required] RequestDataDto inputData)
    {
        var report = await _processingHandler.Execute(inputData);
        return report;
    }
}