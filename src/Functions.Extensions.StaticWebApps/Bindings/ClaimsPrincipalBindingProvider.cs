using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;

namespace SpringComp.Functions.Extensions.StaticWebApps.Bindings
{
    internal class ClaimsPrincipalBindingProvider : IBindingProvider
    {
        private readonly ILogger logger_;

        /// <summary>
        /// Initialize a new instance of the <see cref="ClaimsPrincipalBindingProvider" /> class.
        /// This class is responsible for creating the corresponding <see cref="ClaimsPrincipalBinding" /> object.
        /// </summary>
        /// <param name="logger"></param>
        public ClaimsPrincipalBindingProvider(ILogger logger)
        {
            logger_ = logger;
        }

        /// <summary>
        /// Creates the <see cref="ClaimsPrincipalBinding" /> class.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<UserInfoAttribute>(inherit: false);

            IBinding binding = new ClaimsPrincipalBinding(attribute, logger_);
            return Task.FromResult(binding);
        }
    }
}
