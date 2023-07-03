using Microsoft.Extensions.Configuration;
using Questao2.Models;
using Questao2.Rest.Interfaces;
using Questao2.Util;
using RestSharp;

namespace Questao2.Rest
{
    public class RestService : IRestService
    {
        private readonly RestClient _client;
        public RestService(IConfiguration configuration)
        {
            _client = new RestClient(configuration.GetSection("UriApi").Value);
        }

        public async Task<Response> GetAsync(string team, int year, int page)
        {
            var request = new RestRequest($"?year={year}&team1={team}&page={page}", Method.Get);

            var result = await _client.ExecuteAsync<Response>(request);

            var validationResult = RestResponseExtensions.ValidateResponse(result.StatusCode);

            if (!validationResult.Success)
                throw new HttpRequestException(validationResult.ErrorMessage);

            return Helper.Serialize.StringToObject<Response>(result.Content);
        }
    }
}
