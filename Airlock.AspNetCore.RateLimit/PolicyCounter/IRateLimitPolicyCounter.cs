using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Airlock.AspNetCore.RateLimit.ClientIdentifier;

namespace Airlock.AspNetCore.RateLimit.PolicyCounter
{
    public interface IRateLimitPolicyCounter
    {
        Task<RateLimitCacheEntry> CanAccessResource(string policyId, int limitPerHour, IClientIdentifier clientId);
    }
}
