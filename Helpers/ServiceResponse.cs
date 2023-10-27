
namespace ImprovedPicpay.Helpers;

public class ServiceResponse<T>
{
    public ServiceResponse()
    {
        
    }
    public ServiceResponse(T value)
    {
        Data = value;
    }
    public ServiceResponse(bool succeeded, Error error)
    {
        Succeeded = succeeded;
        Error = error;
    }
    public T Data { get; set; }
    public bool Succeeded { get; set; } = true;
    public bool IsFailure => !Succeeded;
    public string Message { get; set; } = string.Empty;
    public Error Error { get; }

    public static implicit operator T(ServiceResponse<T> result) => result.Data;
    public static implicit operator ServiceResponse<T>(T value) => new ServiceResponse<T>(value);

}

public class ServiceResponse
{
    public static ServiceResponse<T> Failure<T>(Error error)
    {
        return new ServiceResponse<T>(false, error);
    }
}
