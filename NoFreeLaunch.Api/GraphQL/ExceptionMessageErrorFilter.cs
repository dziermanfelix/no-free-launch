namespace NoFreeLaunch.Api.GraphQL;

public class ExceptionMessageErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception?.Message is { } message)
            return error.WithMessage(message);
        return error;
    }
}
