
using System.Data.SqlClient;
using System.Reflection;
using Dapper;

namespace TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Configuration.AddUserSecrets<Program>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var summaries = new[]
            {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

            app.MapGet("/test", () =>
            {
                try
                {
                    using var sql = new SqlConnection(Environment.GetEnvironmentVariable("APPSETTING_RDB_CONNECTION_STRING"));

                    var queried = sql.QueryAsync("SELECT TOP 10 * FROM Houses");

                    return null;
                }
                catch(Exception e)
                {
                    return e;
                }

                //return new TestResponse()
                //{
                //    Variables = Environment.GetEnvironmentVariables(),
                //    Rdb = Environment.GetEnvironmentVariable("APPSETTING_RDB_CONNECTION_STRING"),
                //    Issuer = Environment.GetEnvironmentVariable("APPSETTING_JWT_FIREBASE_VALID_ISSUER"),
                //    Audience = Environment.GetEnvironmentVariable("APPSETTING_JWT_FIREBASE_VALID_AUDIENCE"),

                //};
                //return Environment.GetEnvironmentVariables();
                //return builder.Configuration.AsEnumerable();
                //return new TestResponse()
                //{
                //    Rdb = builder.Configuration["db"],
                //    Issuer = builder.Configuration["Jwt:Firebase:ValidIssuer"],
                //    Audience = builder.Configuration["Jwt:Firebase:ValidAudience"],
                //};
            });

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

            app.Run();
        }
    }
}