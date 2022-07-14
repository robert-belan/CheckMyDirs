using System.Text;
using System.Text.Json;
using CheckMyDirs.Api.Constants;
using CheckMyDirs.Api.Helpers;
using CheckMyDirs.Api.Models;
using CheckMyDirs.Common.Models;
using CheckMyDirs.Api.Contracts;

namespace CheckMyDirs.Api.Features;

public class ComparerHandler
{
    private readonly ILoggerManager _logger;
    private List<string> _reportRecords = new();
    private Dictionary<string, PreviousFileStateType>? _previousStatesIndex; 
    private Dictionary<string, CurrentFileStateType>? _currentStatesIndex;

    
    
    public ComparerHandler(ILoggerManager logger)
    {
        _logger = logger;
    }
    
    public async Task<FinalReportType> CompareAndGetFinalReport(
        string path, 
        List<CurrentFileStateType> currentFilesStates,
        List<PreviousFileStateType> previousFilesStates) 
    {
        
        // Index current states entities
        _currentStatesIndex = currentFilesStates
            .ToDictionary(currentFilesState => currentFilesState.FullName);

        var finalReport = new FinalReportType();
        
        if (!previousFilesStates.Any()) // == directory not checked previously
        {
            #region Create NEW .pseudogit file

            List<PreviousFileStateType> fileStates = new();
            
            foreach (var currentFilesState in currentFilesStates)
            {
                var fileEntity = new PreviousFileStateType
                {
                    FullName = currentFilesState.FullName,
                    Checksum = currentFilesState.Checksum,
                    Version = InitialValuesConstants.FileVersion
                };

                // Populate report
                var stateRecord = StringHelpers.GetAdded(path, currentFilesState);
                _reportRecords.Add(stateRecord);
                
                fileStates.Add(fileEntity);
            }

            var dotPseudogitLocation = PathHelpers.GetDotPseudogit(path);
            
            // Create new ".pseudogit" file and save up-to-date states
            await SaveChanges(dotPseudogitLocation, fileStates);

            // Log ".pseudogit" file creation location
            _logger.LogInfo(dotPseudogitLocation);
            
            // Notify user
            finalReport.Message = "Files-state history recording initialized.";

            #endregion
        }
        else
        {
            // Index previous states entities
            _previousStatesIndex = previousFilesStates
                .ToDictionary(previousStateEntity => previousStateEntity.FullName);

            if (!currentFilesStates.Any()) {
                finalReport.Message = "Folder is empty.";
            }
            
            #region Check ADDED and MODIFIED files
            foreach (var currentFileState in currentFilesStates)
            {
                // If the same file path was FOUND (== file still exists)
                if (_previousStatesIndex.ContainsKey(currentFileState.FullName))
                {
                    var previousFileState = _previousStatesIndex[currentFileState.FullName];

                    // If checksums NOT EQUAL (== file was MODIFIED)
                    if (previousFileState.Checksum != currentFileState.Checksum)
                    {
                        var stateRecord = StringHelpers.GetModified(path, currentFileState, previousFileState.Version);
                        _reportRecords.Add(stateRecord);
                        
                        // Update file state
                        previousFileState.Version++;
                        previousFileState.Checksum = currentFileState.Checksum;
                    }  
                }
                // If file wasn't exist previously (== was ADDED)
                else if (!_previousStatesIndex.ContainsKey(currentFileState.FullName)) 
                {
                    var stateRecord = StringHelpers.GetAdded(path, currentFileState);
                    
                    _reportRecords.Add(stateRecord);
                    
                    // Update index by adding new entry
                    _previousStatesIndex.Add(currentFileState.FullName, new PreviousFileStateType()
                    {
                        FullName = currentFileState.FullName,
                        Checksum = currentFileState.Checksum,
                        Version = InitialValuesConstants.FileVersion
                    });
                }
            }
            
            #endregion

            #region Check DELETED files

            foreach (var previousFileState in previousFilesStates)
            {
                // Process only deleted files
                if (_currentStatesIndex.ContainsKey(previousFileState.FullName)) continue;

                // Update index
                _previousStatesIndex.Remove(previousFileState.FullName);

                // var recordState = new StringBuilder()
                //     .Append(ReportStatusTypes.Deleted)
                //     .Append(previousFileState.FullName.Replace(path, string.Empty)); 
                
                var stateRecord = StringHelpers.GetDeleted(path, previousFileState);
                
                _reportRecords.Add(stateRecord);
            }

            #endregion

            // Save up-to-date states
            var updatedStates = _previousStatesIndex.Values.ToList();
            await SaveChanges(PathHelpers.GetDotPseudogit(path), updatedStates);

            if (currentFilesStates.Count != 0)
            {
                var message = _reportRecords.Count == 0
                    ? "Nothing has change."
                    : $"{_reportRecords.Count} changes has been made.";
                finalReport.Message = message;
            }
        }

        // Complete and return report
        finalReport.Records = _reportRecords.ToList();
        return finalReport;
    }
    
    private async Task SaveChanges(string saveFilePath, List<PreviousFileStateType> filesStates)
    {
        await using var streamWriter = File.CreateText(saveFilePath);
        await streamWriter.WriteAsync(JsonSerializer.Serialize(filesStates));
        
        // Explicitly set ".pseudogit" file as hidden
        File.SetAttributes(saveFilePath, FileAttributes.Hidden);
    }
}