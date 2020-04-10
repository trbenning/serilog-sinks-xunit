using Xunit.Sdk;

namespace Serilog.Sinks.XUnit.Tests
{
    using FluentAssertions;
    using NSubstitute;
    using Events;
    using Formatting;
    using Parsing;
    using System;
    using System.IO;
    using Xunit;
    using Xunit.Abstractions;

    public class TestOutputSinkTests
    {
        [Fact]
        public void Constructor_ForMessageSink_ShouldThrowIfMessageSinkIsNull()
        {
            var ex = Record.Exception(() => new TestOutputSink((IMessageSink)null, Substitute.For<ITextFormatter>()));
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_ForMessageSink_ShouldThrowIfTextFormatterIsNull()
        {
            var ex = Record.Exception(() => new TestOutputSink(Substitute.For<IMessageSink>(), null));
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_ForTestOutputHelper_ShouldThrowIfTestOutputHelperIsNull()
        {
            var ex = Record.Exception(() => new TestOutputSink((ITestOutputHelper)null, Substitute.For<ITextFormatter>()));
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_ForTestOutputHelper_ShouldThrowIfTextFormatterIsNull()
        {
            var ex = Record.Exception(() => new TestOutputSink(Substitute.For<ITestOutputHelper>(), null));
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void Emit_ShouldThrowIfEventIsNull()
        {
            var underTest = new TestOutputSink(
                Substitute.For<ITestOutputHelper>(),
                Substitute.For<ITextFormatter>());

            var ex = Record.Exception(() => underTest.Emit(null));

            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void Emit_ForMessageSink_ShouldWriteFormattedEventToTestOutput()
        {
            var messageSink = Substitute.For<IMessageSink>();
            var formatter = Substitute.For<ITextFormatter>();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(messageSink, formatter)
                .CreateLogger();

            const string message = "Hello";
            logger.Information(message);

            messageSink.Received(1).OnMessage(Arg.Any<DiagnosticMessage>());

            formatter.Received(1).Format(
                Arg.Is<LogEvent>(ev => ev.Level == LogEventLevel.Information && ev.MessageTemplate.Text == message),
                Arg.Any<StringWriter>());
        }

        [Fact]
        public void Emit_ForTestOutputHelper_ShouldWriteFormattedEventToTestOutput()
        {
            var outputHelper = Substitute.For<ITestOutputHelper>();
            var formatter = Substitute.For<ITextFormatter>();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(outputHelper, formatter)
                .CreateLogger();

            const string message = "Hello";
            logger.Information(message);

            outputHelper.Received(1).WriteLine(Arg.Any<string>());

            formatter.Received(1).Format(
                Arg.Is<LogEvent>(ev => ev.Level == LogEventLevel.Information && ev.MessageTemplate.Text == message),
                Arg.Any<StringWriter>());
        }

        [Theory]
        [InlineData(LogEventLevel.Debug, new[] { LogEventLevel.Verbose })]
        [InlineData(LogEventLevel.Information, new[] { LogEventLevel.Verbose, LogEventLevel.Debug })]
        [InlineData(LogEventLevel.Warning, new[] { LogEventLevel.Verbose, LogEventLevel.Debug, LogEventLevel.Information })]
        [InlineData(LogEventLevel.Error, new[] { LogEventLevel.Verbose, LogEventLevel.Debug, LogEventLevel.Information, LogEventLevel.Warning })]
        [InlineData(LogEventLevel.Fatal, new[] { LogEventLevel.Verbose, LogEventLevel.Debug, LogEventLevel.Information, LogEventLevel.Warning, LogEventLevel.Error })]
        public void Emit_ShouldHonorTheRestrictedToMinimumLevelParameter(LogEventLevel minLevel, LogEventLevel[] levelsToWrite)
        {
            var outputHelper = Substitute.For<ITestOutputHelper>();
            var formatter = Substitute.For<ITextFormatter>();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(outputHelper, formatter, minLevel)
                .CreateLogger();

            const string message = "Hello";
            foreach (var level in levelsToWrite)
            {
                logger.Write(level, message);
            }

            outputHelper.DidNotReceive().WriteLine(Arg.Any<string>());

            formatter.DidNotReceive().Format(
                Arg.Any<LogEvent>(),
                Arg.Any<TextWriter>());
        }

        [Fact]
        public void Emit_ShouldWriteDebugEventWhenMinimumLevelSetToDebug()
        {
            var outputHelper = Substitute.For<ITestOutputHelper>();
            var formatter = Substitute.For<ITextFormatter>();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(outputHelper, formatter, LogEventLevel.Debug)
                .CreateLogger();

            const string message = "Hello";
            logger.Debug(message);

            outputHelper.Received(1).WriteLine(Arg.Any<string>());

            formatter.Received(1).Format(
                Arg.Is<LogEvent>(ev => ev.Level == LogEventLevel.Debug && ev.MessageTemplate.Text == message),
                Arg.Any<StringWriter>());
        }
    }
}
