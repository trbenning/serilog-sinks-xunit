using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Serilog.Sinks.XUnit.Tests
{
    public sealed class SampleFixture : IDisposable, IAsyncLifetime
    {
        private readonly ILogger _log;

        public SampleFixture(IMessageSink messageSink)
        {
            // Pass the IMessageSink object to the TestOutput sink
            _log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(messageSink, Events.LogEventLevel.Verbose)
                .CreateLogger()
                .ForContext<SampleFixture>();

            _log.Information("Sample fixture constructor called.");
            // Check the test output window. You should see the above message.
        }

        public void Dispose()
        {
            _log.Information("Sample fixture dispose called.");
            // Check the test output window. You should see the above message.
        }

        public Task InitializeAsync()
        {
            _log.Information("Sample fixture initialize async called.");
            // Check the test output window. You should see the above message.
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _log.Information("Sample fixture dispose async called.");
            // Check the test output window. You should see the above message.
            return Task.CompletedTask;
        }
    }
}
