namespace Serilog
{
    using System;
    using Serilog.Configuration;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Formatting;
    using Serilog.Formatting.Display;
    using Serilog.Sinks.XUnit;
    using Xunit.Abstractions;

    public static class TestOutputLoggerConfigurationExtensions
    {
        const string DefaultConsoleOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}";

        /// <summary>
        /// Writes log events to <see cref="Xunit.Abstractions.ITestOutputHelper"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="testOutputHelper">The <see cref="Xunit.Abstractions.ITestOutputHelper"/> that will be written to.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink.
        /// the default is "{Timestamp} [{Level}] {Message}{NewLine}{Exception}".</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration TestOutput(
            this LoggerSinkConfiguration sinkConfiguration,
            ITestOutputHelper testOutputHelper,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultConsoleOutputTemplate,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null)
        {
            if(sinkConfiguration == null)
                throw new ArgumentNullException(nameof(sinkConfiguration));
            
            if(testOutputHelper == null)
                throw new ArgumentNullException(nameof(testOutputHelper));
            
            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);

            return sinkConfiguration.Sink(new TestOutputSink(testOutputHelper, formatter), restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// Writes log events to <see cref="Xunit.Abstractions.ITestOutputHelper"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="testOutputHelper">The <see cref="Xunit.Abstractions.ITestOutputHelper"/> that will be written to.</param>
        /// <param name="formatter">Controls the rendering of log events into text, for example to log JSON. To
        /// control plain text formatting, use the overload that accepts an output template.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration TestOutput(
            this LoggerSinkConfiguration sinkConfiguration,
            ITestOutputHelper testOutputHelper,
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if(sinkConfiguration == null)
                throw new ArgumentNullException(nameof(sinkConfiguration));
            
            if(formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            return sinkConfiguration.Sink(new TestOutputSink(testOutputHelper, formatter), restrictedToMinimumLevel, levelSwitch);
        }
    }
}