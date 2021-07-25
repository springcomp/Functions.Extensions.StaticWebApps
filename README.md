# Functions.Extensions.StaticWebApps

Azure Functions extension that extracts user information for use in a Static Web Apps api.

## Install

This extension is available as a NuGet package.

```pwsh
dotnet install SpringComp.Functions.Extensions.StaticWebApps
```

## Usage

This extension is designed to work well with azure functions used as backend apis for Azure Static Web Apps.
It implements the [standard code](https://docs.microsoft.com/en-us/azure/static-web-apps/user-information?tabs=csharp) to retrieve the authenticated user details as a ClaimsPrincipal object.

In your HTTP triggered api function add a parameter typed [ClaimsPrincipal](https://docs.microsoft.com/en-us/dotnet/api/system.security.claims.claimsprincipal?view=net-5.0) decorated using the [UserInfo](https://github.com/springcomp/Functions.Extensions.StaticWebApps/blob/master/src/Functions.Extensions.StaticWebApps/UserInfo.cs) attribute.

```c#
    [FunctionName("HttpTrigger")]
    public async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "message")] HttpRequest req,
        [UserInfo] ClaimsPrincipal identity,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      var dictionary = new Dictionary<string, object>();

      foreach (var claim in identity.Claims)
      {
        log.LogTrace($"{claim.Type}: {claim.Value}");
        if (dictionary.ContainsKey(claim.Type))
        {
          var items = dictionary[claim.Type] as List<string>;
          if (items == null)
            items = new List<string>(new[] { dictionary[claim.Type] as string });
          items.Add(claim.Value);
          dictionary[claim.Type] = items;
        }
        else
        {
          dictionary.Add(claim.Type, claim.Value);
        }
      }

      string output;
      using (var stream = new MemoryStream())
      {
        await System.Text.Json.JsonSerializer.SerializeAsync(stream, dictionary);
        output = Encoding.UTF8.GetString(stream.ToArray());
        log.LogTrace(output);
      }

      var response = new HttpResponseMessage();
      response.Content = new StringContent(output, Encoding.UTF8, "application/json");
      return response;
    }
```