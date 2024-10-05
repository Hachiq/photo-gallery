using System.Net;

namespace Core.ExceptionHandling;

public class HandledExceptionModel
{
    public HttpStatusCode Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}
