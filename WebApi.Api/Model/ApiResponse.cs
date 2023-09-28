namespace WebApi.Api.Model;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Result { get; set; }
}