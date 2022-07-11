using System.Text;
using System.Text.Json;
using CheckMyDirs.Api.Exceptions;
using CheckMyDirs.Api.Helpers;
using CheckMyDirs.Api.Models;
using CheckMyDirs.Api.Constants;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace CheckMyDirs.Api.Features;

public class PreviousFilesStatesHandler
{
    public List<PreviousFileStateType>? TryGetPreviousFilesStates(string path)
    {
        // Check if ".pseudogit" exists
        var dotPseudogit = PathHelpers.GetDotPseudogit(path);

        if (!File.Exists(dotPseudogit)) return null;
        
        // Try get previous states entities from ".pseudogit" file 
        try
        {
            using var streamReader = new StreamReader(dotPseudogit, Encoding.UTF8);
            var dotPseudogitContent = streamReader.ReadToEnd();

            var previousFileStates = JsonSerializer
                .Deserialize<List<PreviousFileStateType>>(dotPseudogitContent);
            
            return previousFileStates ?? null;
        }
        catch
        {
            // Intentional original exception absorbing
            throw new InvalidPreviousStatesFileParsingException(
                "Previous states record file reading or parsing has failed.");
        }
    }
}