using System;
using System.Collections.Generic;
using System.Text;

namespace Airlock.AspNetCore.RateLimit.ClientIdentifier
{
    /// <summary>
    /// Identifies a user. This could be by their IP address, or an API key.
    /// </summary>
    public interface IClientIdentifier
    {
        string IdentifierString { get; }
    }
}
