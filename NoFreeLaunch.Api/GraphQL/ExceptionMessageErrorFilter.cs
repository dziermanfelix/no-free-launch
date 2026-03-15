namespace NoFreeLaunch.Api.GraphQL;

/// <summary>
/// Exposes the real exception message in GraphQL errors instead of "Unexpected Execution Error".
/// </summary>
public class ExceptionMessageErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception?.Message is { } message)
            return error.WithMessage(message);
        return error;
    }
}
