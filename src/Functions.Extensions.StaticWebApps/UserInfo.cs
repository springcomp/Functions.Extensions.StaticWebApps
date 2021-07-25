using System;

using Microsoft.Azure.WebJobs.Description;

namespace SpringComp.Functions.Extensions.StaticWebApps
{
    /// <summary>
    /// This attribute decorates a <see cref="ClaimsPrincipal" /> parameter
    /// to the function so that it gets populated with the identity of
    /// the calling user automatically.
    ///
    /// Even though a <see ref="ClaimsPrincipal" /> object is natively
    /// supported in azure functions, the one supplied by default does not
    /// take into account peculiarities from the static web apps.
    ///
    /// https://docs.microsoft.com/en-us/azure/static-web-apps/user-information?tabs=csharp
    ///
    /// This attribute enables retrieval to the user information supplied
    /// by the static web apps runtime.
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class UserInfoAttribute : Attribute
    {
    }
}
