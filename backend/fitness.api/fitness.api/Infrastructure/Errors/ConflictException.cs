namespace fitness.api.Infrastructure.Errors;

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}