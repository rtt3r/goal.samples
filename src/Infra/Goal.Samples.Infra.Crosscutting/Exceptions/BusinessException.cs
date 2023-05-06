namespace Goal.Samples.Infra.Crosscutting.Exceptions;

public class BusinessException : ApplicationException
{
    public BusinessException(string message)
        : base(message)
    {
    }
}
