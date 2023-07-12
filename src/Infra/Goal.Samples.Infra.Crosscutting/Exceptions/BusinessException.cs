namespace Goal.Samples.Infra.Crosscutting.Exceptions;

public class BusinessException : ApplicationException
{
    public BusinessException(params string[] messages)
        : base("An business error occurs.")
    {
        Messages = messages;
    }

    public string[] Messages { get; }
}
