using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Airlock.AspNetCore.RateLimit.ClientIdentifier
{
    public class IPClientIdentifierProvider : IClientIdentifierProvider
    {
        public IClientIdentifier GetClientIdentifier(HttpContext context)
        {
            return new IPClientIdentifier(context.Connection.RemoteIpAddress);
        }
    }
}
