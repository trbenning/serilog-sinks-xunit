[![Build Status](https://dev.azure.com/benning/Serilog.Sinks.XUnit/_apis/build/status/trbenning.serilog-sinks-xunit?branchName=master)](https://dev.azure.com/benning/Serilog.Sinks.XUnit/_build/latest?definitionId=2&branchName=master)
[![NuGet Version](https://img.shields.io/nuget/v/Serilog.Sinks.XUnit.svg?style=flat)](https://www.nuget.org/packages/Serilog.Sinks.XUnit/)

# serilog-sinks-xunit
The xunit test output sink for Serilog

### What is it?
It's a package that will allow you to use Serilog for test output.

### Installation

```
Install-Package Serilog.Sinks.XUnit
```

### Example usage
```csharp
using System;
using Xunit;
using Xunit.Abstractions;

public class Samples
{
    ILogger _output;

    public Samples(_ITestOutputHelper output)
    {
        // Pass the _ITestOutputHelper object to the TestOutput sink
        _output = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output, Events.LogEventLevel.Verbose)
            .CreateLogger()
            .ForContext<IntegrationTests>();
    }

    [Fact]
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
```
