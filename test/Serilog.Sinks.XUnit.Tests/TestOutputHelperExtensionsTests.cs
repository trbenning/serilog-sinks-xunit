namespace Serilog.Sinks.XUnit.Tests
{
    using System;
    using System.IO;
    using Core;
    using Events;
    using FluentAssertions;
    using Formatting;
    using NSubstitute;
    using Xunit;
    using Xunit.v3;

    public static class TestOutputHelperExtensionsTests
    {
        [Fact]
        public static void CreateTestLogger_WithDefaultParameters()
        {
            var outputMock = Substitute.For<_ITestOutputHelper>();
            var logger = outputMock.CreateTestLogger();

            const string message = "This is a test message";
            logger.Information(message);

            outputMock.Received(1).WriteLine(Arg.Is<string>(text => text.Contains($"[Information] {message}")));
        }

        [Fact]
        public static void CreateTestLogger_WithCustomMessageTemplate()
        {
            var outputMock = Substitute.For<_ITestOutputHelper>();
            var logger = outputMock.CreateTestLogger(outputTemplate: "Game of Thrones {Level}: {Message}");

            const string message = "That's what I do. I drink and I know things.";
            logger.Warning(message);

            outputMock.Received(1).WriteLine(Arg.Is<string>(text => text.Equals($"Game of Thrones Warning: {message}")));
        }

        [Fact]
        public static void CreateTestLogger_WithCustomMessageTemplateWithExtraNewline()
        {
            var outputMock = Substitute.For<_ITestOutputHelper>();
            var logger = outputMock.CreateTestLogger(outputTemplate: "Game of Thrones {Level}: {Message}{NewLine}{Exception}");

            const string message = "That's what I do. I drink and I know things.";
            logger.Warning(message);

            outputMock.Received(1).WriteLine(Arg.Is<string>(text => text.Equals($"Game of Thrones Warning: {message}")));
        }

        [Fact]
        public static void CreateTestLogger_WithCustomMinimumLogLevel()
        {
            var outputMock = Substitute.For<_ITestOutputHelper>();
            var logger = outputMock.CreateTestLogger(LogEventLevel.Error);

            const string message = "Foo";
            logger.Debug(message);
            logger.Information(message);
            logger.Warning(message);

            outputMock.DidNotReceive().WriteLine(Arg.Any<string>());
        }

        [Fact]
        public static void CreateTestLogger_WithCustomSwitch()
        {
            var outputMock = Substitute.For<_ITestOutputHelper>();
            var levelSwitch = new LoggingLevelSwitch(LogEventLevel.Warning);
            var logger = outputMock.CreateTestLogger(levelSwitch: levelSwitch);

            const string message = "Bar";
            logger.Verbose(message);
            logger.Debug(message);
            logger.Information(message);
            outputMock.DidNotReceive().WriteLine(Arg.Any<string>());

            levelSwitch.MinimumLevel = LogEventLevel.Information;

            logger.Information(message);

            outputMock.Received(1).WriteLine(Arg.Is<string>(text => text.Contains(message)));
        }

        [Fact]
        public static void CreateTestLogger_WithCustomTextFormatter()
        {
            var outputMock = Substitute.For<_ITestOutputHelper>();
            var textFormatter = Substitute.For<ITextFormatter>();
            var logger = outputMock.CreateTestLogger(textFormatter);

            const string message = "Baz";
            logger.Error(message);

            textFormatter.Received(1).Format(
                Arg.Is<LogEvent>(logEvent => logEvent.Level == LogEventLevel.Error && logEvent.MessageTemplate.Text == message),
                Arg.Any<StringWriter>());
        }

        [Fact]
        public static void CreateTestLogger_ShouldThrowIfTestOutputHelperIsNull()
        {
            Action act = () => default(_ITestOutputHelper).CreateTestLogger();

            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("testOutputHelper");
        }

        [Fact]
        public static void CreateTestLogger_ShouldThrowIfFormatterIsNull()
        {
            Action act = () => Substitute.For<_ITestOutputHelper>().CreateTestLogger(default(ITextFormatter));

            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("formatter");
        }
    }
}
