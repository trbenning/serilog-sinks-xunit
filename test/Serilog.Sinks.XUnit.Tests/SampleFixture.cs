namespace Serilog.Sinks.XUnit.Tests
{
    using System;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.v3;

    public sealed class SampleFixture : IDisposable, IAsyncLifetime
    {
        private readonly ILogger _log;

        public SampleFixture(_IMessageSink messageSink)
        {
            // Pass the _IMessageSink object to the TestOutput sink
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

        public ValueTask InitializeAsync()
        {
            _log.Information("Sample fixture initialize async called.");
            // Check the test output window. You should see the above message.
            return default;
        }

        public ValueTask DisposeAsync()
        {
            _log.Information("Sample fixture dispose async called.");
            // Check the test output window. You should see the above message.
            return default;
        }
    }
}
