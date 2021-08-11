using System;
using DotNetCore.CAP;
using DotNetCore.CAP.Transport;
using Microsoft.Extensions.DependencyInjection;
using CSRedis;

namespace DotNetCore.CAP.RedisMQ
{
    internal sealed class RedisMQCapOptionsExtension : ICapOptionsExtension
    {
        private readonly Action<RedisMQOptions> _configure;

        public RedisMQCapOptionsExtension(Action<RedisMQOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<CapMessageQueueMakerService>();
             
            services.Configure(_configure);

            var options = new RedisMQOptions();

            _configure?.Invoke(options);

            services.AddSingleton<CSRedisClient>(c =>
            {
                return new CSRedisClient(options.Servers);
            });
            services.AddSingleton<ITransport, RedisMQTransport>();
            services.AddSingleton<IConsumerClientFactory, RedisMQConsumerClientFactory>();
        }
    }
}