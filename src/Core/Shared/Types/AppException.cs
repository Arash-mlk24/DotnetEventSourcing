namespace DotnetEventSourcing.src.Core.Shared.Types;

public class AppException(ServiceResult sr) : Exception
{
    public ServiceResult Result { get; private set; } = sr;
}