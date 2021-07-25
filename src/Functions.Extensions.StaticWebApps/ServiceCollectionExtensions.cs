using System;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using SpringComp.Functions.Extensions.StaticWebApps.Bindings;

namespace SpringComp.Functions.Extensions.StaticWebApps
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStaticWebApps(this IServiceCollection services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            services
              .AddWebJobs(x => { return; })
              .AddExtension<ClaimsPrincipalConfigProvider>()
              ;

            return services;
        }

        public static IWebJobsBuilder AddStaticWebApps(this IWebJobsBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddStaticWebApps();

            return builder;
        }
    }
}
