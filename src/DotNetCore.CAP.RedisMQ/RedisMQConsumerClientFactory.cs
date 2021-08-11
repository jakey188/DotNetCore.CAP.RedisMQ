using CSRedis;
using DotNetCore.CAP.Transport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.CAP.RedisMQ
{
    public class RedisMQConsumerClientFactory : IConsumerClientFactory
    {
        private readonly IOptions<RedisMQOptions> _options;
        private readonly ILoggerFactory _loggerFactory;
        private readonly CSRedisClient _redis;
        public RedisMQConsumerClientFactory(IOptions<RedisMQOptions> options,
            ILoggerFactory loggerFactory,
            CSRedisClient redis)
        {
            _options = options;
            _loggerFactory = loggerFactory;
            _redis = redis;
        }

        public IConsumerClient Create(string groupId)
        {
            var logger = _loggerFactory.CreateLogger(typeof(RedisMQConsumerClient));
            return new RedisMQConsumerClient(groupId, _redis, _options, logger);
        }
    }
}
