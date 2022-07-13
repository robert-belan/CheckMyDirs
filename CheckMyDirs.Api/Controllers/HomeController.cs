using System.ComponentModel.DataAnnotations;
using CheckMyDirs.Api.Features;
using CheckMyDirs.Api.Helpers;
using CheckMyDirs.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckMyDirs.Api.Controllers;

[ApiController]
[Route("/")]
public class HomeController : Controller
{
    private ProcessingHandler _processingHandler;
    private readonly CleaningMessHandler _cleaningMessHandler;

    public HomeController(ProcessingHandler processingHandler,
        CleaningMessHandler cleaningMessHandler)
    {
        _processingHandler = processingHandler;
        _cleaningMessHandler = cleaningMessHandler;
    }
    
    [HttpPost]
    public async Task<ActionResult<FinalReportType>> GetPath([FromBody, Required] RequestDataDto inputData)
    {
        var report = await _processingHandler.Execute(inputData);
        return new OkObjectResult(report);
    }

    [HttpGet]
    [Route("pseudogitfiles")]
    public async Task<ActionResult<string[]>> GetCreatedDotPseudogitFilePaths()
    {
        var locations = await PathHelpers.GetDotPseudogitFilesLogs();
        return new OkObjectResult(locations);
    }
    
    // TODO: Warning, HttpGet here is just bug hotfix/workaround. [HttpDelete] method causes HTTP 405 error.
    // See also https://docs.microsoft.com/en-us/aspnet/web-api/overview/testing-and-debugging/troubleshooting-http-405-errors-after-publishing-web-api-applications
    [HttpGet]
    [Route("clean")]
    public async Task<ActionResult> DeleteCreatedDotPseudogitFiles()
    {
        await _cleaningMessHandler.ExecuteOrder66();
        return NoContent();
    }
} 