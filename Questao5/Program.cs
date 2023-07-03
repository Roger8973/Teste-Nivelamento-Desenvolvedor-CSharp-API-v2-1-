using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.OpenApi.Models;
using Questao5.Application.Validators;
using Questao5.Application.Validators.Interfaces;
using Questao5.Domain.Extensions;
using Questao5.Infrastructure.Dapper;
using Questao5.Infrastructure.Dapper.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Requests.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests.Interfaces;
using Questao5.Infrastructure.Filters;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateAttribute>();

}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new ResponseConverterExtensions());
}); ;


builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#region Commands
builder.Services.AddTransient<IMovementAccountCommandRepository, MovementAccountCommandRepository>();
builder.Services.AddTransient<IIdempotencyCommandRepository, IdempotencyCommandRepository>();
#endregion

#region Queries
builder.Services.AddTransient<IIdempotencyQueryRepository, IdempotencyQueryRepository>();
builder.Services.AddTransient<IAccountQueryRepository, AccountQueryRepository>();
#endregion

#region Services
builder.Services.AddScoped<IGetAccountValidationService, GetAccountValidationService>();
builder.Services.AddScoped<ICreateMovementAccountValidationService, CreateMovementAccountValidationService>();
#endregion

builder.Services.AddTransient<IDatabase, Database>();
builder.Services.AddTransient<IDbConnection>((sp) =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new SqliteConnection(configuration.GetConnectionString("DatabaseName"));
});


// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Banck.Account.Api",
        Version = "v1",
    });
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


