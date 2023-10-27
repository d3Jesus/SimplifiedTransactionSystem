namespace ImprovedPicpay.Helpers;

public class Error
{
    public Error(string errorCode, string errorMessage)
    {
        Code = errorCode;
        Message = errorMessage;
    }

    public string Code { get; }
    public string Message { get; }
}
