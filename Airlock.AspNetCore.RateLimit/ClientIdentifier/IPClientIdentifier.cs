using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Airlock.AspNetCore.RateLimit.ClientIdentifier
{
    class IPClientIdentifier : IClientIdentifier
    {
        public IPAddress IPAddress { get; }

        public IPClientIdentifier(IPAddress ipAddress)
        {
            IPAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
        }

        public string IdentifierString => $"IP={IPAddress}";

        public override string ToString() => IPAddress.ToString();
    }
}
