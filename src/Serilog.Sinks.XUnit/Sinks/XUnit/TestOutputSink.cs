namespace Serilog.Sinks.XUnit
{
    using System;
    using System.IO;
    using Core;
    using Events;
    using Formatting;
    using Xunit.v3;

    /// <summary>
    /// A sink to direct Serilog output to the XUnit test output
    /// </summary>
    public class TestOutputSink : ILogEventSink
    {
        private readonly _IMessageSink _messageSink;
        private readonly _ITestOutputHelper _testOutputHelper;
        private readonly ITextFormatter _textFormatter;

        /// <summary>
        /// Creates a new instance of <see cref="TestOutputSink"/>
        /// </summary>
        /// <param name="messageSink">An <see cref="_IMessageSink"/> implementation that can be used to provide test output</param>
        /// <param name="textFormatter">The <see cref="ITextFormatter"/> used when rendering the message</param>
        public TestOutputSink(_IMessageSink messageSink, ITextFormatter textFormatter)
        {
            _messageSink = messageSink ?? throw new ArgumentNullException(nameof(messageSink));
            _textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }

        /// <summary>
        /// Creates a new instance of <see cref="TestOutputSink"/>
        /// </summary>
        /// <param name="testOutputHelper">An <see cref="_ITestOutputHelper"/> implementation that can be used to provide test output</param>
        /// <param name="textFormatter">The <see cref="ITextFormatter"/> used when rendering the message</param>
        public TestOutputSink(_ITestOutputHelper testOutputHelper, ITextFormatter textFormatter)
        {
            _testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
            _textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }

        /// <summary>
        /// Emits the provided log event from a sink 
        /// </summary>
        /// <param name="logEvent">The event being logged</param>
        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));

            var renderSpace = new StringWriter();
            _textFormatter.Format(logEvent, renderSpace);
            var message = renderSpace.ToString().Trim();
            _messageSink?.OnMessage(new _DiagnosticMessage { Message = message });
            _testOutputHelper?.WriteLine(message);
        }
    }
}
