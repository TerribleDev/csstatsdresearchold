using Moq;
using StatsdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StatsdClientTests
{
    public class StatsdExtensionsTests
    {
        private Mock<IOutputChannel> _outputChannel;
        private Statsd _statsd;
        private TestData _testData;

        public void Initialise()
        {
            _outputChannel = new Mock<IOutputChannel>();
            _statsd = new Statsd("localhost", 12000, outputChannel: _outputChannel.Object);
            _testData = new TestData();
        }

        [Fact]
        public void count_SendToStatsd_Success()
        {
            Initialise();
            _outputChannel.Setup(p => p.SendAsync("foo.bar:1|c")).Verifiable();
            _statsd.count().foo.bar += 1;
            _outputChannel.VerifyAll();
        }

        [Fact]
        public void gauge_SendToStatsd_Success()
        {
            Initialise();
            _outputChannel.Setup(p => p.SendAsync("foo.bar:1|g")).Verifiable();
            _statsd.gauge().foo.bar += 1;
            _outputChannel.VerifyAll();
        }

        [Fact]
        public void timing_SendToStatsd_Success()
        {
            Initialise();
            _outputChannel.Setup(p => p.SendAsync("foo.bar:1|ms")).Verifiable();
            _statsd.timing().foo.bar += 1;
            _outputChannel.VerifyAll();
        }

        [Fact]
        public void count_AddNamePartAsString_Success()
        {
            Initialise();
            _outputChannel.Setup(p => p.SendAsync("foo.bar:1|ms")).Verifiable();
            _statsd.timing().foo._("bar")._ += 1;
            _outputChannel.VerifyAll();
        }
    }

}
