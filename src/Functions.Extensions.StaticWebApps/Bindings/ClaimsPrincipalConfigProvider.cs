using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Logging;

namespace SpringComp.Functions.Extensions.StaticWebApps.Bindings
{
    internal class ClaimsPrincipalConfigProvider : IExtensionConfigProvider
    {
        private readonly ILogger logger_;

        /// <summary>
        /// Initialize a new instance of the <see cref="ClaimsPrincipalConfigProvider" /> class.
        /// This class is responsible for establishing a link from the <see cref="ClaimsPrincipalAttribute" /> attribute
        /// and its <see cref="ClaimsPrincipalBindingProvider" /> factory.
        /// </summary>
        /// <param name="logger"></param>
        public ClaimsPrincipalConfigProvider(ILogger<ClaimsPrincipalConfigProvider> logger)
          : this((ILogger)logger)
        {
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="ClaimsPrincipalConfigProvider" /> class.
        /// This class is responsible for establishing a link from the <see cref="ClaimsPrincipalAttribute" /> attribute
        /// and its <see cref="ClaimsPrincipalBindingProvider" /> factory.
        /// </summary>
        /// <param name="logger"></param>
        public ClaimsPrincipalConfigProvider(ILogger logger)
        {
            logger_ = logger;
        }

        /// <summary>
        /// Creates the binding rule for the <see cref="ClaimsPrincipalAttribute" /> attribute.
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(ExtensionConfigContext context)
        {
            var rule = context.AddBindingRule<UserInfoAttribute>();
            rule.Bind(new ClaimsPrincipalBindingProvider(logger_));
        }
    }
}
