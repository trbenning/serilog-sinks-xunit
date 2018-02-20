namespace Serilog.Sinks.XUnit.Tests
{
    using System;
    using Xunit;
    using Xunit.Abstractions;

    public class Samples
    {
        readonly ILogger _log;

        public Samples(ITestOutputHelper output)
        {
            // Pass the ITestOutputHelper object to the TestOutput sink
            _log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(output, Events.LogEventLevel.Verbose)
                .CreateLogger()
                .ForContext<Samples>();
        }

        [Fact]
        [Trait("Category", "Integration")]
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
        [Trait("Category", "Integration")]
        public void VerifyDebugLevelOutput()
        {
            _log.Debug("Test output to Serilog!");
            // Check the test output window. You should see the above message.
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void VerifyVerboseLevelOutput()
        {
            _log.Verbose("Test output to Serilog!");
            // Check the test output window. You should see the above message.
        }
    }
}
