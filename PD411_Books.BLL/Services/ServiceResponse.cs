namespace PD411_Books.BLL.Services
{
    public class ServiceResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
        public object? Payload { get; set; } = null;
    }
}
