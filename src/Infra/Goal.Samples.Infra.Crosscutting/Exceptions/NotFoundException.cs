namespace Goal.Samples.Infra.Crosscutting.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}
