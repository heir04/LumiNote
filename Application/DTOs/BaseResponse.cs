namespace api.Application.DTOs
{
    public class BaseResponse<T>
    {
        public string? Message { get; set; }
        public bool Status { get; set; }
        public T? Data { get; set; }
    }
}