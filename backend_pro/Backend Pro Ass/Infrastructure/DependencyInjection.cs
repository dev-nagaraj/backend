using Application.Command;
using Application.Interfaces;
using Application.Query;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISalesService,SalesService>();
        services.AddScoped<SalesCommand>();
        services.AddScoped<SalesQuery>();
        return services.AddDbContext<BackendProContext>();
    }
}
