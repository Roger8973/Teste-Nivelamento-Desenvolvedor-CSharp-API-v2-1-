using Questao2.Models;
using System.Net;

namespace Questao2.Rest
{
    public static class RestResponseExtensions
    {
        public static CommandResult ValidateResponse(HttpStatusCode response)
        {
            return response switch
            {
                HttpStatusCode.OK or HttpStatusCode.Created => new CommandResult(true),
                HttpStatusCode.BadRequest => new CommandResult(false, "The request sent was malformed or incorrect."),
                HttpStatusCode.NotFound => new CommandResult(false, "The requested resource was not found."),
                HttpStatusCode.Unauthorized => new CommandResult(false, "Authentication is required or has failed."),
                HttpStatusCode.Forbidden => new CommandResult(false, "You do not have permission to access this resource."),
                HttpStatusCode.RequestTimeout => new CommandResult(false, "The request took too long to respond."),
                HttpStatusCode.InternalServerError => new CommandResult(false, "The server encountered an internal error."),
                HttpStatusCode.ServiceUnavailable => new CommandResult(false, "The service is temporarily unavailable."),
                _ => new CommandResult(false, "An unexpected error occurred."),
            };
        }
    }
}
