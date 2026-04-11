using Limak.API.Middlewares;
using Limak.Application.Dtos.ResultDtos;
using Limak.Application.ServiceRegistrations;
using Limak.Infrastructure.ServiceRegistrations;
using Limak.Persistence.Abstractions;
using Limak.Persistence.ServiceRegistrations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Limak.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value!.Errors.Count > 0)
                        .Select(x => new
                        {
                            Errors = x.Value!.Errors.Select(e => e.ErrorMessage)
                        });

                    ResultDto resultDto = new()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = string.Join(", ", errors.SelectMany(x => x.Errors))
                    };

                    return new BadRequestObjectResult(resultDto);
                };
            }); ;

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMVC",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:7001")
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });

            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddApplicationService();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionHandler>();

            var scope = app.Services.CreateScope();

            var initalizer = scope.ServiceProvider.GetRequiredService<IContextInitalizer>();

            await initalizer.InitDatabaseAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowMVC");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
