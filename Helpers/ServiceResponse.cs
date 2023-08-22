namespace ImprovedPicpay.Helpers;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Succeeded { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}
