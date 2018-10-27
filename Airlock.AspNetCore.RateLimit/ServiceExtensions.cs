using System;
using System.Collections.Generic;
using System.Text;
using Airlock.AspNetCore.RateLimit.ClientIdentifier;
using Airlock.AspNetCore.RateLimit.PolicyCounter;
using Microsoft.Extensions.DependencyInjection;

namespace Airlock.AspNetCore.RateLimit
{
    public static class ServiceExtensions
    {
        public static void AddRateLimit(this IServiceCollection services)
        {
            // Use default options
            services.AddRateLimit(options => { });
        }

        public static void AddRateLimit(this IServiceCollection services, Action<RateLimitOptions> options)
        {
            var rateLimitOptions = new RateLimitOptions();
            options(rateLimitOptions);
            services.AddSingleton(rateLimitOptions);

            services.AddSingleton<IRateLimitPolicyCounter, DistributedCacheRateLimitPolicyCounter>();
            services.AddSingleton<IClientIdentifierProvider, IPClientIdentifierProvider>();
        }
    }
}
