using System;
using System.Collections.Generic;
using System.Text;

namespace Airlock.AspNetCore.RateLimit.PolicyCounter
{
    public class RateLimitCacheEntry
    {
        public int Remaining { get; set; }
        public DateTime Resets { get; set; }
    }
}
