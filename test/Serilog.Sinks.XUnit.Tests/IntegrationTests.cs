namespace Serilog.Sinks.XUnit.Tests
{
    using System;
    using Xunit;
    using Xunit.Abstractions;

    public class IntegrationTests
    {
        ILogger _output;

        public IntegrationTests(ITestOutputHelper output)
        {
            // Pass the ITestOutputHelper object to the TestOutput sink
            _output = new LoggerConfiguration()
                .WriteTo.TestOutput(output, Events.LogEventLevel.Verbose)
                .CreateLogger()
                .ForContext<IntegrationTests>();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void ExampleUsage()
        {
            // Use ILogger as you normally would. These messages will show up in the test output
            _output.Information("Test output to Serilog!");

            Action sketchy = () => throw new Exception("I threw up.");
            var exception = Record.Exception(sketchy);

            _output.Error(exception, "Here is an error.");
            Assert.NotNull(exception);
        }
    }
}
