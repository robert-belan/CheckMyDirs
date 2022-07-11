using CheckMyDirs.Common;
using CheckMyDirs.Common.Models;
using CheckMyDirs.Api.Constants;
using CheckMyDirs.Api.Models;

namespace CheckMyDirs.Api.Features;

public sealed class ProcessingHandler
{
    private readonly ComparerHandler _comparerHandler;
    private readonly PathValidationHandler _pathValidationHandler;
    private readonly CurrentFilesStatesHandler _currentFilesStatesHandler;
    private readonly PreviousFilesStatesHandler _previousFilesStatesHandler;
    
    public ProcessingHandler(
        ComparerHandler comparerHandler,
        PathValidationHandler pathValidationHandler,
        CurrentFilesStatesHandler currentFilesStatesHandler,
        PreviousFilesStatesHandler previousFilesStatesHandler)
    {
        _comparerHandler = comparerHandler;
        _pathValidationHandler = pathValidationHandler;
        _currentFilesStatesHandler = currentFilesStatesHandler;
        _previousFilesStatesHandler = previousFilesStatesHandler;
    }

    public async Task<FinalReportType> Execute(RequestDataDto userInput)
    {
        var validatedPath = _pathValidationHandler.TryGetFullPath(userInput.Path);
        
        var currentFilesStates = _currentFilesStatesHandler.GetCurrentFilesStates(validatedPath);
        var previousFilesStates = _previousFilesStatesHandler.TryGetPreviousFilesStates(validatedPath);

        var report = await _comparerHandler.CompareAndGetFinalReport(validatedPath, 
            currentFilesStates, 
            previousFilesStates);

        return report;
    }
}

