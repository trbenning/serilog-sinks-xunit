using System.Threading.Tasks;

namespace Serilog.Sinks.XUnit.Tests
{
    using System;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class SampleTests : IDisposable, IAsyncLifetime, IClassFixture<SampleFixture>
    {
        private readonly ILogger _log;

        public SampleTests(SampleFixture fixture, ITestOutputHelper output)
        {
            // Pass the ITestOutputHelper object to the TestOutput sink
            _log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(output, Events.LogEventLevel.Verbose)
                .CreateLogger()
                .ForContext<SampleTests>();

            _log.Information("Sample test constructor called.");
            // Check the test output window. You should see the above message.
        }

        public void Dispose()
        {
            _log.Information("Sample test dispose called.");
            // Check the test output window. You should see the above message.
        }

        public Task InitializeAsync()
        {
            _log.Information("Sample test initialize async called.");
            // Check the test output window. You should see the above message.
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _log.Information("Sample test dispose async called.");
            // Check the test output window. You should see the above message.
            return Task.CompletedTask;
        }

        [Fact]
        [Trait("Category", "Sample")]
        public void ExampleUsage()
        {
            // Use ILogger as you normally would. These messages will show up in the test output
            _log.Information("Test output to Serilog!");

            Action sketchy = () => throw new Exception("I threw up.");
            var exception = Record.Exception(sketchy);

            _log.Error(exception, "Here is an error.");
            Assert.NotNull(exception);
        }

        [Fact]
        [Trait("Category", "Sample")]
        public void VerifyDebugLevelOutput()
        {
            _log.Debug("Test output to Serilog!");
            // Check the test output window. You should see the above message.
        }

        [Fact]
        [Trait("Category", "Sample")]
        public void VerifyVerboseLevelOutput()
        {
            _log.Verbose("Test output to Serilog!");
            // Check the test output window. You should see the above message.
        }
    }
}
