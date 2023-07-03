using Questao5.Domain.Enumerators;

namespace Questao5.Application
{
    public abstract class Response
    {
        public bool Success { get; set; }
        public ErrorResponse Error { get; set; }
    }

    public class ErrorResponse
    {
        public object Message { get; set; }
        public ErrorCodes ErrorCode { get; set; }
    }
}
