using Microsoft.Extensions.DependencyInjection;

namespace UpdateClientService.API.Services.ProxyApi
{
    public static class ProxyApiExtensions
    {
        public static IServiceCollection AddProxyApi(this IServiceCollection services)
        {
            return services.AddScoped<IProxyApi, ProxyApi>();
        }
    }
}