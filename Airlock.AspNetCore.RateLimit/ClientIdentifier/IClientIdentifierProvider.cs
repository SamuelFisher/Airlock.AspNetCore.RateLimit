using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Airlock.AspNetCore.RateLimit.ClientIdentifier
{
    public interface IClientIdentifierProvider
    {
        IClientIdentifier GetClientIdentifier(HttpContext context);
    }
}
