using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.RedisMQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICapPublisher _capPublisher;
        public TestController(ICapPublisher capPublisher,
            ILogger<TestController> logger)
        {
            _capPublisher = capPublisher;
            _logger = logger;
        }

        [HttpGet("send1")]
        public async Task<IActionResult> Send1(int count=1)
        {
            var rng = new Random();
            for (var i = 0; i < count; i++)
            {
                await _capPublisher.PublishAsync($"send{i}", rng.Next());
            }
            return Ok();
        }

        [CapSubscribe("send0")]
        public void ShowWeb0(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }

        [CapSubscribe("send1")]
        public void ShowWeb1(int test)
        {
            _logger.LogInformation("【收到】"+test);

            //throw new Exception("测试错误");
        }
        [CapSubscribe("send2")]
        public void ShowWeb2(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }

        [CapSubscribe("send3")]
        public void ShowWeb3(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }

        [CapSubscribe("send4")]
        public void ShowWeb4(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }

        [CapSubscribe("send5")]
        public void ShowWeb5(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }

        [CapSubscribe("send6")]
        public void ShowWeb6(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }

        [CapSubscribe("send7")]
        public void ShowWeb7(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }

        [CapSubscribe("send8")]
        public void ShowWeb8(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }

        [CapSubscribe("send9")]
        public void ShowWeb9(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }

        [CapSubscribe("send10")]
        public void ShowWeb10(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }

        [CapSubscribe("send11")]
        public void ShowWeb12(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }

        [CapSubscribe("send13")]
        public void ShowWeb613(int test)
        {
            _logger.LogInformation("【收到】" + test);

            //throw new Exception("测试错误");
        }
    }
}
