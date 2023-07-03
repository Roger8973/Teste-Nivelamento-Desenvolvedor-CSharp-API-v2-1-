using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Questao2.Models;
using Questao2.Rest;
using Questao2.Rest.Interfaces;
using RestSharp;

namespace Questao2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var service = host.Services.GetRequiredService<Service>();

            await service.ExecuteAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IRestService, RestService>();
                    services.AddSingleton<Service>();
                });
    }

    public class Service
    {
        private readonly IRestService _restService;

        public Service(IRestService restServico)
        {
            _restService = restServico;
        }

        public async Task ExecuteAsync()
        {
            string teamName = "Paris Saint-Germain";
            int year = 2013;
            int totalGoals = await GetTotalScoredGoals(teamName, year);

            Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

            teamName = "Chelsea";
            year = 2014;
            totalGoals = await GetTotalScoredGoals(teamName, year);

            Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);
        }

        public async Task<int> GetTotalScoredGoals(string team, int year)
        {
            var page = 0; 
            var response = await _restService.GetAsync(team, year, page);
            var totalGoals = SumTeamGoals(response.Data);

            for (page = 1; page <= response.TotalPages; page++)
            {
                response = await _restService.GetAsync(team, year, page);
                totalGoals += SumTeamGoals(response.Data);
            }

            return totalGoals;
        }

        private static int SumTeamGoals(IEnumerable<ChampionshipDice> data)
            => data.Sum(x => x.Team1Goals);
    }
}
