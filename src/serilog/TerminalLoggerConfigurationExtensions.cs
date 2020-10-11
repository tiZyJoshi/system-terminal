using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Terminal;

namespace Serilog
{
    public static class TerminalLoggerConfigurationExtensions
    {
        public static LoggerConfiguration Terminal(this LoggerSinkConfiguration sinkConfiguration,
            string outputTemplate = "", IFormatProvider? formatProvider = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum, LoggingLevelSwitch? levelSwitch = null,
            LogEventLevel? standardErrorFromLevel = null, TerminalTheme? theme = null)
        {
            _ = sinkConfiguration ?? throw new ArgumentNullException(nameof(sinkConfiguration));

            return sinkConfiguration.Sink(new TerminalSink(outputTemplate, formatProvider, standardErrorFromLevel,
                theme ?? new DefaultTerminalTheme()), restrictedToMinimumLevel, levelSwitch);
        }
    }
}
