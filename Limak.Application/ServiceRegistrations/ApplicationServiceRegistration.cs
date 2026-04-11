using FluentValidation;
using FluentValidation.AspNetCore;
using Limak.Application.Validators.UserValidators;
using Microsoft.Extensions.DependencyInjection;

namespace Limak.Application.ServiceRegistrations;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationService(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
        services.AddAutoMapper(_ => { }, typeof(ApplicationServiceRegistration).Assembly);

    }
}
