using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using SpringComp.Functions.Extensions.StaticWebApps;
using SpringComp.Functions.Extensions.StaticWebApps.Bindings;

[assembly: WebJobsStartup(typeof(Startup))]
namespace SpringComp.Functions.Extensions.StaticWebApps
{
    /// <summary>
    /// Registers the <see cref="ClaimsPrincipalConfigProvider" /> extension.
    /// </summary>
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddStaticWebApps();
        }
    }
}
