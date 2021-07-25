using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Logging;

namespace SpringComp.Functions.Extensions.StaticWebApps.Bindings
{
    internal class ClaimsPrincipalBinding : IBinding
    {
        private readonly UserInfoAttribute attribute_;
        private readonly ILogger logger_;

        /// <summary>
        /// Initialize a new instance of the <see cref="ClaimsPrincipalBinding" /> class.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="logger"></param>
        public ClaimsPrincipalBinding(UserInfoAttribute attribute, ILogger logger)
        {
            attribute_ = attribute;
            logger_ = logger;
        }

        public bool FromAttribute => true;

        /// <summary>
        /// This method is unused and throws an exception.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an instance of the <see cref="ClaimsPrincipalValueProvider" /> class
        /// responsible for retrieving static web apps user information from an incoming
        /// <see cref="HttpRequest" /> parameter to the function.
        ///
        /// This method does not currently supports other common parameter types
        /// such ash HttpRequestMessag<see cref="HttpRequest" /> parameter to the function.
        ///
        /// This method does not currently supports other common parameter types such as
        /// HttpRequestMessage.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            // The BindingContext object contains the following parameters:
            // - "$request" {Microsoft.AspNetCore.Http.DefaultHttpRequest}
            // - Query {IDictionary<String, StringValues>}
            // - Headers {IDictionary<String, StringValues>}
            // - req {Microsoft.AspNetCore.Http.DefaultHttpRequest}
            // - sys {Microsoft.Azure.WebJobs.Host.Bindings.SystemBindingData}

            var request = context.BindingData.ContainsKey("$request")
              ? context.BindingData["$request"] as HttpRequest
              : null
              ;

            if (request == null)
                throw new ApplicationException("Missing required HttpRequest parameter to the function.");

            return Task.FromResult<IValueProvider>(
              new FromHttpRequestClaimsPrincipalProvider(
                request,
                attribute_,
                logger_
                ));
        }

        public ParameterDescriptor ToParameterDescriptor()
          => new ParameterDescriptor();
    }
}
