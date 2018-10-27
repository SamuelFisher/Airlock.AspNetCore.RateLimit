using System;
using System.Collections.Generic;
using System.Text;
using Airlock.AspNetCore.RateLimit.ClientIdentifier;
using Airlock.AspNetCore.RateLimit.PolicyCounter;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Airlock.AspNetCore.RateLimit
{
    public class RateLimitAttribute : Attribute, IFilterFactory
    {
        private readonly int _limitPerHour;

        public RateLimitAttribute(int limitPerHour)
        {
            _limitPerHour = limitPerHour;
        }

        public bool IsReusable { get; } = false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filterLogger = serviceProvider.GetRequiredService<ILogger<RateLimitFilter>>();
            var options = serviceProvider.GetService<RateLimitOptions>();
            var counter = serviceProvider.GetRequiredService<IRateLimitPolicyCounter>();
            var clientIdentifierProvider = serviceProvider.GetRequiredService<IClientIdentifierProvider>();
            return new RateLimitFilter(filterLogger, options, counter, clientIdentifierProvider, _limitPerHour);
        }
    }
}
