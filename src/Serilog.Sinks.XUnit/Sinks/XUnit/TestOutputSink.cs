namespace Serilog.Sinks.XUnit
{
    using System;
    using System.IO;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Formatting;
    using Xunit.Abstractions;

    public class TestOutputSink : ILogEventSink
    {
        readonly ITestOutputHelper _testOutputHelper;
        readonly ITextFormatter _textFormatter;

        public TestOutputSink(ITestOutputHelper testOutputHelper, ITextFormatter textFormatter)
        {
            if(testOutputHelper == null)
                throw new ArgumentNullException(nameof(testOutputHelper));

            if(textFormatter == null)
                throw new ArgumentNullException(nameof(textFormatter));
                
            _testOutputHelper = testOutputHelper;
            _textFormatter = textFormatter;
        }

        public void Emit(LogEvent logEvent)
        {
            if(logEvent == null)
                throw new ArgumentNullException(nameof(logEvent));
            
            var renderSpace = new StringWriter();
            _textFormatter.Format(logEvent, renderSpace);
            _testOutputHelper.WriteLine(renderSpace.ToString());
        }
    }
}