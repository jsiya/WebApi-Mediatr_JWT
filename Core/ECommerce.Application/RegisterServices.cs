using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application;

public static class RegisterServices
{
    public static void AddApplicationRegister(this IServiceCollection services)
    {
        services.AddMediatR(p=>p.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }
}