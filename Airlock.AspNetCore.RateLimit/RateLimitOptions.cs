using System;
using System.Collections.Generic;
using System.Text;

namespace Airlock.AspNetCore.RateLimit
{
    public class RateLimitOptions
    {
        public bool IncludeRateLimitHeaders { get; set; } = true;
    }
}
