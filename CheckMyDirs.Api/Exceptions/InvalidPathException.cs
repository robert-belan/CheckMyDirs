namespace CheckMyDirs.Api.Exceptions;

public class InvalidPathException : Exception
{
    public InvalidPathException(string message) : base(message)
    {
        
    }
}