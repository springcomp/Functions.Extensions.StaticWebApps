using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;

namespace SpringComp.Functions.Extensions.StaticWebApps.Bindings
{
    /// <summary>
    /// This class is parses the incoming <see cref="HttpRequest" /> function parameter
    /// and extracts the corresponding user information as a <see cref="ClaimsPrincipal" /> object.
    /// </summary>
    internal class FromHttpRequestClaimsPrincipalProvider : IValueProvider
    {
        private readonly HttpRequest request_;
        private readonly UserInfoAttribute attribute_;
        private readonly ILogger logger_;

        public FromHttpRequestClaimsPrincipalProvider(HttpRequest request, UserInfoAttribute attribute, ILogger logger)
        {
            request_ = request;
            attribute_ = attribute;
            logger_ = logger;
        }

        public Type Type => typeof(ClaimsPrincipal);

        public Task<object> GetValueAsync()
        {

            var claimsPrincipal = Parse(request_);
            return Task.FromResult((object)claimsPrincipal);
        }

        public string ToInvokeString()
          => string.Empty;

        // the following code is taken from
        // https://docs.microsoft.com/en-us/azure/static-web-apps/user-information?tabs=csharp

        private class ClientPrincipal
        {
            public string IdentityProvider { get; set; }
            public string UserId { get; set; }
            public string UserDetails { get; set; }
            public IEnumerable<string> UserRoles { get; set; }
        }

        private static ClaimsPrincipal Parse(HttpRequest req)
        {
            // backup the original identity information

            var claimsPrincipal = req.HttpContext.User;

            // attempt to retrieve Static Web Apps user information

            var principal = new ClientPrincipal();

            if (req.Headers.TryGetValue("x-ms-client-principal", out var header))
            {
                var data = header[0];
                var decoded = Convert.FromBase64String(data);
                var json = Encoding.ASCII.GetString(decoded);
                var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                principal = JsonSerializer.Deserialize<ClientPrincipal>(json, serializerOptions);
            }

            principal.UserRoles = principal.UserRoles?.Except(new string[] { "anonymous" }, StringComparer.CurrentCultureIgnoreCase);

            if (!principal.UserRoles?.Any() ?? true)
            {
                return claimsPrincipal;
            }

            var identity = new ClaimsIdentity(principal.IdentityProvider);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId));
            identity.AddClaim(new Claim(ClaimTypes.Name, principal.UserDetails));
            identity.AddClaims(principal.UserRoles.Select(r => new Claim(ClaimTypes.Role, r)));

            return new ClaimsPrincipal(identity);
        }
    }
}
