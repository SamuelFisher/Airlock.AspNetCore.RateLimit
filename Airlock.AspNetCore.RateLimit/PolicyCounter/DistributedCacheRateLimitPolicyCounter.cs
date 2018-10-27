using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Airlock.AspNetCore.RateLimit.ClientIdentifier;
using Microsoft.Extensions.Caching.Distributed;

namespace Airlock.AspNetCore.RateLimit.PolicyCounter
{
    public class DistributedCacheRateLimitPolicyCounter : IRateLimitPolicyCounter
    {
        private readonly IDistributedCache _cache;

        public DistributedCacheRateLimitPolicyCounter(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<RateLimitCacheEntry> CanAccessResource(string policyId, int limitPerHour, IClientIdentifier clientId)
        {
            var key = $"{policyId}:{clientId.IdentifierString}";
            var existing = await _cache.GetJsonAsync<RateLimitCacheEntry>(key) ?? new RateLimitCacheEntry
            {
                Remaining = limitPerHour,
                Resets = DateTime.UtcNow + TimeSpan.FromHours(1),
            };
            var updated = new RateLimitCacheEntry
            {
                Remaining = existing.Remaining - 1,
                Resets = existing.Resets,
            };
            await _cache.SetJsonAsync(key, updated, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = new DateTimeOffset(updated.Resets, TimeSpan.Zero),
            });
            return existing;
        }
    }
}
