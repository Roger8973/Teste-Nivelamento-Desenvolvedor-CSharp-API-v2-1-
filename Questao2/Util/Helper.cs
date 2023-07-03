namespace Questao2.Util
{
    public static class Helper
    {
        public class Serialize
        {
            public static T StringToObject<T>(string value)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            }
        }

        public static bool IsValidResponse(System.Net.HttpStatusCode statuCode) => statuCode switch
        {
            System.Net.HttpStatusCode.OK => true,
            System.Net.HttpStatusCode.Created => true,
            System.Net.HttpStatusCode.BadRequest => throw new HttpRequestException("The request sent was malformed or incorrect."),
            System.Net.HttpStatusCode.NotFound => throw new HttpRequestException("The requested resource was not found."),
            System.Net.HttpStatusCode.Unauthorized => throw new HttpRequestException("Authentication is required or has failed."),
            System.Net.HttpStatusCode.Forbidden => throw new HttpRequestException("You do not have permission to access this resource."),
            System.Net.HttpStatusCode.RequestTimeout => throw new HttpRequestException("The request took too long to respond."),
            System.Net.HttpStatusCode.InternalServerError => throw new HttpRequestException("The server encountered an internal error."),
            System.Net.HttpStatusCode.ServiceUnavailable => throw new HttpRequestException("The service is temporarily unavailable."),
            _ => false,
        };
    }
}
