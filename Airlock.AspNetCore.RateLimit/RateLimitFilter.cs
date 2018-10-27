using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Airlock.AspNetCore.RateLimit.ClientIdentifier;
using Airlock.AspNetCore.RateLimit.PolicyCounter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Airlock.AspNetCore.RateLimit
{
    public class RateLimitFilter : IAsyncActionFilter
    {
        private readonly ILogger<RateLimitFilter> _logger;
        private readonly RateLimitOptions _options;
        private readonly IRateLimitPolicyCounter _counter;
        private readonly IClientIdentifierProvider _clientIdentifierProvider;
        private readonly int _limitPerHour;

        public RateLimitFilter(ILogger<RateLimitFilter> logger,
                               RateLimitOptions options,
                               IRateLimitPolicyCounter counter,
                               IClientIdentifierProvider clientIdentifierProvider,
                               int limitPerHour)
        {
            _logger = logger;
            _options = options;
            _counter = counter;
            _clientIdentifierProvider = clientIdentifierProvider;
            _limitPerHour = limitPerHour;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor == null)
                throw new NotSupportedException("Filtering other than by controller actions is not supported.");

            string policyId = $"Controller={actionDescriptor.ControllerName},Action={actionDescriptor.ActionName}";
            var clientId = _clientIdentifierProvider.GetClientIdentifier(context.HttpContext);

            var rateLimit = await _counter.CanAccessResource(policyId, _limitPerHour, clientId);

            if (_options.IncludeRateLimitHeaders)
            {
                context.HttpContext.Response.Headers.Add("X-RateLimit-Limit", _limitPerHour.ToString());
                context.HttpContext.Response.Headers.Add("X-RateLimit-Remaining", Math.Max(rateLimit.Remaining - 1, 0).ToString());
                context.HttpContext.Response.Headers.Add("X-RateLimit-Reset", rateLimit.Resets.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            }

            if (rateLimit.Remaining <= 0)
            {
                _logger.LogInformation($"Rate limit exceeded for policy '{policyId}', client '{clientId}'");
                context.Result = new StatusCodeResult(429);
                return;
            }

            await next();
        }
    }
}
