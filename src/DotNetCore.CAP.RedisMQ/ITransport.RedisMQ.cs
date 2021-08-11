using CSRedis;
using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Transport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.CAP.RedisMQ
{
    public class RedisMQTransport : ITransport
    {
        private readonly RedisMQOptions _redisOptions;
        private readonly ILogger _logger;
        private readonly CSRedisClient _redis;
        public RedisMQTransport(
            IOptions<RedisMQOptions> options,
            ILogger<RedisMQTransport> logger,
            CSRedisClient redis)
        {
            _redisOptions = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;
            _redis = redis;
        }

        public BrokerAddress BrokerAddress => new BrokerAddress("Redis", _redisOptions.Servers);

        public async Task<OperateResult> SendAsync(TransportMessage message)
        {
            try
            {
                var name = message.GetName();

                var json = JsonConvert.SerializeObject(message);

                await _redis.LPushAsync(name, json);

                _logger.LogDebug($"Event message [{message.GetName()}] has been published.");

                return OperateResult.Success;
            }
            catch (Exception ex)
            {
                var wrapperEx = new PublisherSentFailedException(ex.Message, ex);

                return OperateResult.Failed(wrapperEx);
            }
        }
    }
}
