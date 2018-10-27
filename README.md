Airlock.AspNetCore.RateLimit
============================

Rate limiting for ASP.NET Core MVC.

_This project has not been thoroughly tested. Please be careful before using it in production
applications._

This library is designed to provide simple rate limiting for MVC controller actions using a similar
mechanism to [GitHub's API rate limiting](https://developer.github.com/v3/#rate-limiting). The
features are limited to:

- Maximum number of requests can be specified per hour only
- The number of available requests is reset one hour after the first request to a resource
- Optionally adds `X-RateLimit-*` headers to the response
- Returns HTTP `429 Too Many Requests` when the rate limit is reached

This library provides an attribute that can be applied to MVC actions, so does not provide rate
limiting for middleware.

## Usage

Configure in `Startup`:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddRateLimit();
        ...
    }
}
```

Add the `RateLimit` attribute to actions:

```csharp
[ApiController]
[Route("api/controller")]
public class MyController : Controller
{
    // 10 requests per user per hour
    [RateLimit(10)]
    [HttpPost("resource")]
    public async Task<IActionResult> Resource(MyRequest request)
    {
        ...
    }
}
```

## Identifying Clients

By default, clients are identified by their IP address. This can be changed (for example, to
support API keys) by registering a custom `IClientIdentifierProvider`.

If your application is behind a reverse proxy, ensure you have
[configured](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-2.1)
ASP.NET Core to use the `fowarded-for` headers correctly.
