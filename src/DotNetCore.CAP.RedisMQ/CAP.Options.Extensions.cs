
using System;
using DotNetCore.CAP;
using DotNetCore.CAP.RedisMQ;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CapOptionsExtensions
    {
        public static CapOptions UseRedisMQ(this CapOptions options, string servers)
        {
            return options.UseRedisMQ(opt => { opt.Servers = servers; });
        }

        public static CapOptions UseRedisMQ(this CapOptions options, Action<RedisMQOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new RedisMQCapOptionsExtension(configure));

            return options;
        }
    }
}