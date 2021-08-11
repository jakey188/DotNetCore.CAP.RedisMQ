using CSRedis;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Transport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCore.CAP.RedisMQ
{
    public class RedisMQConsumerClient : IConsumerClient
    {
        private readonly RedisMQOptions _redisOptions;
        private readonly CSRedisClient _redis;
        private readonly List<string> _topicList;
        private readonly string _groupId;
        private readonly ILogger _logger;
        public RedisMQConsumerClient(string groupId,
            CSRedisClient redis, 
            IOptions<RedisMQOptions> options,
            ILogger logger)
        {
            _groupId = groupId;
            _redisOptions = options.Value ?? throw new ArgumentNullException(nameof(options));
            _redis = redis;
            _topicList = new List<string>();
            _logger = logger;
        }
        public BrokerAddress BrokerAddress => new BrokerAddress("Redis", _redisOptions.Servers);

        public event EventHandler<TransportMessage> OnMessageReceived;
        public event EventHandler<LogMessageEventArgs> OnLog;

        public void Subscribe(IEnumerable<string> topics)
        {
            if (topics == null) throw new ArgumentNullException(nameof(topics));

            foreach (var topic in topics)
            {
                _topicList.Add(topic);
            }
        }

        public void Listening(TimeSpan timeout, CancellationToken cancellationToken)
        {
            foreach (var name in _topicList)
            {
                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        await Consume(name, cancellationToken);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                    }
                }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        private async Task Consume(string topic, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var msg =  _redis.BRPop(5, topic);

                    if (msg != null)
                    {
                        var message = JsonConvert.DeserializeObject<TransportMessage>(msg);

                        message.Headers.Add(Headers.Group, _groupId);

                        OnMessageReceived?.Invoke(message.GetId(), message);
                    }
                    else
                    {
                        await Task.Delay(10);
                    }
                }
                catch (Exception ex)
                {
                    OnLog?.Invoke(this, new LogMessageEventArgs()
                    {
                        LogType = MqLogType.ExceptionReceived,
                        Reason = $"{_groupId}-{topic}-{ex.Message}"
                    });
                }
            }
        }


        public void Commit(object sender)
        {
            //_channel.BasicAck((ulong)sender, false);
        }

        public void Reject(object sender)
        {
            //_channel.BasicReject((ulong)sender, true);
        }

        public void Dispose()
        {
            
        }
    }
}
